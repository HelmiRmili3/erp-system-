using System;
using System.Collections.Generic;
using System.Linq;
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

public record GetCertificationsByUserIdQuery(string UserId) : IRequest<Response<List<CertificationDto>>>;

public class GetCertificationsByUserIdQueryHandler : IRequestHandler<GetCertificationsByUserIdQuery, Response<List<CertificationDto>>>
{
    private readonly IQueryRepository<Certification> _repository;
    private readonly IUserQueryRepository _userRepository;

    public GetCertificationsByUserIdQueryHandler(
        IQueryRepository<Certification> repository,
        IUserQueryRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<Response<List<CertificationDto>>> Handle(GetCertificationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Certification, bool>> filter = c => c.UserId == request.UserId;

        var entities = await _repository.GetAllByFilterAsync(filter, includeTable: null, cancellationToken);

        var dtoList = new List<CertificationDto>();

        // Fetch user data once (all certifications share the same UserId)
        UserDataDto? userDto = null;
        if (!string.IsNullOrWhiteSpace(request.UserId))
        {
            userDto = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        }

        foreach (var cert in entities)
        {
            dtoList.Add(new CertificationDto
            {
                Id = cert.Id,
                UserId = cert.UserId,
                Name = cert.Name,
                Authority = cert.Authority,
                DateObtained = cert.DateObtained,
                FileUrl = cert.FileUrl ?? string.Empty,
                User = userDto
            });
        }

        return new Response<List<CertificationDto>>(dtoList, $"Certifications for user {request.UserId} retrieved successfully.");
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
