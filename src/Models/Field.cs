namespace AnkiNet.Models;

public class Field
{
    private int _ord;

    public string Name;
    public bool Rtl = false;
    public bool Sticky = false;
    public string Media = "[]";
    public string Font;
    public int Size;

    public Field(string name, string font = "Arial", int size = 12)
    {
        Name = name;
        Font = font;
        Size = size;
    }

    public void SetOrd(int ord)
    {
        _ord = ord;
    }

    public string ToJSON()
    {
        return "{\"name\": \"" + Name + "\", \"rtl\": " + Rtl.ToString().ToLower() + ", \"sticky\": " + Sticky.ToString().ToLower() + ", \"media\": " + Media + ", \"ord\": " + _ord + ", \"font\": \"" + Font + "\", \"size\": " + Size + "}";
    }

    public override string ToString()
    {
        return "{{" + Name + "}}";
    }
}