using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsHeadless.Models;
using System.Net.Sockets;

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
        public const int ContentsModifiedCode = 10;
        public const int ContentsDeletedCode = 11;
        public const int ContentsModifiedWarningCode = 12;
        public const int ContentsDeletedWarningCode = 13;
        public const int ContentsCreatedCode = 14;
        public const int ContentsCreatedWarningCode = 15;

        private readonly CmsHeadlessDbContext _contextDb;
        public LogListController(CmsHeadlessDbContext contextDb)
        {
            _contextDb = contextDb;
        }
        public int SaveLog(string username, int logEventId, string logDetails, string logNotes, HttpContext httpContext)
        {
            Log tmpLog = new Log();

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

            var temp = _contextDb.CmsUser.Where(c => c.Email == username).Select(c => c.Id).ToList();
            string userId;

            if (temp.Count > 0)
            {
                userId = temp[temp.Count - 1];
            }
            else
            {
                userId = null;
            }

            var addresses = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
            foreach (var ip in addresses.ToList())
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    tmpLog.LogIPAddress = ip.ToString();
                }
            }
            
            tmpLog.UserId = userId;
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
