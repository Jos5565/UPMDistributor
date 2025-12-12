using UnityEngine;
using UnityEditor;
using System.IO;


public class PrettyJson
{
    public static string Parse(string json)
    {
        if (string.IsNullOrEmpty(json))
            return json;

        int indent = 0;
        bool quoted = false;
        var sb = new System.Text.StringBuilder();

        for (int i = 0; i < json.Length; i++)
        {
            char ch = json[i];

            switch (ch)
            {
                case '{':
                case '[':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        indent++;
                        sb.Append(new string(' ', indent * 2));
                    }
                    break;

                case '}':
                case ']':
                    if (!quoted)
                    {
                        sb.AppendLine();
                        indent--;
                        sb.Append(new string(' ', indent * 2));
                    }
                    sb.Append(ch);
                    break;

                case '"':
                    sb.Append(ch);
                    bool escaped = false;
                    int index = i;
                    while (index > 0 && json[--index] == '\\')
                        escaped = !escaped;

                    if (!escaped)
                        quoted = !quoted;
                    break;

                case ',':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        sb.Append(new string(' ', indent * 2));
                    }
                    break;

                case ':':
                    sb.Append(ch);
                    if (!quoted)
                        sb.Append(" ");
                    break;

                default:
                    sb.Append(ch);
                    break;
            }
        }

        return sb.ToString();
    }
}