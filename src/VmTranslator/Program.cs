using Core.Services.Common;
using Core.Services.VmTranslator;
using Cocona;
using Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;
using VmTranslator.Commands;
using VmTranslator.Services;
using VmTranslator.Services.Implementations;

var builder = CoconaApp.CreateBuilder();
builder.Services
    .AddScoped<IParser, VmTranslatorParser>()
    .AddScoped<ITranslator, VmTranslatorTranslator>()
    .AddScoped<IFileManager, FileManager>();

var app = builder.Build();
app.AddVmTranslatorCommands(args[0]);
app.Run();