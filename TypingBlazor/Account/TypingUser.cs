using Microsoft.AspNetCore.Identity;
using TypingBlazor.Account;
namespace TypingBlazor;

public class TypingUser : IdentityUser
{
    public int TotalCharCount { get; set; } = 0;
    public int CorrectCharCount { get; set; } = 0;
    public double Accuracy
    {
        get
        {
            if (TotalCharCount == 0)
                return 0;
            return (double)CorrectCharCount / TotalCharCount;
        }
       
    }
    public TimeSpan TimeTrained { get; set; } = TimeSpan.Zero;

    public  StatisticsOfLastTraining StatisticsOfLastTraining { get; set; } = new StatisticsOfLastTraining();


}
