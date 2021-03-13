using System;
using System.Collections.Generic;

namespace SDL_WikiDocumentLibrary
{
    public class SdlWikiDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string DiscordContent { get; set; }
        public string RawContent { get; set; }
        public string Url { get; set; }
        public Dictionary<string, string> MetaText { get; set; } = new Dictionary<string, string>();
        public string[] Categories { get; set; }
        public DateTime DateRetrieved { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
