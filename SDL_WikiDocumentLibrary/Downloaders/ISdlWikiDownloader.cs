using System.Collections.Generic;

namespace SDL_WikiDocumentLibrary.Downloaders
{
    public interface ISdlWikiDownloader
    {
        IEnumerable<string> GetDocumentPaths();
        SdlWikiDocument GetDocument(string path);
    }
}
