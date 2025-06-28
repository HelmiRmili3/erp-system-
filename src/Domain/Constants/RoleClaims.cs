//namespace Backend.Domain.Constants
//{
//    public static class RoleClaims
//    {
//        public static readonly IReadOnlyDictionary<string, string[]> Mappings =
//            new Dictionary<string, string[]>
//            {
//                // System roles
//                [Roles.SuperAdmin] = new[]
//                {
//                    Claims.System.FullAccess,
//                    Claims.System.ConfigureSystem,
//                    Claims.System.ManageUsers,
//                    Claims.System.AssignRoles
//                },

//                // HR roles
//                [Roles.HR.Admin] = new[]
//                {
//                    Claims.Employee.ManageAll,
//                    Claims.Employee.ManageDocuments,
//                    Claims.Absence.Approve,
//                    Claims.Crud.Export
//                },

//                [Roles.HR.Specialist] = new[]
//                {
//                    Claims.Employee.Update,
//                    Claims.Employee.ManageDocuments,
//                    Claims.Crud.Read
//                },

//                // Finance roles
//                [Roles.Finance.PayrollManager] = new[]
//                {
//                    Claims.Payroll.Generate,
//                    Claims.Payroll.Distribute,
//                    Claims.Payroll.Correct,
//                    Claims.Crud.Export
//                },

//                // Employee role
//                [Roles.Employee] = new[]
//                {
//                    Claims.Employee.ViewPersonal,
//                    Claims.Absence.Request,
//                    Claims.Expense.Submit,
//                    Claims.Payroll.ViewOwn
//                },

//                // Approval manager (cross-department)
//                [Roles.ApprovalManager] = new[]
//                {
//                    Claims.Absence.Approve,
//                    Claims.Expense.Approve
//                }
//            };

//        public static string[] GetClaimsForRole(string role)
//        {
//            return Mappings.TryGetValue(role, out var claims)
//                ? claims
//                : Array.Empty<string>();
//        }
//    }
//}
