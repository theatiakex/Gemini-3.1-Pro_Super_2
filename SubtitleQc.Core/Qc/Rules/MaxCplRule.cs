using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MaxCplRule : IQcRule
{
    private readonly int _threshold;

    public MaxCplRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(cue => 
        {
            bool exceeds = cue.Lines.Any(line => line.Length > _threshold);
            return new QcResult(cue.Id, nameof(MaxCplRule), exceeds ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
