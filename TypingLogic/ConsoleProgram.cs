using System;
using Microsoft.Extensions.DependencyInjection;
using TypingTrainer.Logic;
namespace TypingTrainer.Logic.ConsoleApp;
class ConsoleProgram
{
    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITextProvider, SimpleTextProvider>();
        services.AddSingleton<IStatisticsInfo, SimpleStatisticsInfo>();
        services.AddSingleton<ICorrectChecker, SimpleCorrectChecker>();
        services.AddSingleton<IMistakeProcessor, AdvancedMistakeProcessor>();
        services.AddSingleton<ITyping, Typing>();
        services.AddSingleton<ITimerProvider, SimpleTimer>();
        return services.BuildServiceProvider();
    }
    
    static void Main(string[] args)
    {
     

   
     

        var serviceProvider = ConfigureServices();
        var typing = serviceProvider.GetService<ITyping>();
       
        bool result = true;
        bool lastResult = true;
      
    

        Console.WriteLine("Start typing the following text:");
        Console.WriteLine(typing.Text);
        string typedText = "";
        while (true)
        {

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
      
            char c = keyInfo.KeyChar;
             result = typing.InputChar(c);
            if (typing.Cursor >= typing.Text.Length)
            {
                Console.WriteLine("Typing completed!");
                break;
            }
            if (result)
            {
                Console.Write(keyInfo.KeyChar);
                typedText += c;
            }
            else if (lastResult == true)
            {
                Console.Beep();
                Console.WriteLine("\n - Mistake! \n");
                typedText = typedText.Substring(0, typing.Cursor);
                Console.Write(typedText);


            }
            else if (lastResult==false)
            {
                PrintStats( typing.StatisticsProvider);
            }
            lastResult = result;
        }
    }
    static void PrintStats(IStatisticsInfo stats)
    {
        Console.WriteLine("\n===== Статистика набора текста =====");
        Console.WriteLine($"Всего символов:     {stats.TotalChars}");
        Console.WriteLine($"Верных символов:    {stats.CorrectChars}");
        Console.WriteLine($"Ошибочных символов: {stats.IncorrectChars}");
        Console.WriteLine($"Точность:           {stats.Accuracy * 100:F2}%");
        Console.WriteLine($"Симв/мин:           {stats.CharacterPerMinute:F0}");
        Console.WriteLine($"Время:              {stats.Time:mm\\:ss\\.fff}");
        Console.WriteLine("====================================");
    }

}
