using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Files.Configurations.Dto;

namespace Backend.Application.Features.Files.Commands;

public record RemoveFile : RemoveFileDto, IRequest<Response<string>>;

public class RemoveFileHandler(IFileService fileService)
    : IRequestHandler<RemoveFile, Response<string>>
{
    private readonly IFileService _fileService = fileService;
    public async Task<Response<string>> Handle(RemoveFile request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Path, nameof(request.Path));

        await _fileService.DeleteFileAsync(request.Path);

        return new Response<string>("", "File removed successfully.");
    }
}


public class RemoveFileValidator : AbstractValidator<RemoveFile>
{
    public RemoveFileValidator()
    {
        RuleFor(p => p.Path).NotNull().WithMessage("{PropertyName} is required.");
    }
}
