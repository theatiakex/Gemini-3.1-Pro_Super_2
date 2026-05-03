using System;
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MinFramesFromShotChangeRule : IQcRule
{
    private readonly IShotChangeProvider _shotChangeProvider;
    private readonly int _thresholdFrames;

    public MinFramesFromShotChangeRule(IShotChangeProvider shotChangeProvider, int thresholdFrames)
    {
        _shotChangeProvider = shotChangeProvider ?? throw new ArgumentNullException(nameof(shotChangeProvider));
        _thresholdFrames = thresholdFrames;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        var cuts = _shotChangeProvider.GetShotChangeFrames();
        
        return cues.Select(cue => 
        {
            bool tooClose = false;
            
            if (cue.StartFrame.HasValue)
            {
                tooClose = cuts.Any(cut => Math.Abs(cue.StartFrame.Value - cut) < _thresholdFrames);
            }
            
            return new QcResult(cue.Id, nameof(MinFramesFromShotChangeRule), tooClose ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
