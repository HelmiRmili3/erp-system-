using Backend.Application.Common.Response;
using Backend.Application.Features.Contracts.Dtos;
using Backend.Domain.Entities;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Extensions;
using MediatR;

namespace Backend.Application.Features.Contracts.Queries;

public record GetContractByIdQuery(int Id) : IRequest<Response<ContractDto>>;

public class GetContractByIdQueryHandler : IRequestHandler<GetContractByIdQuery, Response<ContractDto>>
{
    private readonly IQueryRepository<Contract> _repository;

    public GetContractByIdQueryHandler(IQueryRepository<Contract> repository)
    {
        _repository = repository;
    }

    public async Task<Response<ContractDto>> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<ContractDto>($"Contract with ID {request.Id} not found.")
                .WithError("Not Found", "404");
        }

        var dto = entity.ToDto<ContractDto>();

        return new Response<ContractDto>(dto, "Contract retrieved successfully.");
    }
}
