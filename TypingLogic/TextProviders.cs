
namespace TypingTrainer.Logic;

public interface ITextProvider
{
    string GetText();
}
public class SimpleTextProvider : ITextProvider
{
    string text = "Это простой текст. Он состоит из четырёх предложений. Он нужен для тестирования ввода. Программа должна читать его целиком.";
    public string GetText()
    {
        return text;
    }
}

public class TextFromTXTProvider : ITextProvider
{
    string path = "Text.txt";
    Random random;
    List<string> texts;
    public TextFromTXTProvider()
    {

        this.random = new Random();

        var assymly = typeof(TextFromTXTProvider).Assembly;

        string? resourceName = assymly.GetManifestResourceNames()
            .FirstOrDefault(name => name.EndsWith("Text.txt"));

        if (resourceName == null)
            throw new FileNotFoundException($" файл  не найден");
        using Stream stream = assymly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        texts = new List<string>();

        while (!reader.EndOfStream)
        { 
            string currentLine = reader.ReadLine();
            if (!string.IsNullOrWhiteSpace(currentLine))
                texts.Add(currentLine);
        }
  


    }

    int count = 0;

    public string GetText()
    {
        count++;
        if (count < 5)
            return texts[random.Next(texts.Count)];
        else
        {
            count = 0;
            return null;
        }



    }
}

