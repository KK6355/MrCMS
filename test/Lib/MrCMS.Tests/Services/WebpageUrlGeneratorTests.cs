using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using MrCMS.Helpers;
using MrCMS.Models;
using MrCMS.Services;
using MrCMS.Settings;
using MrCMS.Tests.Stubs;
using MrCMS.TestSupport;
using Xunit;

namespace MrCMS.Tests.Services
{
    // public class WebpageUrlGeneratorTests : InMemoryDatabaseTest
    // {
    //     private readonly IEnsureWebpageUrlIsValid _ensureWebpageUrlIsValid;
    //     private readonly WebpageUrlService _webpageUrlService;
    //     private readonly PageDefaultsSettings _pageDefaultsSettings;
    //
    //     public WebpageUrlGeneratorTests()
    //     {
    //         _ensureWebpageUrlIsValid = A.Fake<IEnsureWebpageUrlIsValid>();
    //         _pageDefaultsSettings = new PageDefaultsSettings();
    //         _webpageUrlService =
    //             new WebpageUrlService(_urlValidationService, Session, ServiceProvider, _pageDefaultsSettings);
    //     }
    //
    //     [Fact]
    //     public async Task WebpageUrlGenerator_GetDocumentUrl_ReturnsAUrlBasedOnTheHierarchyIfTheFlagIsSetToTrue()
    //     {
    //         var textPage = new BasicMappedWebpage {Name = "Test Page", UrlSegment = "test-page", Site = CurrentSite};
    //
    //         await Session.TransactAsync(session => session.SaveOrUpdateAsync(textPage));
    //
    //         string documentUrl = await _webpageUrlService.Suggest(TODO, new SuggestParams
    //         {
    //             PageName = "Nested Page",
    //             ParentId = textPage.Id,
    //             WebpageType = typeof(BasicMappedWebpage).FullName,
    //             UseHierarchy = true
    //         });
    //
    //         documentUrl.Should().Be("test-page/nested-page");
    //     }
    //
    //     [Fact]
    //     public async Task WebpageUrlGenerator_GetDocumentUrl_ReturnsAUrlBasedOnTheNameIfTheFlagIsSetToFalse()
    //     {
    //         var textPage = new BasicMappedWebpage {Name = "Test Page", UrlSegment = "test-page", Site = CurrentSite};
    //
    //         await Session.TransactAsync(session => session.SaveOrUpdateAsync(textPage));
    //
    //         string documentUrl = await _webpageUrlService.Suggest(TODO, new SuggestParams
    //         {
    //             PageName = "Nested Page",
    //             ParentId = textPage.Id,
    //             WebpageType = typeof(BasicMappedWebpage).FullName,
    //             UseHierarchy = false
    //         });
    //
    //         documentUrl.Should().Be("nested-page");
    //     }
    //
    //     [Fact]
    //     public async Task WebpageUrlGenerator_GetDocumentUrlWithExistingName_ShouldReturnTheUrlWithADigitAppended()
    //     {
    //         var parent = new BasicMappedWebpage {Name = "Parent", UrlSegment = "parent", Site = CurrentSite};
    //         var textPage = new BasicMappedWebpage
    //         {
    //             Name = "Test Page",
    //             Parent = parent,
    //             UrlSegment = "parent/test-page",
    //             Site = CurrentSite
    //         };
    //         await Session.TransactAsync(async session =>
    //         {
    //             await session.SaveOrUpdateAsync(parent);
    //             await session.SaveOrUpdateAsync(textPage);
    //         });
    //         A.CallTo(() => _urlValidationService.UrlIsValidForWebpage("parent/test-page/nested-page", null))
    //             .Returns(false);
    //
    //         string documentUrl = await _webpageUrlService.Suggest(TODO, new SuggestParams
    //         {
    //             PageName = "Nested Page",
    //             ParentId = textPage.Id,
    //             WebpageType = typeof(BasicMappedWebpage).FullName,
    //             UseHierarchy = true
    //         });
    //
    //         documentUrl.Should().Be("parent/test-page/nested-page-1");
    //     }
    //
    //     [Fact]
    //     public async Task
    //         WebpageUrlGenerator_GetDocumentUrlWithExistingName_MultipleFilesWithSameNameShouldNotAppendMultipleDigits()
    //     {
    //         var parent = new BasicMappedWebpage {Name = "Parent", UrlSegment = "parent", Site = CurrentSite};
    //         var textPage = new BasicMappedWebpage
    //         {
    //             Name = "Test Page",
    //             Parent = parent,
    //             UrlSegment = "parent/test-page",
    //             Site = CurrentSite
    //         };
    //         await Session.TransactAsync(async session =>
    //         {
    //             await session.SaveOrUpdateAsync(parent);
    //             await session.SaveOrUpdateAsync(textPage);
    //         });
    //         A.CallTo(() => _urlValidationService.UrlIsValidForWebpage("parent/test-page/nested-page", null))
    //             .Returns(false);
    //         A.CallTo(() => _urlValidationService.UrlIsValidForWebpage("parent/test-page/nested-page-1", null))
    //             .Returns(false);
    //
    //         string documentUrl = await _webpageUrlService.Suggest(TODO, new SuggestParams
    //         {
    //             PageName = "Nested Page",
    //             ParentId = textPage.Id,
    //             WebpageType = typeof(BasicMappedWebpage).FullName,
    //             UseHierarchy = true
    //         });
    //
    //         documentUrl.Should().Be("parent/test-page/nested-page-2");
    //     }
    // }
}