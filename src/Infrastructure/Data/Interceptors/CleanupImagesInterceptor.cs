using Backend.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Backend.Domain.Entities;

namespace Backend.Infrastructure.Data.Interceptors;

public class CleanupImagesInterceptor(IFileService fileService) : SaveChangesInterceptor
{
    private readonly IFileService _fileService = fileService;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context != null)
        {
            HandleDeleteLogic(context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleDeleteLogic(DbContext context)
    {
        var deletedEntities = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in deletedEntities)
        {
            if (entry.Entity is Category category)
            {
                if (category.Logo != null) _fileService.DeleteFileAsync(category.Logo);
            }

        }
    }
}
