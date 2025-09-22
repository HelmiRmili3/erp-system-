using Backend.Application.Common.Response;
using Backend.Application.Features.Contracts.Dtos;
using Backend.Domain.Entities;
using Backend.Application.Common.Interfaces;
using Backend.Application.Features.User.IRepositories;
using Backend.Application.Features.User.Dto;
using MediatR;
using Backend.Application.Common.Extensions;

namespace Backend.Application.Features.Contracts.Queries
{
    public record GetContractByIdQuery(int Id) : IRequest<Response<ContractDto>>;

    public class GetContractByIdQueryHandler
        : IRequestHandler<GetContractByIdQuery, Response<ContractDto>>
    {
        private readonly IQueryRepository<Contract> _repository;
        private readonly IUserQueryRepository _userRepository;

        public GetContractByIdQueryHandler(
            IQueryRepository<Contract> repository,
            IUserQueryRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<Response<ContractDto>> Handle(
            GetContractByIdQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                return new Response<ContractDto>($"Contract with ID {request.Id} not found.")
                    .WithError("Not Found", "404");
            }

            // Map to DTO
            var dto = entity.ToDto<ContractDto>();

            // Fetch user data
            if (!string.IsNullOrWhiteSpace(entity.UserId))
            {
                var userDto = await _userRepository.GetByIdAsync(entity.UserId, cancellationToken);
                dto.User = userDto;
            }

            return new Response<ContractDto>(dto, "Contract retrieved successfully.");
        }
    }
}
