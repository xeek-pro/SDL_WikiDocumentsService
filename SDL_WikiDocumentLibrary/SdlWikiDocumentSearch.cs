using Microsoft.Extensions.Options;
using SDL_WikiDocumentLibrary.Downloaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDL_WikiDocumentLibrary
{
    public class SdlWikiDocumentSearch : ISdlWikiDocumentSearch
    {
        private readonly SdlWikiDocumentSearchSettings _settings;
        private readonly IWikiRepository _repo;
        private readonly ISdlWikiDownloader _downloader;

        public const int DOCUMENT_PATHS_CACHE_EXPIRATION_DAYS = 1;
        private HashSet<string> _documentPathsCache;
        private DateTime _documentPathsCacheLastUpdate;

        public SdlWikiDocumentSearch(IOptionsMonitor<SdlWikiDocumentSearchSettings> optionsMonitor, IWikiRepository repo, ISdlWikiDownloader downloader)
        {
            _settings = optionsMonitor.CurrentValue;
            _repo = repo;
            _downloader = downloader;
        }

        public SdlWikiDocument GetDocument(string name)
        {
            SdlWikiDocument document = _repo.GetDocument(name);
            if(document == null)
            {
                if((document = _downloader.GetDocument(name)) != null)
                {
                    _repo.UpdateDocument(document);
                }
            }

            return document;
        }

        public HashSet<string> GetDocumentPaths() => GetDocumentPaths(out _);
        public HashSet<string> GetDocumentPaths(out bool fromCache)
        {
            if(_documentPathsCache == null || (DateTime.UtcNow - _documentPathsCacheLastUpdate).TotalDays > DOCUMENT_PATHS_CACHE_EXPIRATION_DAYS)
            {
                _documentPathsCache = _downloader.GetDocumentPaths().ToHashSet();
                _documentPathsCacheLastUpdate = DateTime.UtcNow;
                fromCache = false;
            }
            else
            {
                fromCache = true;
            }

            return _documentPathsCache;
        }
    }
}
