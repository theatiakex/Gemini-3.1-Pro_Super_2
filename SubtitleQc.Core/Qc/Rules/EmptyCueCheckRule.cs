using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class EmptyCueCheckRule : IQcRule
{
    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(cue => 
        {
            bool isEmpty = !cue.Lines.Any(line => !string.IsNullOrWhiteSpace(line));
            return new QcResult(cue.Id, nameof(EmptyCueCheckRule), isEmpty ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
