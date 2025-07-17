namespace Backend.Domain.Constants;

public static class Permissions
{
    public static class Roles
    {
        public const string View = "Roles.View";
        public const string Create = "Roles.Create";
        public const string Edit = "Roles.Edit";
        public const string Delete = "Roles.Delete";
    }

    public static class Settings
    {
        public const string View = "Settings.View";
        public const string Edit = "Settings.Edit";
    }

    public static class System
    {
        public const string CanPurge = nameof(CanPurge);
    }

    public static class Absences
    {
        public const string View = "Absences.View";
        public const string Create = "Absences.Create";
        public const string Edit = "Absences.Edit";
        public const string Delete = "Absences.Delete";
    }

    public static class Attendances
    {
        public const string View = "Attendances.View";
        public const string Create = "Attendances.Create";
        public const string Edit = "Attendances.Edit";
        public const string Delete = "Attendances.Delete";
    }

    public static class Certifications
    {
        public const string View = "Certifications.View";
        public const string Create = "Certifications.Create";
        public const string Edit = "Certifications.Edit";
        public const string Delete = "Certifications.Delete";
    }

    public static class Contracts
    {
        public const string View = "Contracts.View";
        public const string Create = "Contracts.Create";
        public const string Edit = "Contracts.Edit";
        public const string Delete = "Contracts.Delete";
    }

    public static class Expanses
    {
        public const string View = "Expenses.View";
        public const string Create = "Expenses.Create";
        public const string Edit = "Expenses.Edit";
        public const string Delete = "Expenses.Delete";
    }

    public static class Payrolls
    {
        public const string View = "Payrolls.View";
        public const string Create = "Payrolls.Create";
        public const string Edit = "Payrolls.Edit";
        public const string Delete = "Payrolls.Delete";
    }

    // Optional: A flat list of all permissions
    public static readonly string[] All =
    [
        // Roles
        Roles.View, Roles.Create, Roles.Edit, Roles.Delete,

        // Settings
        Settings.View, Settings.Edit,

        // System
        System.CanPurge,

        // Absences
        Absences.View, Absences.Create, Absences.Edit, Absences.Delete,

        // Attendances
        Attendances.View, Attendances.Create, Attendances.Edit, Attendances.Delete,

        // Certifications
        Certifications.View, Certifications.Create, Certifications.Edit, Certifications.Delete,

        // Contracts
        Contracts.View, Contracts.Create, Contracts.Edit, Contracts.Delete,

        // Expanses
        Expanses.View, Expanses.Create, Expanses.Edit, Expanses.Delete,

        // Payrolls
        Payrolls.View, Payrolls.Create, Payrolls.Edit, Payrolls.Delete
    ];
}
