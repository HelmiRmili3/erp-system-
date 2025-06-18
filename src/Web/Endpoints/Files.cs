using Microsoft.AspNetCore.Mvc;
using Backend.Application.Features.Files.Commands;

namespace Backend.Web.Endpoints;

public class Files : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost<UploadFile>(UploadFile, "", "multipart/form-data")
            .MapDelete(RemoveFile, "{path}");
    }

    public async Task<IResult> UploadFile([FromForm] UploadFile command, ISender sender)
    {
        var entity = await sender.Send(command);

        return Results.Created(nameof(Files), entity);
    }

    public async Task<IResult> RemoveFile(ISender sender, string path)
    {
        return Results.Ok(await sender.Send(new RemoveFile { Path = path }));
    }
}
