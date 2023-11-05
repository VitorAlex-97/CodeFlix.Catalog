using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;
public class GetCategoryTestFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new();

    public string GetValidCategoryName()
    {
        var categoryName = string.Empty;
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        return categoryName.Length > 255 ? categoryName[..255] : categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        return categoryDescription.Length > 10_000 ? categoryDescription[..10_000] : categoryDescription;
    }

    public bool GetRandomBoolean() => new Random().NextDouble() > 0.5;

    public Category GetValidCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription()
        );
}

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureColletion 
    : ICollectionFixture<GetCategoryTestFixture>{}