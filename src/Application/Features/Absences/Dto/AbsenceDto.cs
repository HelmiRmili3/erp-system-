using Backend.Domain.Enums;
namespace Backend.Application.Features.Absences.Dto
{
    public class AbsenceDto : BaseAbsenceDto
    {
        // You can extend here with additional relations if needed
    }

    public class BaseAbsenceDto
    {
        public int Id { get; init; }
        public string UserId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AbsenceType AbsenceType { get; set; }
        public AbsenceStatus StatusType { get; set; }
        public string? Reason { get; set; }
    }

    public record AbsenceAddDto
    {
        public required string UserId { get; set; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public AbsenceType AbsenceType { get; init; }
        public AbsenceStatus StatusType { get; init; }
        public string? Reason { get; init; }
    }

    public record AbsenceUpdateDto : AbsenceAddDto
    {
        public int Id { get; init; }
    }
}
