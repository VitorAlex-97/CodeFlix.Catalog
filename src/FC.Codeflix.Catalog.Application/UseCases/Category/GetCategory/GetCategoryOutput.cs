using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategoryOutput
{
    public Guid Id;
    public string Name;
    public string Description;
    public bool IsActive;
    public DateTime CreatedAt;

    public GetCategoryOutput(
        Guid id, 
        string name, 
        string description, 
        bool isActive, 
        DateTime createdAt
    )
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static GetCategoryOutput FromCategory(DomainEntity.Category category)
        => new (
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        ); 
}