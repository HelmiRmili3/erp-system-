using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Backend.Application.Common.Response;
using Backend.Application.Features.Certifications.Dto;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Extensions;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Certifications.Queries;

public record GetCertificationsByUserIdQuery(string UserId) : IRequest<Response<List<CertificationDto>>>;

public class GetCertificationsByUserIdQueryHandler : IRequestHandler<GetCertificationsByUserIdQuery, Response<List<CertificationDto>>>
{
    private readonly IQueryRepository<Certification> _repository;

    public GetCertificationsByUserIdQueryHandler(IQueryRepository<Certification> repository)
    {
        _repository = repository;
    }

    public async Task<Response<List<CertificationDto>>> Handle(GetCertificationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Certification, bool>> filter = c => c.UserId == request.UserId;

        var entities = await _repository.GetAllByFilterAsync(filter, includeTable: null, cancellationToken);

        var dtos = entities.Select(c => c.ToDto<CertificationDto>()).ToList();

        return new Response<List<CertificationDto>>(dtos, $"Certifications for user {request.UserId} retrieved successfully.");
    }
}

public class GetCertificationsByUserIdQueryValidator : AbstractValidator<GetCertificationsByUserIdQuery>
{
    public GetCertificationsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
    }
}
