using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.Features.Absences.Queries
{
    public record GetAbsencesQuery : IRequest<Response<List<AbsenceDto>>>;

    public class GetAbsencesQueryHandler : IRequestHandler<GetAbsencesQuery, Response<List<AbsenceDto>>>
    {
        private readonly IAbsenceQueryRepository _repository;

        public GetAbsencesQueryHandler(IAbsenceQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<List<AbsenceDto>>> Handle(GetAbsencesQuery request, CancellationToken cancellationToken)
        {
            var absences = await _repository.GetAllAsync(cancellationToken);

            var absenceDtos = absences
                .Select(a => a.ToDto<AbsenceDto>())
                .ToList();

            return new Response<List<AbsenceDto>>(absenceDtos);
        }
    }
}
