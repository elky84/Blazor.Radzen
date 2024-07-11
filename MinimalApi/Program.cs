using System.Text.Json.Serialization;
using AutoMapper;
using Dapper;
using Dapper.Json;
using Microsoft.AspNetCore.Diagnostics;
using MinimalApi.Components;
using MinimalApi.Domain.Account.Dao;
using MinimalApi.Domain.Account.Mapper;
using MinimalApi.Domain.Account.Repository;
using MinimalApi.Domain.Common;
using MinimalApi.Endpoint;
using MySqlConnector;
using Radzen;
using System.Net;

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

#region DapperMapper

SqlMapper.AddTypeHandler(new JsonTypeHandler<DetailDao>());

#endregion // DapperMapper

#region AutoMapper

var configuration = new MapperConfiguration(cfg =>
{
    AccountMapper.Set(cfg);
});

// only during development, validate your mappings; remove it before release
#if DEBUG
configuration.AssertConfigurationIsValid();
#endif
builder.Services.AddSingleton(configuration.CreateMapper());

#endregion //AutoMapper

builder.Services.AddScoped<MySqlConnection>((sp) => new MySqlConnection(builder.Configuration.GetConnectionString("DB")));
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
            } )));

#region api

var api = app.MapGroup("/api");

AccountEndpoint.Map(api);

#endregion api

app.MapGet("/hello", () => "Hello World!"); // 테스트 코드에서 사용 중. 샘플에서만 사용 예정

await app.RunAsync();

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program { } // for UnitTest