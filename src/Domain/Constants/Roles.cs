namespace Backend.Domain.Constants;

public abstract class Roles
{
    public const string Administrator = nameof(Administrator);
    public const string User = nameof(User);
    public const string Employee = nameof(Employee);

}
//namespace Backend.Domain.Constants
//{
//    public static class Roles
//    {
//        // System-wide roles
//        public const string SuperAdmin = nameof(SuperAdmin);
//        public const string SystemAdmin = nameof(SystemAdmin);
//        public const string Auditor = nameof(Auditor);

//        // Departmental roles
//        public static class HR
//        {
//            public const string Admin = $"{nameof(HR)}.{nameof(Admin)}";
//            public const string Manager = $"{nameof(HR)}.{nameof(Manager)}";
//            public const string Specialist = $"{nameof(HR)}.{nameof(Specialist)}";
//            public const string Recruiter = $"{nameof(HR)}.{nameof(Recruiter)}";
//        }

//        public static class Finance
//        {
//            public const string Admin = $"{nameof(Finance)}.{nameof(Admin)}";
//            public const string PayrollManager = $"{nameof(Finance)}.{nameof(PayrollManager)}";
//            public const string Accountant = $"{nameof(Finance)}.{nameof(Accountant)}";
//        }

//        // Functional roles
//        public static class Operations
//        {
//            public const string Director = $"{nameof(Operations)}.{nameof(Director)}";
//            public const string Manager = $"{nameof(Operations)}.{nameof(Manager)}";
//            public const string Supervisor = $"{nameof(Operations)}.{nameof(Supervisor)}";
//        }

//        // Employee self-service roles
//        public const string Employee = nameof(Employee);
//        public const string Contractor = nameof(Contractor);

//        // Mobile-specific roles
//        public const string MobileUser = nameof(MobileUser);
//        public const string FieldEmployee = nameof(FieldEmployee);

//        // Special composite roles
//        public const string ApprovalManager = nameof(ApprovalManager);
//        public const string ReportViewer = nameof(ReportViewer);
//    }
//}
