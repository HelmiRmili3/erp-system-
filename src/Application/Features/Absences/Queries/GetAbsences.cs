using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Application.Features.User.IRepositories;
using Backend.Application.Features.User.Dto;

namespace Backend.Application.Features.Absences.Queries
{
    public record GetAbsencesQuery(PagingParameter PagingParameter) : IRequest<PagedResponse<List<AbsenceDto>>>;

    public class GetAbsencesQueryHandler : IRequestHandler<GetAbsencesQuery, PagedResponse<List<AbsenceDto>>>
    {
        private readonly IAbsenceQueryRepository _repository;
        private readonly IUserQueryRepository _userRepository;

        public GetAbsencesQueryHandler(
            IAbsenceQueryRepository repository,
            IUserQueryRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<PagedResponse<List<AbsenceDto>>> Handle(GetAbsencesQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _repository.GetPagedAsync(
                filter: null,
                pageNumber: request.PagingParameter.PageNumber,
                pageSize: request.PagingParameter.PageSize,
                includeTable: null,
                cancellationToken: cancellationToken
            );

            var dtoList = new List<AbsenceDto>();

            if (pagedResult.Data != null)
            {
                foreach (var absence in pagedResult.Data)
                {
                    UserDataDto? userDto = null;

                    if (!string.IsNullOrWhiteSpace(absence.UserId))
                    {
                        userDto = await _userRepository.GetByIdAsync(absence.UserId, cancellationToken);
                    }

                    dtoList.Add(new AbsenceDto
                    {
                        Id = absence.Id,
                        UserId = absence.UserId,
                        StartDate = absence.StartDate,
                        EndDate = absence.EndDate,
                        AbsenceType = absence.AbsenceType,
                        StatusType = absence.StatusType,
                        Reason = absence.Reason,
                        User = userDto
                    });
                }
            }

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
