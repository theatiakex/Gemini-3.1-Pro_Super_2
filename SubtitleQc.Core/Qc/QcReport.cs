using System.Collections.Generic;

namespace SubtitleQc.Core.Qc;

public sealed class QcReport
{
    public IReadOnlyList<QcResult> Results { get; }

    public QcReport(IReadOnlyList<QcResult> results)
    {
        Results = results ?? System.Array.Empty<QcResult>();
    }
}
