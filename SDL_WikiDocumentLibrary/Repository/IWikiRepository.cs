using System;

namespace SDL_WikiDocumentLibrary
{
    public interface IWikiRepository
    {
        /// <summary>
        /// The last time any document in this repository has been updated (downloaded from remote source)
        /// </summary>
        public DateTime DateUpdated { get; }

        public void Load();

        public void Save();

        /// <summary>
        /// Locates a document by exact name or downloads it if necessary
        /// </summary>
        /// <param name="name">The exact name of the document (case-insensitive)</param>
        /// <returns>The found document or null if it cannot be found</returns>
        public SdlWikiDocument GetDocument(string name);

        /// <summary>
        /// Updates an existing or adds a new a document into the repository
        /// </summary>
        /// <param name="document">The document to update, using the lower-case name to update an existing document</param>
        /// <returns>True if the document did not previously exist and was added</returns>
        public bool UpdateDocument(SdlWikiDocument document);
    }
}
