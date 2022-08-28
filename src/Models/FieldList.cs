using System;
using System.Collections.Generic;
using System.Linq;

namespace AnkiNet.Models;

public class FieldList : List<Field>
{
    private string _format;

    public FieldList()
    {
    }

    public FieldList(string format)
    {
        _format = format;
    }

    public new void Add(Field field)
    {
        field.SetOrd(Count);
        base.Add(field);
    }

    public string ToJSON()
    {
        var json = from field in base.FindAll(x => x != null)
                   select field.ToJSON();

        return string.Join(",\n", json.ToArray());
    }

    public string ToFrontBack()
    {
        return string.Join("\\n<hr id=answer />\\n", (object[])ToArray());
    }

    public override string ToString()
    {
        return string.Join("\\n<br>\\n", (object[])ToArray());
    }

    public string Format(string format)
    {
        var array = ToArray();
        
        for (int i = 0; i < array.Length; ++i)
        {
            format = format.Replace("{" + i + "}", array[i].ToString());
        }

        return format;
    }
}