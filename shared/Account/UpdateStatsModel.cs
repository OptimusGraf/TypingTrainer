namespace TypingBlazor.Account;

public class UpdateStatsModel
{
    public int TotalCharCount { get; set; }
    public int CorrectCharCount { get; set; }
    public double TimeTrainedInSeconds { get; set; }
    public int LastCharacterPerMinute { get; set; }
    public double LastAccuracy { get; set; }
}