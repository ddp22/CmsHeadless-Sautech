using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class LogEvent{
        [Key]
        public int Log_eventID { get; set; }
        public string? Log_eventDescription { get; set; }
        public int Log_eventCode { get; set; }
        public int Log_typeID { get; set; }
        public LogEvent()
        {

        }
        public LogEvent(int log_eventID, string log_eventDescription, int log_eventCode, int log_typeID)
        {
            Log_eventID = log_eventID;
            Log_eventDescription = log_eventDescription;
            Log_eventCode = log_eventCode;
            Log_typeID = log_typeID;
        }
    }
}
