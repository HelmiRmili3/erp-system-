using Microsoft.AspNetCore.Mvc;
using Backend.Application.Features.Categories.Commands;
using Backend.Application.Features.Categories.Queries;

namespace Backend.Web.Endpoints;

public class Categories : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetAllCategories)
            .MapGet(GetCategory, "{id}");

        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost<CreateCategoryCommand>(CreateCategory, "", "multipart/form-data")
            .MapPost(UpdateCategory, "{id}")
            .MapDelete(DeleteCategory, "{id}");
    }

    public async Task<IResult> GetAllCategories(ISender sender)
    {
        return Results.Ok(await sender.Send(new GetCategoriesQuery()));
    }

    public async Task<IResult> GetCategory(ISender sender, int id)
    {
        return Results.Ok(await sender.Send(new GetCategoryQuery(id)));
    }

    [Consumes("multipart/form-data")]
    public async Task<IResult> CreateCategory([FromForm] CreateCategoryCommand command, ISender sender)
    {
        var response = await sender.Send(command);

        return Results.Created(nameof(Categories), response);
    }

    [Consumes("multipart/form-data")]
    public async Task<IResult> UpdateCategory(ISender sender, int id, [FromForm] UpdateCategoryCommand command)
    {
        if (id != command.Id)
        {
            return Results.BadRequest("Id mismatch");
        }

        return Results.Ok(await sender.Send(command));
    }

    public async Task<IResult> DeleteCategory(ISender sender, int id)
    {
        return Results.Ok(await sender.Send(new DeleteCategoryCommand(id)));
    }

}
