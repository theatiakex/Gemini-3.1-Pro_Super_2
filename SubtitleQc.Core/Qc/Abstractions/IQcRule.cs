using System.Collections.Generic;

namespace SubtitleQc.Core.Qc.Abstractions;

public interface IQcRule
{
    IEnumerable<QcResult> Evaluate(IReadOnlyList<Models.Cue> cues);
}
