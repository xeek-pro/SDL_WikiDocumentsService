using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SDL_WikiDocumentLibrary.Downloaders
{
    public class SdlWikiGitHubDownloader : ISdlWikiDownloader
    {
        private readonly SdlWikiDownloaderSettings _settings;
        private readonly GitHubClient _client;

        public SdlWikiGitHubDownloader(IOptionsMonitor<SdlWikiDownloaderSettings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;
            _client = new GitHubClient(new ProductHeaderValue(_settings.GitHubProductHeader));
        }

        public bool CheckGitHubRepo()
        {
            try
            {
                return null != _client.Repository.Get(_settings.GitHubRepoUser, _settings.GitHubRepoName).Result;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<string> GetDocumentPaths()
        {
            var contents = _client.Repository.Content.GetAllContents(_settings.GitHubRepoUser, _settings.GitHubRepoName).Result;

            foreach(var content in contents)
            {
                if (content.Type.Value == ContentType.File)
                    yield return Path.GetFileNameWithoutExtension(content.Path);
            }
        }

        public SdlWikiDocument GetDocument(string path)
        {
            if (!path.EndsWith(".mediawiki")) path += ".mediawiki";

            var contents = _client.Repository.Content.GetAllContents(_settings.GitHubRepoUser, _settings.GitHubRepoName, path).Result;
            if(contents?.Any() == true)
            {
                var content = contents.ElementAt(0);

                DateTime? lastCommitDate = null;
                try
                {
                    var commits = _client.Repository.Commit.GetAll(_settings.GitHubRepoUser, _settings.GitHubRepoName, new CommitRequest() { Path = path }, new ApiOptions() { PageCount = 1, PageSize = 1 }).Result;
                    lastCommitDate = commits.ElementAtOrDefault(0).Commit.Committer.Date.UtcDateTime;
                }
                catch { }

                var document = new SdlWikiDocument()
                {
                    Name = Path.GetFileNameWithoutExtension(content.Name),
                    Url = content.DownloadUrl,
                    RawContent = content.Content,
                    DateRetrieved = DateTime.UtcNow,
                    DateUpdated = lastCommitDate
                };

                return document;
            }
            else
            {
                throw new Exception("GitHub Repository Contents Client failed to get a result from path");
            }
        }
    }
}
