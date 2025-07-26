using Backend.Domain.Enums;
namespace Backend.Application.Features.Absences.Dto
{
    public class AbsenceDto : BaseAbsenceDto
    {
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
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public AbsenceType AbsenceType { get; init; }
        public AbsenceStatus StatusType = AbsenceStatus.Pending;
        public string? Reason { get; init; }
    }

    public record AbsenceUpdateDto 
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public AbsenceType AbsenceType { get; init; }
        public string? Reason { get; init; }
        public int Id { get; init; }
    }
}
