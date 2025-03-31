using Auth.Api.BackgroundProcesses;
using Auth.Api.Configuration;
using Auth.Api.Middleware;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegistrationConfigureSections(builder.Configuration);
builder.AddIoC();
builder.Services.AddCorsCustom();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenConfig();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApiVersioningCustom();
builder.Services.AddHostedService<InitialWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerCustom();
}

app.UseCorsCustom();

app.UseForwardedHeaders(new()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseMiddleware<HandleExceptionsMiddleware>();

app.UseHsts();

app.UseIoC(builder);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
// rabbitMQ  RabbitMQ поддерживает несколько протоколов: AMQP 0-9-1 Ч открытый протокол
//  microservice
// grpc
//   Apache Kafka, RabbitMQ, Azure Service Bus
// video stream
//phone sms
//PAYMENT
// AI
//elasticsearch
//wpf