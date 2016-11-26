using GitRemote.Models;
using GitRemote.Services;
using Microsoft.Practices.ObjectBuilder2;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GitRemote.GitHub.Managers
{
    public class FileTreeManager
    {
        private readonly string _login;
        private readonly string _reposName;
        private readonly RestClient _restClient;
        private List<string> _branches;
        private string _currentBranch;
        private readonly List<string> _currentPath;
        private List<Dictionary<string, List<FileExplorerModel>>> _tree;

        public FileTreeManager(string login, string reposName)
        {
            _restClient = new RestClient(ConstantsService.GitHubApiLink);
            _login = login;
            _reposName = reposName;
            _currentPath = new List<string>();
        }

        public ObservableCollection<FileExplorerModel> GetFiles(string path)
        {
            var collection = new ObservableCollection<FileExplorerModel>();
            if ( StringService.CheckForNullOrEmpty(path) )
                _currentPath.Add(path);
            var stringPath = _currentPath.JoinStrings("");
            var index = _currentPath.Count - 1;
            _tree[index][stringPath].Sort(Comparison);

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
                _currentBranch = branch;
            else
                _currentBranch = await GetDefaultBranchAsync();
        }

        public async Task SetTreeAsync()
        {
            _tree = await GetTreeAsync();
        }

        private async Task<JArray> GetJsonTreeAsync()
        {
            var request = new RestRequest($"/repos/{_login}/{_reposName}/git/trees/{_currentBranch}?recursive=1", Method.GET)
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

            var tree = new List<Dictionary<string, List<FileExplorerModel>>>();

            try
            {
                foreach ( var element in jsonTree )
                {
                    var fileType = element["type"].ToString();

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
            }
            catch ( Exception ex )
            {

                throw;
            }

            return tree;
        }

        private string ConvertSize(object originalSizeObject)
        {
            var originalSize = Convert.ToInt32(originalSizeObject.ToString());
            var size = originalSize < 1024
                ? Convert.ToString(originalSize) + "B"
                : Convert.ToString(Math.Round(( double )originalSize / 1024, 2)) + "KB";

            return size;
        }

        private async Task<string> GetDefaultBranchAsync()
        {
            var request = new RestRequest($"/repos/{_login}/{_reposName}", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var repos = JObject.Parse(responceResult.Content);

            return repos["default_branch"].ToString();
        }

        private async Task<List<string>> GetBranchesAsync()
        {
            var request = new RestRequest($"/repos/{_login}/{_reposName}/branches", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var jsonBranches = JArray.Parse(responceResult.Content);

            var branches = jsonBranches.Select(branch => branch["name"].ToString()).ToList();

            return branches;
        }

        private int Comparison(FileExplorerModel fileExplorerModel, FileExplorerModel explorerModel)
        {
            if ( fileExplorerModel.FileType == "dir" )
                return explorerModel.FileType == "dir" ? 0 : -1;

            return explorerModel.FileType == "dir" ? 1 : 0;
        }

        public bool PopUpExplorer()
        {
            if ( _currentPath.Count <= 1 ) return false;

            _currentPath.RemoveAt(_currentPath.Count - 1);
            return true;
        }
    }
}
