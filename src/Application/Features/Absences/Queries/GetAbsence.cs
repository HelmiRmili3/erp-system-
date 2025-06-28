using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.Features.Absences.Queries
{
    public record GetAbsenceQuery(int Id) : IRequest<Response<AbsenceDto>>;

    public class GetAbsenceQueryHandler : IRequestHandler<GetAbsenceQuery, Response<AbsenceDto>>
    {
        private readonly IAbsenceQueryRepository _repository;

        public GetAbsenceQueryHandler(IAbsenceQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<AbsenceDto>> Handle(GetAbsenceQuery request, CancellationToken cancellationToken)
        {
            var absence = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (absence == null)
            {
                return new Response<AbsenceDto>("Absence not found");
            }

            return new Response<AbsenceDto>(absence.ToDto<AbsenceDto>());
        }
    }
}
