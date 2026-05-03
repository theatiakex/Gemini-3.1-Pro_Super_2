using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public class WebVttParser : ISubtitleParser
{
    public IEnumerable<Cue> Parse(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            yield break;
        }

        var text = content.Replace("\r\n", "\n");
        if (!text.StartsWith("WEBVTT"))
        {
            yield break;
        }

        var blocks = text.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var block in blocks)
        {
            if (block.StartsWith("WEBVTT") || block.StartsWith("NOTE"))
            {
                continue;
            }

            var cue = ParseBlock(block);
            if (cue != null)
            {
                yield return cue;
            }
        }
    }

    private Cue? ParseBlock(string block)
    {
        var lines = block.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 1)
        {
            return null;
        }

        int timeLineIndex = 0;
        string id = Guid.NewGuid().ToString("N");
        
        var timeMatch = Regex.Match(lines[0], @"(?:(\d{2}:)?(\d{2}:\d{2}\.\d{3}))\s*-->\s*(?:(\d{2}:)?(\d{2}:\d{2}\.\d{3}))");
        
        if (!timeMatch.Success && lines.Length > 1)
        {
            timeMatch = Regex.Match(lines[1], @"(?:(\d{2}:)?(\d{2}:\d{2}\.\d{3}))\s*-->\s*(?:(\d{2}:)?(\d{2}:\d{2}\.\d{3}))");
            if (timeMatch.Success)
            {
                id = lines[0].Trim();
                timeLineIndex = 1;
            }
        }

        if (!timeMatch.Success)
        {
            return null;
        }

        var startStr = timeMatch.Groups[1].Success ? timeMatch.Groups[1].Value + timeMatch.Groups[2].Value : "00:" + timeMatch.Groups[2].Value;
        var endStr = timeMatch.Groups[3].Success ? timeMatch.Groups[3].Value + timeMatch.Groups[4].Value : "00:" + timeMatch.Groups[4].Value;

        if (!TimeSpan.TryParseExact(startStr, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture, out var start))
        {
            return null;
        }

        if (!TimeSpan.TryParseExact(endStr, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture, out var end))
        {
            return null;
        }

        var cueLines = new List<string>();
        for (int i = timeLineIndex + 1; i < lines.Length; i++)
        {
            cueLines.Add(lines[i].TrimEnd());
        }

        return new Cue(id, start, end, cueLines);
    }
}
