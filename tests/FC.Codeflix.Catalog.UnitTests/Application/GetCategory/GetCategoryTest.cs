using FC.Codeflix.Catalog.Domain.Repository;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exemploCategory = _fixture.GetValidCategory();

        repositoryMock.Setup(x => x.Get(
                It.Is<Guid>(guid => guid != Guid.Empty),
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(exemploCategory);

        var input = new UseCase.GetCategoryInput(exemploCategory.Id);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(
                It.Is<Guid>(guid => guid != Guid.Empty),
                It.IsAny<CancellationToken>()
        ), Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(exemploCategory.Name);
        output.Description.Should().Be(exemploCategory.Description);
        output.IsActive.Should().Be(exemploCategory.IsActive);
        output.Id.Should().Be(exemploCategory.Id);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesNotExist))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task NotFoundExceptionWhenCategoryDoesNotExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exempleGuid = Guid.NewGuid();

        repositoryMock.Setup(x => x.Get(
                It.Is<Guid>(guid => guid != Guid.Empty),
                It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"Category '{exempleGuid}' not found"));

        var input = new UseCase.GetCategoryInput(exempleGuid);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x => x.Get(
                It.Is<Guid>(guid => guid != Guid.Empty),
                It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}