using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using MinimalApi.Common.Dapper;
using MinimalApi.Common.Database;
using MinimalApi.Components;
using MinimalApi.Domain.Account.Repository;
using MinimalApi.Domain.Common;
using MinimalApi.Endpoint;
using Radzen;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;

#region builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents(options => options.DetailedErrors = true)
    .AddInteractiveServerComponents(options => options.DetailedErrors = true)
    .AddCircuitOptions(options => options.DetailedErrors = true)
    .AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRadzenComponents()
    .AddHttpContextAccessor();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<ChangedBags>();

#region DapperMapper

SqlMapper.AddTypeHandler(new DetailTypeHandler());

#endregion // DapperMapper

#region AutoMapper

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

#endregion //AutoMapper

builder.Services.AddScoped<DbContext>();
builder.Services.AddScoped<AccountRepository>();

#endregion // builder

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        => await context.Response.WriteAsJsonAsync(
            new ResponseHeader
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = context.Features.Get<IExceptionHandlerPathFeature>()?.Error.Message
            })));

#region api

var api = app.MapGroup("/api");

AccountEndpoint.Map(api);

#endregion api

app.MapGet("/hello", () => "Hello World!"); // 테스트 코드에서 사용 중. 샘플에서만 사용 예정

await app.RunAsync();

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
} // for UnitTest