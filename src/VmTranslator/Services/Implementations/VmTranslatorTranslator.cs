using System.Text;
using Core.Exceptions;

namespace VmTranslator.Services.Implementations;

public sealed class VmTranslatorTranslator : ITranslator
{
    private static int _jumpCount;
    private const string GetBothStackTop = """
                                      //adds top stack number to D and goes to the adress of the second
                                      @SP
                                      M=M-1
                                      A=M
                                      D=M
                                      @SP
                                      M=M-1
                                      A=M    
                                      """;

    private const string EndOperation = """
                                        // incremets sp
                                        @SP
                                        M=M+1
                                        """;

    public byte[] Translate(char[][] target, string fileName)
    {
        var instruction = new string(target[0]) switch
        {
            "add" => TranslateArithmetic('+'),
            "sub" => TranslateArithmetic('-'),
            "neg" => TranslateNegative(),
            "eq" => TranslateComparison("=="),
            "gt" => TranslateComparison(">"),
            "lt" => TranslateComparison("<"),
            "and" => TranslateBitwiseComparison('&'),
            "or" => TranslateBitwiseComparison('|'),
            "not" => TranslateNot(),
            "pop" => TranslatePop(GetToAddress(
                new string(target[1]),
                new string(target[2]),
                fileName)),
            "push" => TranslatePush(GetToAddress(
                new string(target[1]),
                new string(target[2]), 
                fileName)),
            _ => throw new TranslationException($"Invalid instruction: {target[0]}")
        };

        return Encoding.UTF8.GetBytes(instruction);
    }


    private static string TranslateArithmetic(char symbol)
    {
        if (symbol != '+' && symbol != '-')
            throw new TranslationException($"Invalid symbol for arithmetic: {symbol}");
        
        var operation = symbol == '+' ? "adds" : "subtracts";
        var assembly = symbol == '+' ? "D+M" : "M-D";
        return $"""
               {GetBothStackTop}
               // {operation} sp[n-2] and sp[n-1]
               M={assembly}
               {EndOperation}
               
               """;
    }

    private static string TranslateNegative()
    {
        return """
               // translate neg
               @SP
               M=M-1
               A=M
               M=-M
               @SP
               M=M+1
               
               """;
    }
    
    private static string TranslateComparison(string symbol)
    {
        string operation = symbol switch
        {
            "==" => "JEQ",
            ">" => "JGT",
            "<" => "JLT",
            _ => throw new TranslationException($"Invalid symbol for comparison: {symbol}")
        };

        return $"""
               {GetBothStackTop}
               //sp[n-2] {symbol} sp[n-1] -1 true 0 false
               D=M-D
               @JUMP_SUC_{_jumpCount}
               D;{operation}
               @SP
               A=M
               M=0
               @JUMP_END_{_jumpCount}
               0;JMP
               (JUMP_SUC_{_jumpCount})
               @SP
               A=M
               M=-1
               (JUMP_END_{_jumpCount++})
               {EndOperation}
               
               """;
    }

    private static string TranslateBitwiseComparison(char symbol)
    {
        if (symbol != '|' && symbol != '&')
            throw new TranslationException($"Invalid symbol for bitwise comparison: {symbol}");
        return $"""
               {GetBothStackTop}
               // performs {symbol} between sp[n-2] and sp[n-1]
               M=D{symbol}M
               {EndOperation}
               
               """;
    }

    private static string TranslateNot()
    {
        return """
               // nots sp[n-1]
               @SP
               M=M-1
               A=M
               M=!M
               @SP
               M=M+1 
               
               """;
    }
    
    /// <summary>
    /// Generates assembly for getting into the segment address
    /// </summary>
    /// <param name="segment">Name of the register</param>
    /// <param name="offset">Address offset</param>
    /// <returns>The assembly equivalent to the segment address</returns>
    private static string GetToAddress(string segment, string offset, string fileName)
    {
        return new string(segment) switch
        {
            "local" => GetDefaultSegment("LCL"),
            "argument" => GetDefaultSegment("ARG"),
            "this" => GetDefaultSegment("THIS"),
            "that" => GetDefaultSegment("THAT"),
            "temp" => $"@{5+int.Parse(offset)}",
            "constant" => $"c@{offset}",
            "static" => $"@{fileName}.{offset}",
            "pointer" => offset == "0" 
                ? "@THIS"
                : "@THAT",
            _ => throw new TranslationException($"Invalid segment: {segment}")
        };

        string GetDefaultSegment(string alias)
        {
            return $"""
                   // adds default segment
                   @{offset}
                   D=A
                   @{alias}
                   A=D+M
                   
                   """;
        }
    }

    private static string TranslatePush(string address)
    {
        var value =  "M";
        if (address.StartsWith('c'))
        {
            address = address[1..];
            value = "A";
        }
        
        return $"""
               {address}
               // pushes to sp
               D={value}
               @SP
               A=M
               M=D
               @SP
               M=M+1
               
               """;
    }
    
    private static string TranslatePop(string address)
    {
        return $"""
               {address}
               // pops the top of the stack and stores at equivalent memory register
               D=A
               @R13
               M=D
               @SP
               M=M-1
               A=M
               D=M
               @R13
               A=M
               M=D
               
               """;
    }
}