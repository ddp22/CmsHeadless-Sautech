using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models{
    public class LogType{
        [Key]
        [Required]
        public int Log_typeID { get; set; }
        public string? Log_typeDescription { get; set; }
        public string Log_typeCode { get; set; }
        //public ICollection<LogEvent> LogEvents { get; set; }

        public LogType()
        {

        }

        public LogType(int log_typeID, string? log_typeDescription, string log_typeCode)
        {
            Log_typeID = log_typeID;
            Log_typeDescription = log_typeDescription;
            Log_typeCode = log_typeCode;
        }
    }
}
