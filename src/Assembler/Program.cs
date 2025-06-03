using Application.Services.Assembler;
using Application.Services.Assembler.Implementations;
using Application.Services.Common;
using Cocona;
using Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();
builder.Services
    .AddScoped<IParser, AssemblyParser>()
    .AddScoped<ITranslator, AssemblyTranslatorBinary>()
    .AddScoped<ISymbolTable, SymbolTable>()
    .AddScoped<IFileManager, FileManager>()
    .AddScoped<IAssembler, HackAssembler>();

var app = builder.Build();
app.AddCommand(async ([Argument] string path, IAssembler assembler) =>
{
   await assembler.Assemble(path);
});
app.Run();