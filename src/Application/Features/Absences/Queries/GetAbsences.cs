using Backend.Application.Common.Extensions;
using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;

namespace Backend.Application.Features.Absences.Queries
{
    public record GetAbsencesQuery(PagingParameter PagingParameter) : IRequest<PagedResponse<List<AbsenceDto>>>;

    public class GetAbsencesQueryHandler : IRequestHandler<GetAbsencesQuery, PagedResponse<List<AbsenceDto>>>
    {
        private readonly IAbsenceQueryRepository _repository;

        public GetAbsencesQueryHandler(IAbsenceQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResponse<List<AbsenceDto>>> Handle(GetAbsencesQuery request, CancellationToken cancellationToken)
        {
            // Pass filter as null to get all, and includeTable as null or empty string if no navigation properties needed
            var pagedResult = await _repository.GetPagedAsync(
                filter: null,             // No filter; get all records
                pageNumber: request.PagingParameter.PageNumber,
                pageSize: request.PagingParameter.PageSize,
                includeTable: null,       // or "" if you want
                cancellationToken: cancellationToken
            );

            // pagedResult is PagedResponse<List<Absence>>
            // Map each Absence entity to AbsenceDto
            var dtoList = pagedResult.Data?.Select(a => a.ToDto<AbsenceDto>()).ToList() ?? new List<AbsenceDto>();

            // Return a new PagedResponse of List<AbsenceDto> with same pagination metadata
            return new PagedResponse<List<AbsenceDto>>(
                data: dtoList,
                pageNumber: pagedResult.PageNumber,
                pageSize: pagedResult.PageSize,
                recordsCount: new RecordsCount
                {
                    RecordsFiltered = pagedResult.RecordsFiltered,
                    RecordsTotal = pagedResult.RecordsTotal
                }
            );
        }
    }
}
