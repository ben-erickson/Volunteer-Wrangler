using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using VolunteerOrganizer.Library;
using Microsoft.Data.SqlClient;

namespace VolunteerOrganizer.Pages
{
    public class EventViewModel : PageModel
    {
        public List<Event> EventList { get; set; }
        public void OnGet()
        {
            // Initialize eventList to an empty list
            this.EventList = new List<Event>();

            // Query the database to get all of the Events
            DataTable events = SQLWorker.ExecuteQuery(new SqlCommand("select EventGUID from Event"));

            for (int i = 0; i < events.Rows.Count; i++)
            {
                EventList.Add(new Event((Guid)events.Rows[i][0]));
            }
        }
    }
}
