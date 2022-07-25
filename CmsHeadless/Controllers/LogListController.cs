using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsHeadless.Models;

namespace CmsHeadless.Controllers
{
    public class LogListController : Controller
    {

        //Constant Event Codes
        public const int ApplicationErrorCode = 1;
        public const int DbErrorCode = 2;
        public const int UnclassifiedErrorCode = 3;
        public const int LoginWrongUsernameWarningCode = 4;
        public const int LoginWrongEmailPasswordWarningCode = 5;        
        public const int UnclassifiedWarningCode = 6;
        public const int LoginSuccessfulCode = 7;
        public const int LogoutSuccessfulCode = 8;
        public const int UnclassifiedInfo = 9;
        public const int ContentsModifiedCode = 20;
        public const int ContentsDeletedCode = 21;
        public const int ContentsModifiedWarningCode = 22;
        public const int ContentsDeletedWarningCode = 23;
        public const int ContentsCreatedCode = 24;
        public const int ContentsCreatedWarningCode = 25;

        private readonly CmsHeadlessDbContext _contextDb;
        public LogListController(CmsHeadlessDbContext contextDb)
        {
            _contextDb = contextDb;
        }
        public int SaveLog(string username, int logEventId, string logDetails, string logNotes, HttpContext httpContext)
        {
            string userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            string browser;
            if (userAgent.Contains("Edg/"))
            {
                browser = "Edge";
            }
            else
            {
                browser = httpContext.Request.Browser().Type.ToString();
            }

            var temp = _contextDb.User.Where(c => c.Email == username).Select(c => c.Id).ToList();
            string userId;

            if (temp.Count > 0)
            {
                userId = temp[temp.Count - 1];
            }
            else
            {
                userId = "Not defined";
            }

            Log tmpLog = new Log();
            tmpLog.UserId = userId;
            tmpLog.LogIPAddress = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[1].ToString();
            tmpLog.LogBrowserVersion = httpContext.Request.Browser().Version.ToString();
            tmpLog.LogBrowser = browser;
            tmpLog.LogDateTime = DateTime.Now.Date + DateTime.Now.TimeOfDay;
            tmpLog.LogDetails = logDetails;
            tmpLog.LogEventLog_eventID = logEventId;
            tmpLog.LogNotes = logNotes;
            tmpLog.LogOS = Environment.OSVersion.ToString()[..(Environment.OSVersion.ToString().Length - 16)];
            tmpLog.LogOSVersion = Environment.OSVersion.Version.ToString()[..7];
            _contextDb.Log.Add(tmpLog);
            return _contextDb.SaveChanges();
        }
    }
}
