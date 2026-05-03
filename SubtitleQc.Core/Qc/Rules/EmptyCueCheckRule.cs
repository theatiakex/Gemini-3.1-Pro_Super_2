using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class EmptyCueCheckRule : IQcRule
{
    public IEnumerable<QcResult> Evaluate(IEnumerable<Cue> cues)
    {
        foreach (var cue in cues)
        {
            var status = cue.Lines.All(string.IsNullOrWhiteSpace) ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, status, nameof(EmptyCueCheckRule));
        }
    }
}
