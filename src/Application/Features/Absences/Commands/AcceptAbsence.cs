using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.IRepositories;

namespace Backend.Application.Features.Absences.Commands;

public record AcceptAbsenceCommand(int AbsenceId) : IRequest<Response<string>>;

public class AcceptAbsenceCommandHandler : IRequestHandler<AcceptAbsenceCommand, Response<string>>
{
    private readonly IAbsenceCommandRepository _repository;

    public AcceptAbsenceCommandHandler(IAbsenceCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<string>> Handle(AcceptAbsenceCommand request, CancellationToken cancellationToken)
    {
        await _repository.ApproveAbsenceAsync(request.AbsenceId);
        return new Response<string>("Absence approved successfully");
    }
}
