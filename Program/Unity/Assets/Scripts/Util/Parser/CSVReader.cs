using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CSVReader
{
    public class Row
    {
        public string[ ] cell;

        public int GetInt(int i)
        {
            int value = 0;
            if (i >= cell.Length)
                return value;

            if (string.IsNullOrEmpty(cell[i]) == false)
                int.TryParse(cell[i], out value);

            return value;
        }

        public float GetFloat(int i)
        {
            float value = 0;

            if (string.IsNullOrEmpty(cell[i]) == false)
                float.TryParse(cell[i], out value);

            return value;
        }

        public string GetString(int i)
        {
            return cell[i];
        }

        public bool GetBool(int i)
        {
            return cell[i] == "TRUE";
        }
    }

    string fullText;
    Row[ ] row;

    public int rowCount;
    public int colCount;

    public static CSVReader Load(string path)
    {
        TextAsset asset = Resources.Load(path) as TextAsset;
        string data = asset.text;

        CSVReader reader = new CSVReader( );
        reader.fullText = data; //asset.text;

        string[ ] lines = reader.fullText.Split('\r');

        reader.rowCount = lines.Length - 1;
        reader.row = new Row[reader.rowCount];

        reader.colCount = 0;
        for (int i = 0; i < reader.rowCount; i++)
        {
            reader.row[i] = new Row( );

            reader.row[i].cell = lines[i].Split(',');
            if (reader.row[i].cell[0].Length > 0)
            {
                //reader.row[i].cell[0] = reader.row[i].cell[0].Substring(0, reader.row[i].cell[0].Length - 1);
                reader.row[i].cell[0] = reader.row[i].cell[0].Replace("\n", "");
            }
            reader.colCount = Mathf.Max(reader.colCount, reader.row[i].cell.Length);
        }

        return reader;
    }

    public Row GetRow(int i)
    {
        if (row.Length <= i)
            return null;

        return row[i];
    }
}