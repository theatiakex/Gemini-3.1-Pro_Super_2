using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public class TtmlParser : ISubtitleParser
{
    public IEnumerable<Cue> Parse(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            yield break;
        }

        XDocument doc;
        try
        {
            doc = XDocument.Parse(content);
        }
        catch
        {
            yield break;
        }

        var ns = doc.Root?.Name.Namespace ?? XNamespace.None;
        var body = doc.Root?.Element(ns + "body");
        if (body == null)
        {
            yield break;
        }

        var queue = new Queue<XElement>();
        queue.Enqueue(body);

        while (queue.Count > 0)
        {
            var element = queue.Dequeue();

            if (element.Name.LocalName == "p")
            {
                var cue = ParseCue(element);
                if (cue != null)
                {
                    yield return cue;
                }
            }
            else
            {
                foreach (var child in element.Elements())
                {
                    queue.Enqueue(child);
                }
            }
        }
    }

    private Cue? ParseCue(XElement pElement)
    {
        var beginAttr = pElement.Attribute("begin");
        var endAttr = pElement.Attribute("end");

        if (beginAttr == null || endAttr == null)
        {
            return null;
        }

        if (!TryParseTime(beginAttr.Value, out var start) || !TryParseTime(endAttr.Value, out var end))
        {
            return null;
        }

        var idAttr = pElement.Attribute(XNamespace.Xml + "id") ?? pElement.Attribute("id");
        var id = idAttr?.Value ?? Guid.NewGuid().ToString("N");

        var lines = new List<string>();
        string currentLine = "";
        ExtractText(pElement, lines, ref currentLine);

        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }

        return new Cue(id, start, end, lines);
    }

    private void ExtractText(XElement element, List<string> lines, ref string currentLine)
    {
        foreach (var node in element.Nodes())
        {
            if (node is XText textNode)
            {
                currentLine += textNode.Value.Replace("\n", "").Replace("\r", "");
            }
            else if (node is XElement childElement)
            {
                if (childElement.Name.LocalName == "br")
                {
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        lines.Add(currentLine);
                    }
                    currentLine = "";
                }
                else
                {
                    ExtractText(childElement, lines, ref currentLine);
                }
            }
        }
    }

    private bool TryParseTime(string timeStr, out TimeSpan time)
    {
        if (TimeSpan.TryParseExact(timeStr, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture, out time) ||
            TimeSpan.TryParseExact(timeStr, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out time))
        {
            return true;
        }
        
        return false;
    }
}
