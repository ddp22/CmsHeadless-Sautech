using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class LogEvent{
        [Key]
        [Required]
        public int Log_eventID { get; set; }
        public string? Log_eventDescription { get; set; }
        public string Log_eventCode { get; set; }
        [ForeignKey("Log_typeID")]
        [Required]
        public int LogTypeLog_typeID { get; set; }
        public LogType LogType { get; set; } = null!;
        //public ICollection<Log> Logs { get; set; }
        public LogEvent()
        {

        }
        public LogEvent(int log_eventID, string log_eventDescription, string log_eventCode, int log_typeID)
        {
            Log_eventID = log_eventID;
            Log_eventDescription = log_eventDescription;
            Log_eventCode = log_eventCode;
            LogTypeLog_typeID = log_typeID;
        }
    }
}
