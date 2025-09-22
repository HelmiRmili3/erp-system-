using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Application.Features.User.IRepositories;
using Backend.Application.Features.User.Dto;
using MediatR;

namespace Backend.Application.Features.Absences.Queries
{
    public record GetAbsenceQuery(int Id) : IRequest<Response<AbsenceDto>>;

    public class GetAbsenceQueryHandler : IRequestHandler<GetAbsenceQuery, Response<AbsenceDto>>
    {
        private readonly IAbsenceQueryRepository _repository;
        private readonly IUserQueryRepository _userRepository;

        public GetAbsenceQueryHandler(
            IAbsenceQueryRepository repository,
            IUserQueryRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<Response<AbsenceDto>> Handle(GetAbsenceQuery request, CancellationToken cancellationToken)
        {
            var absence = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (absence == null)
            {
                return new Response<AbsenceDto>("Absence not found");
            }

            UserDataDto? userDto = null;

            if (!string.IsNullOrWhiteSpace(absence.UserId))
            {
                userDto = await _userRepository.GetByIdAsync(absence.UserId, cancellationToken);
            }

            var dto = new AbsenceDto
            {
                Id = absence.Id,
                UserId = absence.UserId,
                StartDate = absence.StartDate,
                EndDate = absence.EndDate,
                AbsenceType = absence.AbsenceType,
                StatusType = absence.StatusType,
                Reason = absence.Reason,
                User = userDto
            };

            return new Response<AbsenceDto>(dto, "Absence retrieved successfully");
        }
    }
}
