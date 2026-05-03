using System;

namespace SubtitleQc.Core.Qc;

public sealed class QcResult
{
    public string CueId { get; }
    public string RuleName { get; }
    public QcStatus Status { get; }
    public string Message { get; }

    public QcResult(string cueId, string ruleName, QcStatus status, string message = "")
    {
        CueId = cueId ?? throw new ArgumentNullException(nameof(cueId));
        RuleName = ruleName ?? throw new ArgumentNullException(nameof(ruleName));
        Status = status;
        Message = message ?? string.Empty;
    }
}
