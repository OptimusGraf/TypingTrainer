namespace TypingBlazor.Account;

public class StatisticsOfLastTraining
{
    public  StatisticsOfLastTraining()
    {
        characterPerMinute = 0;
        Accuracy = 0;
    }
    public int characterPerMinute { get; set; }
    public double Accuracy { get; set; }
}