using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TypingTrainer.Logic;

public interface ITextProvider
{
    string GetText();
}

public interface ICorrectChecker
{
    bool IsCorrect(string text, int cursor, char c);
}
public interface ITyping
{

    string Text { get; }
    int Cursor { get; }
    bool InputChar(char c);
    IStatisticsInfo StatisticsProvider { get; }
    ITimerProvider Timer { get; }

}
public interface IMistakeProcessor
{
    void ProcessMistake(ref int cursorPosition, bool isCorrect, string text);
}
public interface ITimerProvider
{
    TimeSpan Time { get; }
    void Start();
    void Stop();

}
public class SimpleTimer : ITimerProvider
{
    public TimeSpan Time { get { return endTime < startTime ? DateTime.Now - startTime : endTime - startTime; } }
    private DateTime startTime;
    private DateTime endTime = DateTime.MinValue;
    public SimpleTimer()
    {

    }

    public void Start()
    {
        startTime = DateTime.Now;
    }
    public void Stop()
    {
        endTime = DateTime.Now;

    }


}
public interface IStatisticsInfo
{
    int TotalChars { get; }
    int CorrectChars { get; }
    int IncorrectChars { get; }
    double Accuracy { get; }
    int CharacterPerMinute { get; }
    TimeSpan TimeForLastChar { get; }
    void RecordInput(bool isCorrect, TimeSpan time);

}
public class SimpleStatisticsInfo : IStatisticsInfo
{
    public int TotalChars { get; private set; }
    public int CorrectChars { get; private set; }
    public int IncorrectChars { get; private set; }
    public TimeSpan TimeForLastChar { get; private set; }
    public double Accuracy
    {
        get
        {
            if (TotalChars == 0) return 0;
            return (double)CorrectChars / TotalChars;
        }
    }
    public int CharacterPerMinute
    {
        get
        { // исправить ошибку!!
            if (CorrectChars == 0)
                return 0;
            return (int)(CorrectChars / ((double)TimeForLastChar.Seconds / 60));
        }
    }
    public void RecordInput(bool isCorrect, TimeSpan t)
    {
        TotalChars++;
        if (isCorrect)
        {
            CorrectChars++;
        }
        else
        {
            IncorrectChars++;
        }
        TimeForLastChar = t;
    }
}
public class SimpleMistakeProcessor : IMistakeProcessor
{
    public void ProcessMistake(ref int cursorPosition, bool isCorrect, string text)
    {
        if (isCorrect)
        {
            cursorPosition++;
        }
    }
}
public class AdvancedMistakeProcessor : IMistakeProcessor
{
    public void ProcessMistake(ref int cursorPosition, bool isCorrect, string text)
    {

        if (isCorrect)
        {
            cursorPosition++;
        }
        else
        {
            while (cursorPosition >= 1 && text[cursorPosition - 1] != '!' && text[cursorPosition - 1] != '.' && text[cursorPosition - 1] != '?')
            {
                cursorPosition--;
            }
        }
    }
}
public class SimpleTextProvider : ITextProvider
{
    string text = "Это простой текст. Он состоит из четырёх предложений. Он нужен для тестирования ввода. Программа должна читать его целиком.";
    public string GetText()
    {
        return text;
    }
}

public class SimpleCorrectChecker : ICorrectChecker
{

    public bool IsCorrect(string text, int cursor, char c)
    {
        if (text == null) return false;
        if (cursor < 0 || cursor >= text.Length) return false;

        return char.ToLowerInvariant(text[cursor]) == char.ToLowerInvariant(c);
    }
}

public class Typing : ITyping
{
    public ITimerProvider Timer { get; private set; }
    private IStatisticsInfo statisticsProvider;
    public IStatisticsInfo StatisticsProvider
    {
        get => statisticsProvider;
    }
    private readonly ICorrectChecker correctChecker;

    public ITextProvider textProvider;
    IMistakeProcessor mistakeProcessor;
    int cursorPosition = 0;
    string text;


    public string Text
    {
        get => text;
    }


    public int Cursor
    {
        get => cursorPosition;
    }
    public Typing(ITextProvider textProvider, ICorrectChecker correctChecker, IMistakeProcessor mistakeProcessor, IStatisticsInfo statisticsProvider, ITimerProvider timer)
    {
        this.textProvider = textProvider;
        this.correctChecker = correctChecker;
        this.mistakeProcessor = mistakeProcessor;
        text = textProvider.GetText();
        this.statisticsProvider = statisticsProvider;
        Timer = timer;
    }


    public bool InputChar(char c)
    {
        if (Timer.Time == TimeSpan.Zero)
        {
            Timer.Start();
        }

        bool isCorrect = correctChecker.IsCorrect(text, cursorPosition, c);
        mistakeProcessor.ProcessMistake(ref cursorPosition, isCorrect, text);
        statisticsProvider.RecordInput(isCorrect, Timer.Time);
        if (cursorPosition >= text.Length)
        {
            Timer.Stop();
        }
        return isCorrect;
    }
}


