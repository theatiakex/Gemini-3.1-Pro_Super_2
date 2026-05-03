using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MaxCplRule : IQcRule
{
    private readonly int _threshold;

    public MaxCplRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IEnumerable<Cue> cues)
    {
        foreach (var cue in cues)
        {
            var status = cue.Lines.Any(line => line.Length > _threshold) ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, status, nameof(MaxCplRule));
        }
    }
}
