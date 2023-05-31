namespace SQLtoCSharpObjectList;

public class Program
{
    public static void Main()
    {
        ProcessFile.Process("data.txt", new TransformedClass(), typeof(TransformedClass).Name, false);
    }
}