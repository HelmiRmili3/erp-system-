using Backend.Application.Features.Configurations.Commands;
using Backend.Application.Features.Configurations.Queries;

namespace Backend.Web.Endpoints;

public class Configurations : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        //app.MapGroup(this)
        //    .MapGet(GetConfigurations)
        //    .MapGet(GetConfiguration, "{key}");

        //app.MapGroup(this)
        //    .RequireAuthorization()
        //    .MapPost(CreateConfiguration)
        //    .MapPut(UpdateConfiguration, "{key}")
        //    .MapDelete(DeleteConfiguration, "{key}");
    }

    //public async Task<IResult> GetConfigurations(ISender sender)
    //{
    //    return Results.Ok(await sender.Send(new GetConfigurationsQuery()));
    //}

    //public async Task<IResult> GetConfiguration(ISender sender, string key)
    //{
    //    return Results.Ok(await sender.Send(new GetConfigurationQuery(key)));
    //}

    //public async Task<IResult> CreateConfiguration(CreateConfigurationCommand command, ISender sender)
    //{
    //    var entity = await sender.Send(command);

    //    return Results.Created(nameof(Configurations), entity);
    //}

    //public async Task<IResult> UpdateConfiguration(ISender sender, string key, UpdateConfigurationCommand command)
    //{
    //    if (key != command.Key)
    //    {
    //        return Results.BadRequest("Id mismatch");
    //    }

    //    return Results.Ok(await sender.Send(command));
    //}

    //public async Task<IResult> DeleteConfiguration(ISender sender, string key)
    //{
    //    return Results.Ok(await sender.Send(new DeleteConfigurationCommand(key)));
    //}
}
