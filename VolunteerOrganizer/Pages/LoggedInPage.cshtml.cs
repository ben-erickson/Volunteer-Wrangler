using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VolunteerOrganizer.Library;
using VolunteerOrganizer.Library.Enumerations;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Pages
{
    public class LoggedInPageModel : PageModel
    {
        public User LoggedInUser { get; set; }
        public List<(string?, Guid)> OrganizerEvents { get; set; }
        public List<(string?, Guid)> VolunteerEvents { get; set; }
        public bool EventKeyError { get; set; }

        public LoggedInPageModel()
        {
            this.LoggedInUser = new User();
            this.OrganizerEvents = new List<(string?, Guid)>();
            this.VolunteerEvents = new List<(string?, Guid)>();
            this.EventKeyError = false;
        }

        public void OnGet(string userId)
        {
            this.SetValues(userId);
        }

        public void OnPostAddVolunteer (string userId)
        {
            this.SetValues(userId);

            string eventKey = Request.Form["searchKeyText"];

            // If the key corresponds to an event, assign the volunteer to it
            SqlCommand keyCommand = new SqlCommand("select top 1 EventGUID from EventSearch where EventSearchKey = @EventKey");
            keyCommand.Parameters.AddWithValue("@EventKey", eventKey);

            DataTable keyResults = SQLWorker.ExecuteQuery(keyCommand);

            if (keyResults.Rows.Count == 0)
            {
                this.EventKeyError = true;
            }
            else
            {
                // In case the user had an error before, reset the error bool
                this.EventKeyError = false;

                // Insert a volunteer individual into individual
                SqlCommand individualCommand = new SqlCommand(
                    "insert into Individual(IndividualGUID, EventGUID, UserGUID, UserType) " +
                    "values(newid(), @EventGUID, @UserGUID, '02')");
                individualCommand.Parameters.AddWithValue("@EventGUID", keyResults.Rows[0][0]);
                individualCommand.Parameters.AddWithValue("@UserGUID", this.LoggedInUser.UserGuid);

                SQLWorker.ExecuteNonQuery(individualCommand);
            }
        }

        private void SetValues (string userId)
        {
            this.LoggedInUser = new User(Guid.Parse(userId));

            // Get the event names and GUIDs that are associated with the current user as well as the user's association
            // to those events
            SqlCommand commnad = new SqlCommand(
                "select Event.EventName, Event.EventGUID, Individual.UserType " +
                "from Event " +
                "inner join Individual on Event.EventGUID = Individual.EventGUID " +
                "where Individual.UserGUID = @UserGUID");
            commnad.Parameters.AddWithValue("@UserGUID", userId);

            DataTable queryResult = SQLWorker.ExecuteQuery(commnad);

            for (int i = 0; i < queryResult.Rows.Count; i++)
            {
                if (queryResult.Rows[i][2].ToString() == "01")
                {
                    // Add the event to the organizer list
                    this.OrganizerEvents.Add((queryResult.Rows[i][0].ToString(), (Guid)queryResult.Rows[i][1]));
                }
                else if (queryResult.Rows[i][2].ToString() == "02")
                {
                    // Add the event to the volunteer list
                    this.VolunteerEvents.Add((queryResult.Rows[i][0].ToString(), (Guid)queryResult.Rows[i][1]));
                }
            }
        }
    }
}
