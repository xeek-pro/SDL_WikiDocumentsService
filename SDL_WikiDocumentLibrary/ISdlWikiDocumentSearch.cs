using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDL_WikiDocumentLibrary
{
    public interface ISdlWikiDocumentSearch
    {
        public SdlWikiDocument GetDocument(string name);
        public HashSet<string> GetDocumentPaths();
        public HashSet<string> GetDocumentPaths(out bool fromCache);
    }
}
