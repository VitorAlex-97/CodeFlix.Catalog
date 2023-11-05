using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.UnitTests.Common;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity;

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection 
    : ICollectionFixture<CategoryTestFixture> 
{}

public class CategoryTestFixture : BaseFixture 
{
    public CategoryTestFixture() : base()
    {
    }

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
    public Category GetValidCategory() 
        => new(GetValidCategoryName(), GetValidCategoryDescription());
}
