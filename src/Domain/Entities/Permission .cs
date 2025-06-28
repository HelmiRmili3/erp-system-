
using Backend.Domain.Entities;
public class Permisstion
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; }
    public DateTime CreatedDate { get; set; }

}
