using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Ong.Api.Campaigns.Extensions;
using Ong.Application;
using Ong.Application.Requests;
using Ong.Domain.Queries;
using Ong.Infra;
using OpenTelemetry.Metrics;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfraLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddRuntimeInstrumentation();
        metrics.AddPrometheusExporter();
    });


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ong Campaigns API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token JWT"
    });

    c.AddSecurityRequirement((OpenApiDocument doc) =>
    {
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer", doc),
                []
            }
        };
    });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("GestorONGPolicy", policy => policy.RequireRole("GestorONG"))
    .AddPolicy("DoadorPolicy", policy => policy.RequireRole("Doador"));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");

app.MapPost("/campaigns", async ([FromBody] CreateCampaignRequest request, IMediator mediator) =>
{
    var result = await mediator.Send(request);
    return result.HasErrors ? Results.BadRequest(result) : Results.Ok(result);
}).RequireAuthorization("GestorONGPolicy").WithTags("Campaigns");

app.MapPut("/campaigns/{id}", async (Guid id, [FromBody] UpdateCampaignRequest request, IMediator mediator) =>
{
    request.Id = id;
    var result = await mediator.Send(request);
    return result.HasErrors ? Results.BadRequest(result) : Results.Ok(result);
}).RequireAuthorization("GestorONGPolicy").WithTags("Campaigns");

app.MapGet("/campaigns/active", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetActiveCampaignsRequest());
    return result.HasErrors ? Results.BadRequest(result) : Results.Ok(result);
}).WithTags("Campaigns");

app.MapPost("/donations/{campaignId}", async (Guid campaignId, [FromBody] DonationRequest request, IMediator mediator, HttpContext httpContext) =>
{
    request.CampaignId = campaignId;
    var userIdString = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? httpContext.User.FindFirst("sub")?.Value;
    if (!Guid.TryParse(userIdString, out var userId)) return Results.Unauthorized();
    request.UserId = userId;

    var result = await mediator.Send(request);
    return result.HasErrors ? Results.BadRequest(result) : Results.Ok(result);
}).RequireAuthorization("DoadorPolicy").WithTags("Donations");

app.MapPatch("/campaigns/{id}/donation-received", async (Guid id, [FromBody] DonationReceivedRequest request, IMediator mediator) =>
{
    request.CampaignId = id;
    var result = await mediator.Send(request);
    return result.HasErrors ? Results.BadRequest(result) : Results.Ok(result);
}).WithTags("Campaigns");

app.MapGet("/campaigns/outbox", async ([FromServices] IOutboxMessageQuery query) =>
{
    var messages = await query.ObterOutboxMessagesPendentesAsync();
    return messages;
}).RequireApiKey();

app.MapPatch("/campaigns/outbox/{id}/processed", async (Guid id, IMediator mediator) =>
{
    var result = await mediator.Send(new UpdateOutboxRequest() { Id = id });
    return result.HasErrors ? Results.BadRequest(result) : Results.Ok(result);
}).RequireApiKey();

app.MapPatch("/campaigns/outbox/{id}/error", async (Guid id, [FromBody] UpdateOutboxRequest request, IMediator mediator) =>
{
    request.Id = id;
    var result = await mediator.Send(request);
    return result.HasErrors ? Results.BadRequest(result) : Results.Ok(result);
}).RequireApiKey();

app.MapPost("/campaigns/users", async ([FromBody] CreateUserRequest request, IMediator mediator) =>
{
    var result = await mediator.Send(request);
    return result.HasErrors ? Results.BadRequest(result) : Results.Ok(result);
}).RequireApiKey();

app.Run();
