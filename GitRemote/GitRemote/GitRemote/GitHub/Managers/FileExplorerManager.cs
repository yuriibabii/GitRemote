using GitRemote.Models;
using GitRemote.Services;
using Newtonsoft.Json.Linq;
using Nito.Mvvm;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GitRemote.GitHub.Managers
{
    public class FileExplorerManager
    {
        private readonly string _login;
        private readonly string _reposName;
        private string _path = string.Empty;
        private readonly RestClient _restClient;
        private NotifyTask<string> _defaultBranch;

        public FileExplorerManager(string login, string reposName)
        {
            _restClient = new RestClient(ConstantsService.GitHubApiLink);
            _login = login;
            _reposName = reposName;
            //_defaultBranch = NotifyTask.Create(GetDefaultBranchAsync());
        }

        public async Task<List<FileExplorerModel>> GetFilesAsync()
        {
            try
            {
                var gitHubFiles = await GetGitHubExplorerItemsAsync(_path="GitRemote/");

                var gitRemoteFiles = new List<FileExplorerModel>();

                foreach ( var file in gitHubFiles )
                {
                    var fileType = file["type"].ToString();
                    if ( fileType == "symlink" || fileType == "submodule" ) continue;

                    var model = new FileExplorerModel
                    {
                        Name = file["name"].ToString(),
                        FileType = fileType
                    };

                    if ( model.IsFolder )
                    {
                        var items = await GetGitHubExplorerItemsAsync(_path + model.Name);
                        model.FoldersCount = GetFoldersCount(items);
                        model.FilesCount = GetFilesCount(items);
                    }

                    model.FileSize = ConvertSize(file["size"]);

                    gitRemoteFiles.Add(model);
                }

                gitRemoteFiles.Sort(Comparison);

                return gitRemoteFiles;
            }
            catch ( WebException ex )
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch ( Exception ex )
            {
                throw new Exception("Getting ExplorerFiles from github failed! " + ex.Message);
            }
        }

        private int Comparison(FileExplorerModel fileExplorerModel, FileExplorerModel explorerModel)
        {
            if ( fileExplorerModel.FileType == "dir" )
                return explorerModel.FileType == "dir" ? 0 : -1;

            return explorerModel.FileType == "dir" ? 1 : 0;
        }

        private async Task<JArray> GetGitHubExplorerItemsAsync(string path = "", string place = null)
        {

            if ( _defaultBranch?.Result == null )
            {
                _defaultBranch = NotifyTask.Create(GetDefaultBranchAsync());
                var task = _defaultBranch.Task;
                await task;
            }

            place = place ?? _defaultBranch.Result;

            var request = new RestRequest($"/repos/{_login}/{_reposName}/contents/{path}?ref={place}", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var arrayOfElements = JArray.Parse(responceResult.Content);

            return arrayOfElements;
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

        private async Task<IEnumerable<string>> GetBranchesNamesAsync()
        {
            var request = new RestRequest($"/repos/{_login}/{_reposName}/branches", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var branches = JArray.Parse(responceResult.Content);
            var branchesNames = branches.Select(repo => repo["name"].ToString());

            return branchesNames;
        }

        private async Task<IEnumerable<string>> GetTagsNamesAsync()
        {
            var request = new RestRequest($"/repos/{_login}/{_reposName}/tags", Method.GET)
            {
                Serializer = { ContentType = "application/json" }
            };

            var responceResult = await _restClient.Execute(request);
            var tags = JArray.Parse(responceResult.Content);
            var tagsNames = tags.Select(tag => tag["name"].ToString());

            return tagsNames;
        }

        private string GetFoldersCount(JArray items)
        {
            var count = items.Count(item => item["type"].ToString() == "dir");

            return Convert.ToString(count);
        }

        private string GetFilesCount(JArray items)
        {
            var count = items.Count(item => item["type"].ToString() == "file");

            return Convert.ToString(count);
        }

        private string ConvertSize(object originalSizeObject)
        {
            var originalSize = Convert.ToInt32(originalSizeObject.ToString());
            var size = originalSize < 1024
                ? Convert.ToString(originalSize) + "B"
                : Convert.ToString(Math.Round(( double )originalSize / 1024, 2)) + "KB";

            return size;
        }
    }
}
