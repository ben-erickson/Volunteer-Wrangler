using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VolunteerOrganizer.Library;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Pages
{
    public class EventEditorModel : PageModel
    {
        public bool Editing { get; set; }
        public Event CurrentEvent { get; set; }
        public User LoggedInUser { get; set; }
        public List<Project> EventProjects { get; set; }
        // This list has the IndividualGUID and name of event's volunteers
        public List<(Guid, string, Guid)> EventVolunteers { get; set; }

        public EventEditorModel()
        {
            this.CurrentEvent = new Event();
            this.LoggedInUser = new User();
            this.Editing = false;
            this.EventProjects = new List<Project>();
            this.EventVolunteers = new List<(Guid, string, Guid)>();
        }

        public void OnGet(string eventId, string userId)
        {
            this.SetValues(eventId, userId);
        }

        public void OnPostEditEvent(string eventId, string userId)
        {
            this.SetValues(eventId, userId);

            this.Editing = true;
        }

        public void OnPostFinishEditing(string eventId, string userId)
        {
            this.SetValues(eventId, userId);

            string eventName = Request.Form["eventNameText"];
            string eventDescription = Request.Form["eventDescriptionText"];

            DateTime.TryParse(Request.Form["eventStartDate"].ToString(), out DateTime startDate);
            DateTime.TryParse(Request.Form["eventEndDate"].ToString(), out DateTime endDate);

            //DateTime startDate = DateTime.Parse((string)Request.Form["eventStartDate"]);
            //DateTime endDate = DateTime.Parse((string)Request.Form["eventEndDate"]);

            // Check to make sure that the given DateTimes are within SQL's acceptable ranges
            if (startDate < DateTime.Parse("01/01/1753") || startDate > DateTime.Parse("12/31/9999"))
            {
                startDate = DateTime.Parse("1/1/2000");
            }

            if (endDate < DateTime.Parse("01/01/1753") || endDate > DateTime.Parse("12/31/9999"))
            {
                endDate = DateTime.Parse("1/1/2000");
            }

            // Update the Event in the database
            SqlCommand eventUpdateCommand = new SqlCommand(
                "update Event " +
                "set EventName = @EventName, EventDescription = @EventDescription, StartDate = @StartDate, EndDate = @EndDate " +
                "where EventGUID = @EventGUID");
            eventUpdateCommand.Parameters.AddWithValue("@EventName", eventName);
            eventUpdateCommand.Parameters.AddWithValue("@EventDescription", eventDescription);
            eventUpdateCommand.Parameters.AddWithValue("@StartDate", startDate.ToString());
            eventUpdateCommand.Parameters.AddWithValue("@EndDate", endDate.ToString());
            eventUpdateCommand.Parameters.AddWithValue("@EventGUID", eventId);

            SQLWorker.ExecuteNonQuery(eventUpdateCommand);

            // Recreate the current event with the new data
            //this.CurrentEvent = new Event(eventName, eventDescription, startDate, endDate, Guid.Parse(eventId));

            this.Editing = false;
        }

        public void OnPostNewProject(string eventId, string userId)
        {
            this.SetValues(eventId, userId);

            Response.Redirect($"/ProjectEditor/{this.CurrentEvent.EventGUID}");
        }

        private void SetValues(string eventId, string userId)
        {
            // Assign the values provided to maintain data between posts
            this.CurrentEvent = new Event(Guid.Parse(eventId));
            this.LoggedInUser = new User(Guid.Parse(userId));

            // Query the database to get all of the projects associated with the current event
            SqlCommand projectQuery = new SqlCommand("select TaskGUID from Task where EventGUID = @EventGUID");
            projectQuery.Parameters.AddWithValue("@EventGUID", this.CurrentEvent.EventGUID);

            DataTable projectResults = SQLWorker.ExecuteQuery(projectQuery);

            // Assign the projects into the EventProjects
            for (int i = 0; i < projectResults.Rows.Count; i++)
            {
                this.EventProjects.Add(new Project((Guid)projectResults.Rows[i][0]));
            }

            // Query the database to get all of the individuals volunteering for the current event
            SqlCommand volunteerQuery = new SqlCommand(
                "select Individual.IndividualGUID, UserData.FirstName, UserData.LastName, UserData.UserGUID " +
                "from Individual " +
                "inner join UserData on Individual.UserGUID = UserData.UserGUID " +
                "where Individual.EventGUID = @EventGUID " +
                "and Individual.UserType = '02'");
            volunteerQuery.Parameters.AddWithValue("EventGUID", this.CurrentEvent.EventGUID);

            DataTable volunteerResults = SQLWorker.ExecuteQuery(volunteerQuery);

            for (int i = 0; i < volunteerResults.Rows.Count; i++)
            {
                Guid volunteerGUID = (Guid)volunteerResults.Rows[i][0];
                string volunteerName = $"{volunteerResults.Rows[i][1]} {volunteerResults.Rows[i][2]}";
                Guid userGuid = (Guid)volunteerResults.Rows[i][3];

                // If the volunteerGuid is a duplicate, don't insert it
                if (!this.EventVolunteers.Any(x => x.Item3 == userGuid))
                {
                    this.EventVolunteers.Add((volunteerGUID, volunteerName, userGuid));
                }
            }
        }
    }
}
