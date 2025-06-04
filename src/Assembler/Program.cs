using Core.Services.Assembler.Implementations;
using Core.Services.Common;
using Assembler.Commands;
using Assembler.Services;
using Assembler.Services.Implementations;
using Cocona;
using Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();
builder.Services
    .AddScoped<IParser, AssemblyParser>()
    .AddScoped<ITranslator, AssemblyTranslatorBinary>()
    .AddScoped<ISymbolTable, SymbolTable>()
    .AddScoped<IFileService>(_ => new FileService(args[0]));

var app = builder.Build();
app.AddAssemblerCommands();
app.Run();