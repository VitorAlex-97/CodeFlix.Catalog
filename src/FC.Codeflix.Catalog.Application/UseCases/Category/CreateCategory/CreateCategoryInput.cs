using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategoryInput : IRequest<CategoryModelOutput>
{
    public string Name;
    public string? Description;
    public bool IsActive;

    public CreateCategoryInput(string name, string? description, bool isActive = true)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }
}