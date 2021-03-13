using Microsoft.Extensions.Options;
using NUnit.Framework;
using SDL_WikiDocumentLibrary;
using NSubstitute;

namespace SDL_WikiDocumentTests
{
    public class SdlWikiRepositoryTests
    {
        SdlWikiRepoSettings settings = null;

        [SetUp]
        public void Setup()
        {
            settings = new SdlWikiRepoSettings();
        }

        [Test]
        public void Save()
        {
            var optionsMonitorMock = Substitute.For<IOptionsMonitor<SdlWikiRepoSettings>>();
            optionsMonitorMock.CurrentValue.Returns(settings);

            var repo = new SdlWikiRepository(optionsMonitorMock);
            repo.Save();
        }
    }
}
