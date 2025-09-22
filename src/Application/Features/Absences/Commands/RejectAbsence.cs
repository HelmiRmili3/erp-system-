using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.IRepositories;

namespace Backend.Application.Features.Absences.Commands;

public record RejectAbsenceCommand(int AbsenceId) : IRequest<Response<string>>;

public class RejectAbsenceCommandHandler : IRequestHandler<RejectAbsenceCommand, Response<string>>
{
    private readonly IAbsenceCommandRepository _repository;

    public RejectAbsenceCommandHandler(IAbsenceCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<string>> Handle(RejectAbsenceCommand request, CancellationToken cancellationToken)
    {

        await _repository.RejectAbsenceAsync(request.AbsenceId);
        return new Response<string>("Absence rejected successfully");
    }
}
