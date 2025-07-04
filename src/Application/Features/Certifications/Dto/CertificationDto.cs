namespace Backend.Application.Features.Certifications.Dto
{
    public class CertificationDto : BaseCertificationDto
    {
        // Extend with navigation properties if needed (e.g., User details)
    }

    public class BaseCertificationDto
    {
        public int Id { get; init; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Authority { get; set; } = string.Empty;
        public DateTime DateObtained { get; set; }
        public string FileUrl { get; set; } = string.Empty;
    }

    public record CreateCertificationDto
    {
        public required string UserId { get; init; }
        public required string Name { get; init; }
        public required string Authority { get; init; }
        public required DateTime DateObtained { get; init; }
        public required string FileUrl { get; init; }
    }

    public record UpdateCertificationDto : CreateCertificationDto
    {
        public int Id { get; init; }
    }
}
