using Backend.Application.Common.Parameters;

namespace Backend.Application.Common.Response;

public class PagedResponse<T> : Response<T>
{
    public virtual int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int RecordsFiltered { get; set; }
    public int RecordsTotal { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize, RecordsCount recordsCount)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        RecordsFiltered = recordsCount.RecordsFiltered;
        RecordsTotal = recordsCount.RecordsTotal;
        Data = data;
        Message = string.Empty;
        Succeeded = true;
        Errors = [];
    }
}
