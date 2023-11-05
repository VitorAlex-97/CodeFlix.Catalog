using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryFixtureCollection 
    : ICollectionFixture<CreateCategoryTestFixture>
{ }
public class CreateCategoryTestFixture : BaseFixture
{
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

    public CreateCategoryInput GetValidInput()
        => new CreateCategoryInput(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

    public CreateCategoryInput GetInvalidInputShortName()
    {
        //nome não pode ser menor que 3 caracteres
        var invalidInputShortName = new CreateCategoryInput(
            GetValidCategoryName()[..2],
            GetValidCategoryDescription()
        );

        return invalidInputShortName;
    }

    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        //nome não pode ser maior que 255 caracteres
        var tooLongNameForCategory = "";
        while (tooLongNameForCategory.Length <= 255)
        {
            var description = Faker.Commerce.ProductName();
            tooLongNameForCategory = $"{tooLongNameForCategory} {description}";
        }
        var invalidInputTooLongName = new CreateCategoryInput(
            tooLongNameForCategory,
            GetValidCategoryDescription()
        );

        return invalidInputTooLongName;
    }

    public CreateCategoryInput GetInvalidInputDescriptionNull()
    {
        //Description não pode ser nulo
        var invalidInputDescriptionNull = new CreateCategoryInput(
            GetValidCategoryName(),
            null
        );

        return invalidInputDescriptionNull;
    }

    public CreateCategoryInput GetInvalidInputTooLongDescription()
    {
        //Description não pode ser maior que 10.000 caracteres
        var tooLongDescriptionForCategory = "";
        while (tooLongDescriptionForCategory.Length <= 10_000)
        {
            var description = Faker.Commerce.ProductDescription();
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {description}";
        }
        var invalidInputTooLongDesciption = new CreateCategoryInput(
            GetValidCategoryName(),
            tooLongDescriptionForCategory
        );

        return invalidInputTooLongDesciption;
    }
    
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}