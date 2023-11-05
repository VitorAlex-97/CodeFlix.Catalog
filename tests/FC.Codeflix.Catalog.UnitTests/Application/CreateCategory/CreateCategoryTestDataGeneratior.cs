namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;
public class CreateCategoryTestDataGeneratior
{
    public static IEnumerable<object[]>GetInvalidInputs(int times = 12)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidsInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidsInputsList.Add(new object[] {
                        fixture.GetInvalidInputShortName(),
                        "Name should be at least 3 characters long"
                    });
                    break;
                case 1:
                    invalidsInputsList.Add(new object[] {
                        fixture.GetInvalidInputTooLongName(),
                        "Name should be less or equals 255 characters long"
                    });
                    break;
                case 2:
                    invalidsInputsList.Add(new object[] {
                        fixture.GetInvalidInputDescriptionNull(),
                        "Description should not be null"
                    });
                    break;
                case 3:
                    invalidsInputsList.Add(new object[] {
                        fixture.GetInvalidInputTooLongDescription(),
                        "Description should be less or equals 10000 characters long"
                    });
                    break;
                default:
                    break;
            }
        }

        return invalidsInputsList;
    }
}