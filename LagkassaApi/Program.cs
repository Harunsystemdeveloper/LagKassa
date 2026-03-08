using LagkassaApi.DTOs;
using LagkassaApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<LagkassaService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Lagkassa API körs!");

app.MapGet("/players", (LagkassaService service) =>
{
    return Results.Ok(service.GetAllPlayers());
});

app.MapGet("/players/{id}", (int id, LagkassaService service) =>
{
    var player = service.GetPlayerById(id);

    if (player == null)
        return Results.NotFound("Spelaren hittades inte");

    return Results.Ok(player);
});

app.MapPost("/players/{id}/deposit", (int id, DepositDto dto, LagkassaService service) =>
{
    var success = service.Deposit(id, dto.Amount);

    if (!success)
        return Results.BadRequest("Fel spelare eller fel belopp");

    return Results.Ok("Pengar insatta");
});

app.MapGet("/activities", (LagkassaService service) =>
{
    return Results.Ok(service.GetAllActivities());
});

app.MapPost("/activities", (CreateActivityDto dto, LagkassaService service) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest("Aktiviteten måste ha ett namn");

    if (dto.TotalCost <= 0)
        return Results.BadRequest("Kostnaden måste vara större än 0");

    var result = service.CreateActivity(dto);

    if (result == null)
        return Results.BadRequest("Ogiltiga spelare eller tom lista");

    return Results.Ok(result);
});

app.Run();
