using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Employees.Dto;
using Backend.Application.Features.Employees.IRepositories;
using MediatR;

namespace Backend.Application.Features.Employees.Queries;

public record GetEmployeeQuery(int id) : IRequest<Response<EmployeeDto>>;

public class GetEmployeeQueryHandler(IEmployeeCommandRepository repository, IEmployeeQueryRepository queryRepository) : IRequestHandler<GetEmployeeQuery, Response<EmployeeDto>>
{
    private readonly IEmployeeCommandRepository _repository = repository;
    private readonly IEmployeeQueryRepository _queryRepository = queryRepository;
    public async Task<Response<EmployeeDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _queryRepository.GetByIdAsync(request.id, cancellationToken);
        Guard.Against.NotFound(request.id, entity);
        return new Response<EmployeeDto>(entity.ToDto<EmployeeDto>(), "Employee retrieved successfulyy");

    }
}
