using Bogus;
using FluentAssertions;
using FC.Codeflix.Catalog.Domain.Validation;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;
public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidatin - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        Action action = () => DomainValidation.NotNull(value, fieldName);
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidatin - Validation")]
    public void NotNullThrowWhenNull()
    {
        string value = null;
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.NotNull(value, fieldName);
        action.Should()
        .Throw<EntityValidationException>()
        .WithMessage($"{fieldName} should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [Trait("Domain", "DomainValidatin - Validation")]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
        action.Should()
        .Throw<EntityValidationException>()
        .WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidatin - Validation")]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.NotNull(target, fieldName);
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidatin - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
        action.Should()
        .Throw<EntityValidationException>()
        .WithMessage($"{fieldName} should be at least {minLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 10 };
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            var target = faker.Commerce.ProductName();
            var minLength = target.Length + (new Random().Next(1, 20));
            yield return new object[] { target, minLength };
        }
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidatin - Validation")]
    [MemberData(nameof(GetValueGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValueGreaterThanMin(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            var target = faker.Commerce.ProductName();
            var minLength = target.Length - (new Random().Next(1, 5));
            yield return new object[] { target, minLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGraterThanMax))]
    [Trait("Domain", "DomainValidatin - Validation")]
    [MemberData(nameof(GetValuesGreateThanMax), parameters: 10)]
    public void MaxLengthThrowWhenGraterThanMax(string target, int maxLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equals {maxLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesGreateThanMax(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 5 };          
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            var target = faker.Commerce.ProductName();
            var maxLength = target.Length - (new Random().Next(1, 5));
            yield return new object[] { target, maxLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidatin - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void MaxLengthOk(string target, int maxLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 6 };          
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            var target = faker.Commerce.ProductName();
            var maxLength = target.Length + (new Random().Next(0, 5));
            yield return new object[] { target, maxLength };
        }
    }
}