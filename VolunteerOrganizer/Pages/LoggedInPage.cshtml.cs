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
        public List<(string, Guid, UserType)> AssociatedEvents { get; set; }

        public LoggedInPageModel()
        {
            this.LoggedInUser = new User();
            this.AssociatedEvents = new List<(string, Guid, UserType)>();
        }

        public void OnGet(string userId)
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
                UserType relationship;
                if (queryResult.Rows[i][2].ToString() == "01")
                {
                    relationship = UserType.Organizer;
                }
                else
                {
                    relationship = UserType.Volunteer;
                }

                this.AssociatedEvents.Add(((string)queryResult.Rows[i][0], (Guid)queryResult.Rows[i][1], relationship));
            }
        }
    }
}
