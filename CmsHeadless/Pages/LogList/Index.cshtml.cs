using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using System.Data;
using CmsHeadless.Pages.LogList;

namespace CmsHeadless.Pages.LogList
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public const int numberPage = 5;
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;

        [BindProperty(Name = "searchString", SupportsGet = true)]
        public string searchString { get; set; }
        public Models.Log LogNew { get; set; }
        public List<Models.Log> LogAvailable { get; set; }
        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            LogAvailable = new List<Models.Log>();
        }
        public LogList<Models.Log> LogList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            IQueryable<Models.Log> selectLogQuery;
            IQueryable<Models.LogEvent> selectLogEventQuery;
            IQueryable<Models.LogType> selectLogTypeQuery;
            
            selectLogTypeQuery = (from LogType in _context.LogType select LogType);
            selectLogEventQuery = (from LogEvent in _context.LogEvent select LogEvent).Include(t => t.LogType);
            selectLogQuery = (from Log in _context.Log select Log).Include(u => u.User).Include(e => e.LogEvent).Include(et => et.LogEvent.LogType).
                OrderByDescending(a => a.LogDateTime);

            //List<Models.Log> listLog = _context.Log.Include(u => u.User).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                selectLogQuery = selectLogQuery.Where(s => s.User.Email.Contains(searchString) || s.LogBrowser.Contains(searchString)
                || s.LogBrowserVersion.Contains(searchString) || s.LogDetails.Contains(searchString) || s.LogEvent.Log_eventCode.Contains(searchString)
                || s.LogIPAddress.Contains(searchString) || s.LogEvent.Log_eventCode.Contains(searchString) || s.LogNotes.Contains(searchString) || s.LogOS.Contains(searchString)
                || s.LogOSVersion.Contains(searchString) || s.LogEvent.Log_eventDescription.Contains(searchString) || s.LogEvent.LogType.Log_typeCode.Contains(searchString)
                || s.LogEvent.LogType.Log_typeDescription.Contains(searchString)
                || (s.LogDateTime.Date.Day.ToString()+"/0"+s.LogDateTime.Date.Month.ToString().Contains(searchString)+"/"+s.LogDateTime.Date.Year.ToString()).Contains(searchString));
            }
            LogAvailable = selectLogQuery.ToList();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            LogList = await LogList<Models.Log>.CreateAsync(selectLogQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }
    }
}
