using FluentValidation;
using FluentValidation.AspNetCore;
using IssueManager.API.Middlewares;
using IssueManager.Core.Interfaces;
using IssueManager.Core.Models.Validators;
using IssueManager.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<GitHubIssueService>();
builder.Services.AddHttpClient<GitLabIssueService>();
builder.Services.AddScoped<IIssueServiceFactory, IssueServiceFactory>();
builder.Services.AddHttpClient();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddIssueRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateIssueRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RepoDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
