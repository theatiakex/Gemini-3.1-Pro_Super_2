using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class OverlapCheckRule : IQcRule
{
    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        var results = new List<QcResult>();
        var sortedCues = cues.OrderBy(c => c.Start).ToList();
        
        TimeSpan? previousEnd = null;

        foreach (var cue in sortedCues)
        {
            bool overlaps = previousEnd.HasValue && cue.Start < previousEnd.Value;
            results.Add(new QcResult(cue.Id, nameof(OverlapCheckRule), overlaps ? QcStatus.Failed : QcStatus.Passed));
            
            if (!previousEnd.HasValue || cue.End > previousEnd.Value)
            {
                previousEnd = cue.End;
            }
        }

        return results;
    }
}
