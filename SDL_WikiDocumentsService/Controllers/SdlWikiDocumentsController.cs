using Microsoft.AspNetCore.Mvc;
using SDL_WikiDocumentLibrary;
using System.Linq;

namespace SDL_WikiDocumentsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SdlWikiDocumentsController : ControllerBase
    {
        private readonly ISdlWikiDocumentSearch _docSearch;

        public SdlWikiDocumentsController(ISdlWikiDocumentSearch docSearch)
        {
            _docSearch = docSearch;
        }

        // GET: api/<SdlRepositoryController>
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<SdlWikiDocument> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();
            return Ok(_docSearch.GetDocument(name));
        }

        // GET: api/<SdlRepositoryController>/list
        [HttpGet]
        [Route("list")]
        [Produces("application/json")]
        public ActionResult<string[]> GetDocumentsList()
        {
            return Ok(_docSearch.GetDocumentPaths().ToArray());
        }
    }
}
