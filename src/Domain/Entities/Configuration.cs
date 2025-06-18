using Backend.Domain.Entities;

public class Configuration(string key, string value) : BaseAuditableEntity
{
    public string? Key { get; set; } = key;
    public string? Value { get; set; } = value;

    public void Update(string value)
    {
        Value = value;
    }
}
