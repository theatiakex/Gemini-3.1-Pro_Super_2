using System;
using System.Collections.Generic;

namespace SubtitleQc.Core.Models;

public sealed class Cue
{
    public string Id { get; }
    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    public IReadOnlyList<string> Lines { get; }
    public int? StartFrame { get; }

    public Cue(string id, TimeSpan start, TimeSpan end, IReadOnlyList<string> lines, int? startFrame = null)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Start = start;
        End = end;
        Lines = lines ?? Array.Empty<string>();
        StartFrame = startFrame;
    }
}
