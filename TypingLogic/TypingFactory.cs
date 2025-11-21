using Microsoft.Extensions.DependencyInjection;

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
            Config.TypingType.Usual=> ServiceProvider.GetRequiredService<TextFromTXTProvider>(),
            _ => throw new NotImplementedException(),
        };
        IMistakeProcessor mistakeProcessor = config.Difficulty   switch
        {
            Config.TypingDifficulty.Easy =>  new SimpleMistakeProcessor(),
            Config.TypingDifficulty.Hard=> new AdvancedMistakeProcessor(),
            _ => throw new NotImplementedException(),
        };

        ITyping typing = new Typing(textProvider, ServiceProvider.GetRequiredService<ICorrectChecker>(), mistakeProcessor,
            ServiceProvider.GetRequiredService<IStatisticsInfo>(), ServiceProvider.GetRequiredService<ITimerProvider>());

        return typing;

    }
}

