using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingTrainer.Logic;

public interface ITypingFactory
{
    ITyping CreateTyping(Config config);
}
public class TypingFactory : ITypingFactory
{
    IServiceProvider ServiceProvider { get; set; }
    public TypingFactory(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    public ITyping CreateTyping(Config config)
    {
        
        ITextProvider textProvider = config.Type  switch
        {
            "usual" => ServiceProvider.GetRequiredService<TextFromTXTProvider>(),
            _ => throw new NotImplementedException(),
        };
        IMistakeProcessor mistakeProcessor = config.Difficulty   switch
        {
            "easy" => ServiceProvider.GetRequiredService<SimpleMistakeProcessor>(),
            "hard" => ServiceProvider.GetRequiredService<AdvancedMistakeProcessor>(),
            _ => throw new NotImplementedException(),
        };

        ITyping typing = new Typing(textProvider, ServiceProvider.GetRequiredService<ICorrectChecker>(), mistakeProcessor,
            ServiceProvider.GetRequiredService<IStatisticsInfo>(), ServiceProvider.GetRequiredService<ITimerProvider>());

        return typing;

    }
}

