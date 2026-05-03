using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MaxCpsRule : IQcRule
{
    private readonly double _threshold;

    public MaxCpsRule(double threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        return cues.Select(cue => 
        {
            int totalChars = cue.Lines.Sum(line => line.Length);
            double durationSeconds = (cue.End - cue.Start).TotalSeconds;
            
            // Handle divide by zero
            double cps = durationSeconds > 0 ? totalChars / durationSeconds : double.MaxValue;
            
            return new QcResult(cue.Id, nameof(MaxCpsRule), cps > _threshold ? QcStatus.Failed : QcStatus.Passed);
        });
    }
}
