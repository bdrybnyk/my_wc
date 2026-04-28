using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;
using System.ComponentModel;
using my_wc;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

await app.RunAsync();

[McpServerToolType]
public static class WcTools
{
    [McpServerTool, Description("Рахує слова, рядки та байти у тексті")]
    public static string Count(string text)
    {
        return App.FormatOutput(
            text ?? "",
            true,
            true,
            true,
            "ai_input"
        );
    }
}