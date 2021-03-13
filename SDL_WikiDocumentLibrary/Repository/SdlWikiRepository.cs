using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SDL_WikiDocumentLibrary.Downloaders;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace SDL_WikiDocumentLibrary
{
    public class SdlWikiRepository : IWikiRepository
    {
        public ConcurrentDictionary<string, SdlWikiDocument> Documents { get; private set; } = new ConcurrentDictionary<string, SdlWikiDocument>();
        public DateTime DateUpdated { get; private set; }
        public bool Modified { get; private set; }

        private readonly SdlWikiRepoSettings _settings;

        public SdlWikiRepository(IOptionsMonitor<SdlWikiRepoSettings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;
        }

        public void Load()
        {
            var json = File.ReadAllText(_settings.RepoFile);
            var data = JsonConvert.DeserializeObject<SdlWikiRepositoryData>(json);

            Documents = new ConcurrentDictionary<string, SdlWikiDocument>(
                data.Documents.ToDictionary(
                    d => d.Name.ToLowerInvariant(),
                    d => d
            ));

            DateUpdated = data.DateUpdated;
        }

        public void Save()
        {
            var data = new SdlWikiRepositoryData
            {
                Documents = Documents.Values.ToList(),
                DateUpdated = DateUpdated
            };

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_settings.RepoFile, json);

            Modified = false;
        }

        public SdlWikiDocument GetDocument(string name)
        {
            if (Documents.TryGetValue(name.ToLowerInvariant(), out SdlWikiDocument document))
            {
                return document;
            }

            return null; // Not found
        }

        public bool UpdateDocument(SdlWikiDocument document)
        {
            var documentName = document.Name.ToLowerInvariant();
            bool added = false;

            Documents.AddOrUpdate(documentName, document, (key, oldValue) =>
            {
                added = true; 
                return document;
            });

            DateUpdated = DateTime.UtcNow;
            Modified = true;

            return added;
        }
    }
}
