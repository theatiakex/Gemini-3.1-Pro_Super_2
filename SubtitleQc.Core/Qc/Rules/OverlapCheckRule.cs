using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class OverlapCheckRule : IQcRule
{
    public IEnumerable<QcResult> Evaluate(IEnumerable<Cue> cues)
    {
        var cueList = cues.OrderBy(c => c.Start).ToList();
        for (int i = 0; i < cueList.Count; i++)
        {
            var current = cueList[i];
            var isOverlap = false;

            if (i > 0)
            {
                var previous = cueList[i - 1];
                if (current.Start < previous.End)
                {
                    isOverlap = true;
                }
            }

            yield return new QcResult(current.Id, isOverlap ? QcStatus.Failed : QcStatus.Passed, nameof(OverlapCheckRule));
        }
    }
}
