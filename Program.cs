using System.Diagnostics;
using FilterPresetConverter.Tests;

namespace FilterPresetConverter;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        // 如果带有--test参数，运行测试
        if (args.Length > 0 && args[0] == "--test")
        {
            RunTests();
            return;
        }

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }

    static void RunTests()
    {
        Console.WriteLine("开始转换测试...\n");

        try
        {
            var test = new ConversionTest();
            test.RunAllTests();

            Console.WriteLine("\n测试完成!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"测试出错: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}