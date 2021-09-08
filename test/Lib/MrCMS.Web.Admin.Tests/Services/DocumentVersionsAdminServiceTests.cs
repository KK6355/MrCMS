using System.Threading.Tasks;
using FluentAssertions;
using MrCMS.Entities.Documents;
using MrCMS.TestSupport;
using MrCMS.Web.Admin.Services;
using Xunit;

namespace MrCMS.Web.Admin.Tests.Services
{
    public class DocumentVersionsAdminServiceTests
    {
        public DocumentVersionsAdminServiceTests()
        {
            _documentVersionRepository = new InMemoryRepository<DocumentVersion>();
            _inMemoryRepository = new InMemoryRepository<Document>();
            _documentVersionsAdminService = new DocumentVersionsAdminService(_documentVersionRepository,
                _inMemoryRepository);
        }

        private readonly DocumentVersionsAdminService _documentVersionsAdminService;
        private readonly InMemoryRepository<DocumentVersion> _documentVersionRepository;
        private readonly InMemoryRepository<Document> _inMemoryRepository;

        [Fact]
        public async Task DocumentVersionService_GetDocumentVersion_GetsTheVersionWithTheRequestedId()
        {
            var documentVersion = new DocumentVersion();
            await _documentVersionRepository.Add(documentVersion);

            var version = _documentVersionsAdminService.GetDocumentVersion(documentVersion.Id);

            version.Should().Be(documentVersion);
        }
    }
}