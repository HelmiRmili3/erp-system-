namespace Backend.Application.Common.Parameters;

public class QueryParameter : PagingParameter
{
    public virtual string? OrderBy { get; set; }
    public virtual string? OrderByThenBy { get; set; }
    public virtual string? OrderByDirection { get; set; }
    public virtual string? Fields { get; set; }
}
