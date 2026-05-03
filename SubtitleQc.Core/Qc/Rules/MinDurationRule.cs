using System;
using System.Collections.Generic;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MinDurationRule : IQcRule
{
    private readonly TimeSpan _threshold;

    public MinDurationRule(TimeSpan threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IEnumerable<Cue> cues)
    {
        foreach (var cue in cues)
        {
            var status = (cue.End - cue.Start) < _threshold ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, status, nameof(MinDurationRule));
        }
    }
}
