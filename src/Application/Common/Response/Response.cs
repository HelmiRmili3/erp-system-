namespace Backend.Application.Common.Response;

public class Response<T>
{
    public Response()
    {
    }

    public Response(T data, string? message = null)
    {
        Succeeded = true;
        Message = message ?? string.Empty;
        Data = data;
    }

    public Response(string message)
    {
        Succeeded = false;
        Message = message;
    }

    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<Error> Errors { get; set; } = new();
    public T? Data { get; set; }
    public Exception? Exception { get; set; }
    public SearchMetadata? SearchMetadata { get; set; }
    public string? CorrelationId { get; set; } 

    public Response<T> WithSearchMetadata(SearchMetadata metadata)
    {
        SearchMetadata = metadata;
        return this;
    }

    public Response<T> WithCorrelationId(string correlationId)
    {
        CorrelationId = correlationId;
        return this;
    }

    public Response<T> WithError(string errorMessage, string? errorCode = null)
    {
        Errors.Add(new Error(errorMessage, errorCode));
        Succeeded = false;
        return this;
    }
}

public record SearchMetadata(
    int TotalCount,
    int PageNumber,
    int PageSize,
    string? Query = null,
    string? Filter = null);

public record Error(string Message, string? Code = null);
