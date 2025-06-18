using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Employees.Dto;
using Backend.Application.Features.Employees.IRepositories;
using MediatR;

namespace Backend.Application.Features.Employees.Queries;



public record GetEmployeesQuery : IRequest<Response<IEnumerable<EmployeeDto>>>;
public class GetEmployeesQueryHandler(IEmployeeQueryRepository repository) : IRequestHandler<GetEmployeesQuery, Response<IEnumerable<EmployeeDto>>>
{
    private readonly IEmployeeQueryRepository _repository = repository;
    public async Task<Response<IEnumerable<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var entitys = await _repository.GetAllAsync(cancellationToken);
        return new Response<IEnumerable<EmployeeDto>>(entitys.Select(x => x.ToDto<EmployeeDto>()), "Employees retrieved successfully");
    }
}
