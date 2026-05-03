using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public class SrtParser : ISubtitleParser
{
    public IEnumerable<Cue> Parse(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            yield break;
        }

        var blocks = content.Replace("\r\n", "\n").Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var block in blocks)
        {
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
        if (lines.Length < 2)
        {
            return null;
        }

        var id = lines[0].Trim();
        var timeMatch = Regex.Match(lines[1], @"(\d{2}:\d{2}:\d{2},\d{3})\s*-->\s*(\d{2}:\d{2}:\d{2},\d{3})");
        
        if (!timeMatch.Success)
        {
            return null;
        }

        if (!TimeSpan.TryParseExact(timeMatch.Groups[1].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture, out var start))
        {
            return null;
        }

        if (!TimeSpan.TryParseExact(timeMatch.Groups[2].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture, out var end))
        {
            return null;
        }

        var cueLines = new List<string>();
        for (int i = 2; i < lines.Length; i++)
        {
            cueLines.Add(lines[i].TrimEnd());
        }

        return new Cue(id, start, end, cueLines);
    }
}
