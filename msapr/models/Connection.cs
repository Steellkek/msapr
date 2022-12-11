namespace msapr;

public class Connection
{
    public Element _element1{ get; set; }
    public Element _element2{ get; set; }
    public int _value{ get; set; }

    public Connection(Element element1, Element element2, int value)
    {
        _element1 = element1;
        _element2 = element2;
        _value = value;
    }
}