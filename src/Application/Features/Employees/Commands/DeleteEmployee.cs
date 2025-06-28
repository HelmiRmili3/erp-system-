using Backend.Application.Common.Response;
using Backend.Application.Features.Employees.IRepositories;

namespace Backend.Application.Features.Employees.Commands;

public record DeleteEmployeeCommand(int id) : IRequest<Response<string>>;
public class DeleteEmployeeCommansHandler(IEmployeeCommandRepository repository, IEmployeeQueryRepository queryRepository) : IRequestHandler<DeleteEmployeeCommand, Response<string>>
{
    private readonly IEmployeeCommandRepository _repository = repository;
    private readonly IEmployeeQueryRepository _queryRepository = queryRepository;

    public async Task<Response<string>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {

        var entity = await _queryRepository.GetByIdAsync(request.id, cancellationToken);
        await _repository.DeleteAsync(entity!, cancellationToken);
        return new Response<string>("Employee deleted successfully");
    }
}
