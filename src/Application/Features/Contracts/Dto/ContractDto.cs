using Backend.Domain.Enums;

namespace Backend.Application.Features.Contracts.Dtos
{
    public class ContractDto : BaseContractDto
    {
        public int Id { get; init; }
    }

    public class BaseContractDto
    {
        public string UserId { get; set; } = string.Empty;
        public ContractType ContractType { get; set; } = ContractType.Permanent;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? FileUrl { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    }

    public record ContractAddDto
    {
        public required string UserId { get; init; }
        public ContractType ContractType { get; init; } = ContractType.Permanent;
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public EmployeeStatus Status { get; init; } = EmployeeStatus.Active;
    }

    public record ContractUpdateDto : ContractAddDto
    {
        public int Id { get; init; }
        public string? FileUrl { get; init; }

    }
}
