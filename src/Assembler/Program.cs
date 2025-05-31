using Assembler.Services;
using Assembler.Services.Implementations;
using Cocona;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();
builder.Services
    .AddScoped<IParser, Parser>()
    .AddScoped<ITranslator, TextTranslator>()
    .AddScoped<ISymbolTable, SymbolTable>()
    .AddSingleton<IHackAssembler, HackAssembler>();

var app = builder.Build();
app.AddCommand(async ([Argument] string path, IHackAssembler hackAssembler) =>
{
   await hackAssembler.Assemble(path);
});
app.Run();