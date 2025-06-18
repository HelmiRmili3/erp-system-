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
