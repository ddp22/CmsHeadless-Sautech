using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CmsHeadless.Models{
    public class Log{
        [Key]
        [Required]
        public int LogID { get; set; }
        public DateTime LogDateTime { get; set; }
        public string? LogDetails { get; set; }
        [ForeignKey("Id")]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("Log_eventID")]
        [Required]
        public int LogEventLog_eventID { get; set; }
        public string? LogNotes { get; set; }
        public string? LogBrowser { get; set; }
        public string? LogBrowserVersion { get; set; }
        public string LogIPAddress { get; set; }
        public string? LogOS { get; set; }
        public string? LogOSVersion { get; set; }
        public LogEvent LogEvent { get; set; } = null!;
        public User User { get; set; }
        public Log()
        {

        }
        public Log(int logID, DateTime logDateTime, string? logDetails, string? userID, int log_eventID, int lessonID, string? logNotes, string? logBrowser, string? logBrowserVersion, string logIPAddress, string? logOS, string? logOSVersion, LogEvent logEvent)
        {
            LogID = logID;
            LogDateTime = logDateTime;
            LogDetails = logDetails;
            UserId = userID;
            LogEventLog_eventID = log_eventID;
            LogNotes = logNotes;
            LogBrowser = logBrowser;
            LogBrowserVersion = logBrowserVersion;
            LogIPAddress = logIPAddress;
            LogOS = logOS;
            LogOSVersion = logOSVersion;
            LogEvent = logEvent;
        }
    }
}
