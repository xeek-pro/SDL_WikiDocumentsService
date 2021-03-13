using System;

namespace SDL_WikiDocumentLibrary
{
    public class SdlWikiRepoSettings
    {
        public string RepoFile { get; set; } = $"/SdlRepository/SdlWikiRepository.json";
        public TimeSpan DocumentExpiration { get; set; } = TimeSpan.FromDays(14);
    }
}
