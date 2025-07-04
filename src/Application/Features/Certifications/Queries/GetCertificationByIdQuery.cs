using System;
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

public record GetCertificationByIdQuery(int Id) : IRequest<Response<CertificationDto>>;

public class GetCertificationByIdQueryHandler : IRequestHandler<GetCertificationByIdQuery, Response<CertificationDto>>
{
    private readonly IQueryRepository<Certification> _repository;

    public GetCertificationByIdQueryHandler(IQueryRepository<Certification> repository)
    {
        _repository = repository;
    }

    public async Task<Response<CertificationDto>> Handle(GetCertificationByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Certification, bool>> filter = c => c.Id == request.Id;

        var entity = await _repository.GetSingleByFilterAsync(filter, includeTable: null, cancellationToken);

        if (entity == null)
            return new Response<CertificationDto>($"Certification with Id {request.Id} not found.");

        return new Response<CertificationDto>(entity.ToDto<CertificationDto>(), "Certification retrieved successfully.");
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

