using System.ComponentModel.DataAnnotations;
namespace TypingTrainer.Logic;


public class Config
{
    public Config()
    {
    }
    public TypingLanguage Language { get; set; }
    public TypingType Type { get; set; }
    public TypingDifficulty Difficulty { get; set; }
    public enum TypingLanguage
    {
        [Display(Name = "Русский")]
        Russian
    }
    public enum TypingType
    {
        [Display(Name = "Обычный")]
        Usual
    }
    public enum TypingDifficulty
    {
        [Display(Name = "Легко")]
        Easy,
        [Display(Name = "Сложно")]
        Hard
    }
}

