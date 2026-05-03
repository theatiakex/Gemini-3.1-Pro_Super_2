using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class CrossShotBoundaryCheckRule : IQcRule
{
    private readonly IShotChangeProvider _shotChangeProvider;

    public CrossShotBoundaryCheckRule(IShotChangeProvider shotChangeProvider)
    {
        _shotChangeProvider = shotChangeProvider ?? throw new ArgumentNullException(nameof(shotChangeProvider));
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        var cuts = _shotChangeProvider.GetShotChangeTimestamps();
        
        return cues.Select(cue => 
        {
            bool crosses = cuts.Any(cut => cut > cue.Start && cut < cue.End);
            return new QcResult(cue.Id, nameof(CrossShotBoundaryCheckRule), crosses ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
