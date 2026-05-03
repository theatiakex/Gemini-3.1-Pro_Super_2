using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class CrossShotBoundaryCheckRule : IQcRule
{
    private readonly IShotChangeProvider _provider;

    public CrossShotBoundaryCheckRule(IShotChangeProvider provider)
    {
        _provider = provider;
    }

    public IEnumerable<QcResult> Evaluate(IEnumerable<Cue> cues)
    {
        var cuts = _provider.GetShotChangeTimestamps();
        foreach (var cue in cues)
        {
            var spansCut = cuts.Any(cut => cut > cue.Start && cut < cue.End);
            yield return new QcResult(cue.Id, spansCut ? QcStatus.Failed : QcStatus.Passed, nameof(CrossShotBoundaryCheckRule));
        }
    }
}
