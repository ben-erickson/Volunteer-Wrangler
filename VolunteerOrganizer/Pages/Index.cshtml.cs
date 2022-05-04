using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VolunteerOrganizer.Library;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<Event> events { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            this.events = new List<Event>();

            SqlCommand command = new SqlCommand("select EventGUID from Event");
            DataTable queryResult = SQLWorker.ExecuteQuery(command);

            foreach (DataRow row in queryResult.Rows)
            {
                this.events.Add(new Event((Guid)row[0]));
            }
        }
    }
}