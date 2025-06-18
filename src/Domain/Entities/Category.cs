using System.Text.Json.Serialization;
using Backend.Domain.Entities;


public class Category : BaseAuditableEntity
{
    public string? Logo { get; set; }
    public string? Name { get; set; }
    public int? ParentCategoryId { get; set; }

    [JsonIgnore]
    public virtual Category? ParentCategory { get; set; }
    [JsonIgnore]
    public virtual ICollection<Category>? SubCategories { get; set; }

    public void Update(string? name, int? parentCategoryId, string? logo)
    {
        Name = name;
        if(logo != null)
        {
            Logo = logo;
        }
        ParentCategoryId = parentCategoryId;
    }
}
