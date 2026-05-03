using System.Collections.Generic;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MaxLinesRule : IQcRule
{
    private readonly int _threshold;

    public MaxLinesRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IEnumerable<Cue> cues)
    {
        foreach (var cue in cues)
        {
            var status = cue.Lines.Count > _threshold ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, status, nameof(MaxLinesRule));
        }
    }
}
