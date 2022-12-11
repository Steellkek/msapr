namespace msapr;

public class Module
{
    public List<Element> Elements { get; set; } = new();
    public int Square{ get; set; }
    public int Cnt{ get; set; }

    public Module(int count, int square)
    {
        Square = square;
        Cnt = count;
    }
}