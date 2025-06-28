namespace Backend.Domain.Constants
{
    public abstract class Claims
    {
        public const string CanEdit = nameof(CanEdit);
        public const string CanView = nameof(CanView);
        public const string CanDelete = nameof(CanDelete);
        public const string CanCreate = nameof(CanCreate);
        public const string CanPurge = nameof(CanPurge);
        // Add more claims as needed
    }
}
//namespace Backend.Domain.Constants
//{
//    public static class Claims
//    {
//        // System-wide claims
//        public static class System
//        {
//            public const string FullAccess = nameof(FullAccess);
//            public const string ConfigureSystem = nameof(ConfigureSystem);
//            public const string ManageUsers = nameof(ManageUsers);
//            public const string AssignRoles = nameof(AssignRoles);
//            public const string AuditLogs = nameof(AuditLogs);
//        }

//        // CRUD operations (can be used across modules)
//        public static class Crud
//        {
//            public const string Create = nameof(Create);
//            public const string Read = nameof(Read);
//            public const string Update = nameof(Update);
//            public const string Delete = nameof(Delete);
//            public const string Export = nameof(Export);
//            public const string Import = nameof(Import);
//        }

//        // Employee Management Module
//        public static class Employee
//        {
//            public const string ManageAll = $"{nameof(Employee)}.{nameof(ManageAll)}";
//            public const string ViewPersonal = $"{nameof(Employee)}.{nameof(ViewPersonal)}";
//            public const string ManageDocuments = $"{nameof(Employee)}.{nameof(ManageDocuments)}";
//            public const string ViewContracts = $"{nameof(Employee)}.{nameof(ViewContracts)}";
//            public const string ViewSensitiveData = $"{nameof(Employee)}.{nameof(ViewSensitiveData)}";
//        }

//        // Attendance Module
//        public static class Attendance
//        {
//            public const string CheckInOut = $"{nameof(Attendance)}.{nameof(CheckInOut)}";
//            public const string CorrectEntries = $"{nameof(Attendance)}.{nameof(CorrectEntries)}";
//            public const string ViewTeamAttendance = $"{nameof(Attendance)}.{nameof(ViewTeamAttendance)}";
//            public const string GenerateReports = $"{nameof(Attendance)}.{nameof(GenerateReports)}";
//        }

//        // Absence Module
//        public static class Absence
//        {
//            public const string Request = $"{nameof(Absence)}.{nameof(Request)}";
//            public const string Approve = $"{nameof(Absence)}.{nameof(Approve)}";
//            public const string ViewAll = $"{nameof(Absence)}.{nameof(ViewAll)}";
//            public const string ViewTeam = $"{nameof(Absence)}.{nameof(ViewTeam)}";
//        }

//        // Payroll Module
//        public static class Payroll
//        {
//            public const string Generate = $"{nameof(Payroll)}.{nameof(Generate)}";
//            public const string Distribute = $"{nameof(Payroll)}.{nameof(Distribute)}";
//            public const string ViewOwn = $"{nameof(Payroll)}.{nameof(ViewOwn)}";
//            public const string ViewAll = $"{nameof(Payroll)}.{nameof(ViewAll)}";
//            public const string Correct = $"{nameof(Payroll)}.{nameof(Correct)}";
//        }

//        // Expense Module
//        public static class Expense
//        {
//            public const string Submit = $"{nameof(Expense)}.{nameof(Submit)}";
//            public const string Approve = $"{nameof(Expense)}.{nameof(Approve)}";
//            public const string Process = $"{nameof(Expense)}.{nameof(Process)}";
//            public const string Audit = $"{nameof(Expense)}.{nameof(Audit)}";
//            public const string UseAIProcessing = $"{nameof(Expense)}.{nameof(UseAIProcessing)}";
//        }

//        // Mobile-specific claims
//        public static class Mobile
//        {
//            public const string BiometricAuth = $"{nameof(Mobile)}.{nameof(BiometricAuth)}";
//            public const string OfflineAccess = $"{nameof(Mobile)}.{nameof(OfflineAccess)}";
//            public const string LocationTracking = $"{nameof(Mobile)}.{nameof(LocationTracking)}";
//        }
//    }
//}
