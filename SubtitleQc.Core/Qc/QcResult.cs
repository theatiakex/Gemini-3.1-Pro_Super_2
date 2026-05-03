namespace SubtitleQc.Core.Qc;

public class QcResult
{
    public string CueId { get; }
    public QcStatus Status { get; }
    public string RuleName { get; }

    public QcResult(string cueId, QcStatus status, string ruleName)
    {
        CueId = cueId;
        Status = status;
        RuleName = ruleName;
    }
}
