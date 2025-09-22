using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Backend.Application.Common.Response;
using Backend.Application.Features.Certifications.Dto;
using Backend.Application.Common.Interfaces;
using Backend.Application.Features.User.IRepositories;
using Backend.Application.Features.User.Dto;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Certifications.Queries;

public record GetCertificationByIdQuery(int Id) : IRequest<Response<CertificationDto>>;

public class GetCertificationByIdQueryHandler : IRequestHandler<GetCertificationByIdQuery, Response<CertificationDto>>
{
    private readonly IQueryRepository<Certification> _repository;
    private readonly IUserQueryRepository _userRepository;

    public GetCertificationByIdQueryHandler(
        IQueryRepository<Certification> repository,
        IUserQueryRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<Response<CertificationDto>> Handle(GetCertificationByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Certification, bool>> filter = c => c.Id == request.Id;

        var entity = await _repository.GetSingleByFilterAsync(filter, includeTable: null, cancellationToken);

        if (entity == null)
            return new Response<CertificationDto>($"Certification with Id {request.Id} not found.");

        UserDataDto? userDto = null;

        if (!string.IsNullOrWhiteSpace(entity.UserId))
        {
            userDto = await _userRepository.GetByIdAsync(entity.UserId, cancellationToken);
        }

        var dto = new CertificationDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            Authority = entity.Authority,
            DateObtained = entity.DateObtained,
            FileUrl = entity.FileUrl ?? string.Empty,
            User = userDto
        };

        return new Response<CertificationDto>(dto, "Certification retrieved successfully.");
    }
}

public class GetCertificationByIdQueryValidator : AbstractValidator<GetCertificationByIdQuery>
{
    public GetCertificationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");
    }
}
