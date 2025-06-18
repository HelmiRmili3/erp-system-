namespace Backend.Application.Common.Models;

public class ImageUpdateRequest
{
    public int? Id { get; set; }
    public bool IsMain { get; set; }
    public string? AltText { get; set; }
    public bool ToBeDeleted { get; set; }
}
