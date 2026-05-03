using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public class MinFramesFromShotChangeRule : IQcRule
{
    private readonly IShotChangeProvider _provider;
    private readonly int _thresholdFrames;

    public MinFramesFromShotChangeRule(IShotChangeProvider provider, int thresholdFrames)
    {
        _provider = provider;
        _thresholdFrames = thresholdFrames;
    }

    public IEnumerable<QcResult> Evaluate(IEnumerable<Cue> cues)
    {
        var cuts = _provider.GetShotChangeFrames();
        foreach (var cue in cues)
        {
            var isTooClose = false;
            if (cue.StartFrame.HasValue)
            {
                isTooClose = cuts.Any(cut => Math.Abs(cue.StartFrame.Value - cut) < _thresholdFrames);
            }
            yield return new QcResult(cue.Id, isTooClose ? QcStatus.Failed : QcStatus.Passed, nameof(MinFramesFromShotChangeRule));
        }
    }
}
