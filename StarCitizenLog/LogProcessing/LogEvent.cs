namespace StarCitizenLog.LogProcessing;

public class LogEvent
{
    public string Message;
    public DateTime EventDate;
    
    public LogEvent(string log)
    {
        EventDate = DateTime.Now;
        Message = log;
    }
}