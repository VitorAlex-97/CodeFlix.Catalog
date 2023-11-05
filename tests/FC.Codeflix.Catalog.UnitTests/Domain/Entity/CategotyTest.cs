using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity;

[Collection(nameof(CategoryTestFixture))]
public class CategotyTest
{
    private readonly CategoryTestFixture _categoryFixture;

    public CategotyTest(CategoryTestFixture categoryTestFixture)
    {
        _categoryFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate() 
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBe(null);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBe(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive) 
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBe(null);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBe(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(name, validCategory.Description);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(validCategory.Name, null);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThen3Characters), parameters: 10)]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryFixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    public static IEnumerable<object[]> GetNamesWithLessThen3Characters(int numberOfTests)
    {
        var fixture = new CategoryTestFixture();
        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[]
            {
                fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
            };

        }
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equals 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equals 10000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate() 
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        
        category.Activate();
        category.IsActive.Should().BeTrue();
    }
    
    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate() 
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        
        category.Deactivate();
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Upadate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Upadate()
    {
        var category = _categoryFixture.GetValidCategory();
        var categoryWithNewValues = _categoryFixture.GetValidCategory();
        
        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);
        category.Name.Should().Be(categoryWithNewValues.Name);
        category.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact(DisplayName = nameof(UpadateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpadateOnlyName()
    {
        var category = _categoryFixture.GetValidCategory();
        var categoryWithNewValue = _categoryFixture.GetValidCategory();
        var currentDescription = category.Description;

        category.Update(categoryWithNewValue.Name);
        category.Name.Should().Be(categoryWithNewValue.Name);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = _categoryFixture.GetValidCategory();
        Action action = () => category.Update(name);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpadateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("AB")]
    [InlineData("A")]
    [InlineData("12")]
    [InlineData("2")]
    public void UpadateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = _categoryFixture.GetValidCategory();
        Action action = () => category.Update(invalidName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = _categoryFixture.GetValidCategory();  
        // var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var invalidName = _categoryFixture.Faker.Lorem.Letter(256);
        Action action = () => category.Update(invalidName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equals 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = _categoryFixture.GetValidCategory();      
        var invalidDescription = _categoryFixture.GetValidCategoryDescription();
        while (invalidDescription.Length <= 10_000)
        {
            var description = _categoryFixture.Faker.Commerce.ProductDescription();
            invalidDescription = $"{invalidDescription} {description}";
        }
        Action action = () => category.Update(_categoryFixture.GetValidCategoryName(), invalidDescription);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equals 10000 characters long");
    }
}