using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MaxCpsRule : IQcRule
{
    private readonly int _threshold;

    public MaxCpsRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IEnumerable<Cue> cues)
    {
        foreach (var cue in cues)
        {
            var duration = (cue.End - cue.Start).TotalSeconds;
            var chars = cue.Lines.Sum(l => l.Length);
            var cps = duration > 0 ? chars / duration : double.MaxValue;
            var status = cps > _threshold ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, status, nameof(MaxCpsRule));
        }
    }
}
