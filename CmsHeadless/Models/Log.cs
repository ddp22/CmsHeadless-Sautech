using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CmsHeadless.Models{
    public class Log{
        [Key]
        [Required]
        public int LogID { get; set; }
        public DateTime LogDateTime { get; set; }
        [MaxLength(300, ErrorMessage = "Log Details must be 300 characters or less")]
        public string? LogDetails { get; set; }
        [ForeignKey("Id")]
        [Required]
        public string UserID { get; set; }
        [ForeignKey("Log_eventID")]
        [Required]
        public int Log_eventID { get; set; }
        [MaxLength(300, ErrorMessage = "Log Notes must be 300 characters or less")]
        public string? LogNotes { get; set; }
        [MaxLength(15, ErrorMessage = "Log Browser must be 15 characters or less")]
        public string? LogBrowser { get; set; }
        [MaxLength(10, ErrorMessage = "Log Browser Version must be 10 characters or less")]
        public string? LogBrowserVersion { get; set; }
        public string LogIPAddress { get; set; }
        [MaxLength(10, ErrorMessage = "Log OS must be 10 characters or less")]
        public string? LogOS { get; set; }
        [MaxLength(10, ErrorMessage = "Log OS Version must be 10 characters or less")]
        public string? LogOSVersion { get; set; }
        public LogEvent LogEvent { get; set; } = null!;
        public User Id { get; set; }
        public Log()
        {

        }
        public Log(int logID, DateTime logDateTime, string? logDetails, string userID, int log_eventID, int lessonID, string? logNotes, string? logBrowser, string? logBrowserVersion, string logIPAddress, string? logOS, string? logOSVersion, LogEvent logEvent)
        {
            LogID = logID;
            LogDateTime = logDateTime;
            LogDetails = logDetails;
            UserID = userID;
            Log_eventID = log_eventID;
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
