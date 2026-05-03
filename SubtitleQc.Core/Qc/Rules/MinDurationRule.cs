using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MinDurationRule : IQcRule
{
    private readonly TimeSpan _threshold;

    public MinDurationRule(TimeSpan threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(cue => 
        {
            TimeSpan duration = cue.End - cue.Start;
            return new QcResult(cue.Id, nameof(MinDurationRule), duration < _threshold ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
