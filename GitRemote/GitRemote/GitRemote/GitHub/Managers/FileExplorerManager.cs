using GitRemote.DI;
using GitRemote.Models;
using GitRemote.Services;
using Microsoft.Practices.ObjectBuilder2;
using Newtonsoft.Json.Linq;
using Octokit;
using Octokit.Internal;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GitRemote.GitHub.Managers
{
    public class FileExplorerManager
    {
        private readonly string _ownerName;
        private readonly string _reposName;
        private readonly RestClient _restClient;
        private readonly GitHubClient _gitHubClient;
        public string CurrentBranch { get; private set; }
        private readonly List<string> _currentPath;
        private List<Dictionary<string, List<FileExplorerModel>>> _tree;

        public FileExplorerManager(Session session, string ownerName, string reposName)
        {
            _restClient = new RestClient(ConstantsService.GitHubApiLink)
            {
                Authenticator = new HttpBasicAuthenticator
                    (new NetworkCredential(session.Login, session.GetToken()), AuthHeader.Www)
            };

            _ownerName = ownerName;
            _reposName = reposName;
            _currentPath = new List<string>();

            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session.GetToken())));
        }

        /// <summary>
        /// Gets Files from tree, that is dowloanded early
        /// </summary>
        /// <param name="pathPart"></param>
        /// <returns>Observable collection of files, these are placed on current path</returns>
        public ObservableCollection<FileExplorerModel> GetFiles(string pathPart)
        {
            var collection = new ObservableCollection<FileExplorerModel>();
            if ( StringService.CheckForNullOrEmpty(pathPart) )
                _currentPath.Add(pathPart);
            var stringPath = _currentPath.JoinStrings("");
            var index = _currentPath.Count - 1;

            foreach ( var file in _tree[index][stringPath] )
            {
                if ( file.FileType == "dir" )
                {
                    var files = 0;
                    var folders = 0;

                    foreach ( var subFile in _tree[index + 1][stringPath + file.Name + '/'] )
                    {
                        if ( subFile.FileType == "file" )
                            files++;
                        else
                            folders++;
                    }

                    file.FilesCount = Convert.ToString(files);
                    file.FoldersCount = Convert.ToString(folders);
                }
                collection.Add(file);
            }

            return collection;
        }

        public async Task SetCurrentBranchAsync(string branch = "")
        {
            if ( StringService.CheckForNullOrEmpty(branch) )
                CurrentBranch = branch;
            else
                CurrentBranch = await GetDefaultBranchAsync();
        }

        public void ClearCurrentPath()
        {
            _currentPath.RemoveAll(el => true);
        }

        public async Task SetTreeAsync()
        {
            _tree = await GetTreeAsync();
        }

        private async Task<JArray> GetJsonTreeAsync()
        {
            var request = new RestRequest($"/repos/{_ownerName}/{_reposName}/git/trees/{CurrentBranch}?recursive=1", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var responceObject = JObject.Parse(responceResult.Content);
            var arrayOfElements = JArray.Parse(responceObject["tree"].ToString());

            return arrayOfElements;
        }

        private async Task<List<Dictionary<string, List<FileExplorerModel>>>> GetTreeAsync()
        {
            var jsonTree = await GetJsonTreeAsync();

            // Tree it is just levels of path deep, and each level has Dictionary with key, and files, that are relative with key
            var tree = new List<Dictionary<string, List<FileExplorerModel>>>();

            try
            {
                foreach ( var element in jsonTree )
                {
                    var fileType = element["type"].ToString();
                    if ( fileType == "commit" ) continue;

                    fileType = fileType == "blob" ? "file" : "dir";

                    var path = string.Concat(_reposName, '/', element["path"].ToString());
                    var pathParts = path.Split('/');

                    var model = new FileExplorerModel
                    {
                        Name = pathParts[pathParts.Length - 1],
                        FileType = fileType,
                    };

                    if ( model.FileType == "file" )
                        model.FileSize = ConvertSize(element["size"]);

                    var index = pathParts.Length - 2; // "-2" Because a name is also count
                    var pathWithoutName = path.Substring(0, path.Length - pathParts[pathParts.Length - 1].Length);

                    if ( tree.Count <= index )
                        tree.Add(new Dictionary<string, List<FileExplorerModel>>());

                    if ( !tree[index].ContainsKey(pathWithoutName) )
                        tree[index].Add(pathWithoutName, new List<FileExplorerModel>());

                    tree[index][pathWithoutName].Add(model);
                }

                foreach ( var level in tree )
                {
                    foreach ( var group in level )
                    {
                        group.Value.Sort(Comparison);
                    }
                }
            }
            catch ( Exception ex )
            {
                throw new Exception(ex.Message + "GetTreeAsync Failed");
            }

            return tree;
        }

        /// <summary>
        /// Converts to KB's if it possible
        /// </summary>
        /// <param name="originalSizeObject">object, that is integer and equal to file size in bytes</param>
        /// <returns>B's or KB's</returns>
        private string ConvertSize(object originalSizeObject)
        {
            var originalSize = Convert.ToInt32(originalSizeObject.ToString());
            var size = originalSize < 1024
                ? Convert.ToString(originalSize) + "B"
                : Convert.ToString(Math.Round(( double )originalSize / 1024, 2)) + "KB";

            return size;
        }

        public async Task<string> GetDefaultBranchAsync()
        {
            var request = new RestRequest($"/repos/{_ownerName}/{_reposName}", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var repos = JObject.Parse(responceResult.Content);

            return repos["default_branch"].ToString();
        }

        public async Task<List<string>> GetBranchesAsync()
        {
            var request = new RestRequest($"/repos/{_ownerName}/{_reposName}/branches", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var jsonBranches = JArray.Parse(responceResult.Content);

            var branches = jsonBranches.Select(branch => branch["name"].ToString()).ToList();

            return branches;
        }

        public async Task<List<string>> GetTagsNamesAsync()
        {
            var request = new RestRequest($"/repos/{_ownerName}/{_reposName}/tags", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var tags = JArray.Parse(responceResult.Content);
            var tagsNames = tags.Select(tag => tag["name"].ToString()).ToList();

            return tagsNames;
        }

        /// <summary>
        /// Sorts files, in alphabet order, where dirs goes first
        /// </summary>
        /// <param name="fileExplorerModel">First file</param>
        /// <param name="explorerModel">Another file</param>
        /// <returns></returns>
        private int Comparison(FileExplorerModel fileExplorerModel, FileExplorerModel explorerModel)
        {
            if ( fileExplorerModel.FileType == "dir" )
                return explorerModel.FileType == "dir" ? 0 : -1;

            return explorerModel.FileType == "dir" ? 1 : 0;
        }

        /// <summary>
        /// PopUps last part in path
        /// </summary>
        /// <returns>If successful</returns>
        public bool PopUpExplorer()
        {
            if ( _currentPath.Count <= 1 ) return false;

            _currentPath.RemoveAt(_currentPath.Count - 1);
            return true;
        }

        /// <summary>
        /// PopUps range of parts in path
        /// </summary>
        /// <param name="index">Position of first part to PopUp</param>
        public void PopUpExplorerToIndex(int index)
        {
            _currentPath.RemoveRange(index, _currentPath.Count - index);
        }

        public async Task StarRepository()
        {
            await _gitHubClient.Activity.Starring.StarRepo(_ownerName, _reposName);
        }

        public async Task UnstarRepository()
        {
            await _gitHubClient.Activity.Starring.RemoveStarFromRepo(_ownerName, _reposName);
        }

        public async Task ForkRepository()
        {
            await _gitHubClient.Repository.Forks.Create(_ownerName, _reposName, new NewRepositoryFork());
        }

        public async Task OpenInBrowser(IDevice device)
        {
            await device.LaunchUriAsync(new Uri($"{ConstantsService.GitHubOfficialPageUrl}{_ownerName}/{_reposName}"));
        }

    }
}
