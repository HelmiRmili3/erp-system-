using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Files.Configurations.Dto;

namespace Backend.Application.Features.Files.Commands;

public record UploadFile : UploadFileDto, IRequest<Response<string>>;

public class UploadFileHandler(IFileService fileService)
    : IRequestHandler<UploadFile, Response<string>>
{
    private readonly IFileService _fileService = fileService;
    public async Task<Response<string>> Handle(UploadFile request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.File, nameof(request.File));

        var result = await _fileService.SaveFileAsync(request.File, Guid.NewGuid().ToString(), "files");

        return new Response<string>(result, "File uploaded successfully.");
    }
}


public class UploadFileValidator : AbstractValidator<UploadFile>
{
    public UploadFileValidator()
    {
        RuleFor(p => p.File).NotNull().WithMessage("{PropertyName} is required.");
    }
}
