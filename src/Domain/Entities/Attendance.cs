using Backend.Domain.Entities;
using System;

namespace Backend.Domain.Entities
{
    public class Attendance : BaseAuditableEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool IsPresent { get; set; }
        public string? Remarks { get; set; }
    }
}
