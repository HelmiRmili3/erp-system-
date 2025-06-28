
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Employees.Dto;
using Backend.Application.Features.Employees.IRepositories;


namespace Backend.Application.Features.Employees.Commands
{
    public record CreateEmployeeCommand : EmployeeAddDto, IRequest<Response<EmployeeDto>>;

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Response<EmployeeDto>>
    {
        private readonly IEmployeeCommandRepository _repository;

        public CreateEmployeeCommandHandler(IEmployeeCommandRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Validate required string properties, use Guard from your extensions (assuming Guard.Against.NullOrEmpty)
            Guard.Against.NullOrEmpty(request.FirstName, nameof(request.FirstName));
            Guard.Against.NullOrEmpty(request.LastName, nameof(request.LastName));
            Guard.Against.NullOrEmpty(request.JobTitle, nameof(request.JobTitle));
            Guard.Against.NullOrEmpty(request.Department, nameof(request.Department));
            Guard.Against.Null(request.HireDate, nameof(request.HireDate));
            Guard.Against.NullOrEmpty(request.Status, nameof(request.Status));
            Guard.Against.NullOrEmpty(request.ContractType, nameof(request.ContractType));

            var entity = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                JobTitle = request.JobTitle,
                Department = request.Department,
                HireDate = request.HireDate,
                ContractType = request.ContractType,
            };

            var result = await _repository.AddAsync(entity, cancellationToken);

            if (result == null)
            {
                throw new ApplicationException("Employee could not be created");
            }

            return new Response<EmployeeDto>(result.ToDto<EmployeeDto>(), "Employee created successfully");
        }
    }

    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        private readonly IEmployeeQueryRepository _repository;

        public CreateEmployeeCommandValidator(IEmployeeQueryRepository repository)
        {
            _repository = repository;

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

            RuleFor(v => v.JobTitle)
                .NotEmpty().WithMessage("Job title is required.")
                .MaximumLength(100).WithMessage("Job title must not exceed 100 characters.");

            RuleFor(v => v.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(100).WithMessage("Department must not exceed 100 characters.");

            RuleFor(v => v.HireDate)
                .NotEmpty().WithMessage("Hire date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Hire date must be in the past.");

            RuleFor(v => v.ContractType)
                .NotEmpty().WithMessage("Contract type is required.")
                .MaximumLength(50).WithMessage("Contract type must not exceed 50 characters.");

            RuleFor(v => v.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(50).WithMessage("Status must not exceed 50 characters.");
        }
    }
}
