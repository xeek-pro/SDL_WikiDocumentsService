using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using SDL_WikiDocumentLibrary.Downloaders;
using System;
using System.Linq;

namespace SDL_WikiDocumentTests
{
    public class SdlWikiGitHubDownloaderIntegrationTests
    {
        SdlWikiDownloaderSettings settings = null;
        SdlWikiGitHubDownloader gitHubDownloader = null;

        [SetUp]
        public void Setup()
        {
            settings = new SdlWikiDownloaderSettings();
            var optionsMonitorMock = Substitute.For<IOptionsMonitor<SdlWikiDownloaderSettings>>();
            optionsMonitorMock.CurrentValue.Returns(settings);

            gitHubDownloader = new SdlWikiGitHubDownloader(optionsMonitorMock);
        }

        [Test]
        public void ConnectAndCheckRepo()
        {
            Assert.That(gitHubDownloader.CheckGitHubRepo(), Is.True);
        }

        [Test]
        public void GetAllFilePaths()
        {
            var filePaths = gitHubDownloader.GetDocumentPaths()?.ToList();

            Assert.That(filePaths, Is.Not.Null);
            Assert.That(filePaths, Is.Not.Empty);
        }

        [TestCase("SDL_CreateWindow.mediawiki", "SDL_CreateWindow", "https://raw.githubusercontent.com/libsdl-org/sdlwiki/main/SDL_CreateWindow.mediawiki")]
        public void GetRepoDocumentFromPath(string path, string expectedName, string expectedUrl)
        {
            var currentDateTime = DateTime.UtcNow;
            var document = gitHubDownloader.GetDocument(path);

            Assert.That(document, Is.Not.Null);
            Assert.That(document.Name, Is.EqualTo(expectedName));
            Assert.That(document.Url, Is.EqualTo(expectedUrl));
            Assert.That(document.RawContent, Is.Not.Empty);
            Assert.That(document.DateRetrieved, Is.GreaterThanOrEqualTo(currentDateTime));
        }
    }
}