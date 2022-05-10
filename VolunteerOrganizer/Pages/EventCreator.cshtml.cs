using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VolunteerOrganizer.Library;
using Microsoft.Data.SqlClient;

namespace VolunteerOrganizer.Pages
{
    public class EventCreatorModel : PageModel
    {
        public User LoggedInUser { get; set; }

        public EventCreatorModel()
        {
            this.LoggedInUser = new User();
        }

        public void OnGet(string userId)
        {
            this.LoggedInUser = new User(Guid.Parse(userId));
        }

        public void OnPost(string userId)
        {
            // Reassign LoggedInUser, since the object returns to null when a Post request is sent
            this.LoggedInUser = new User(Guid.Parse(userId));

            string eventName = Request.Form["eventNameText"];
            string eventDescription = Request.Form["eventDescriptionText"];
            DateTime eventStartDate = DateTime.Parse(Request.Form["eventStartDate"]);
            DateTime eventEndDate = DateTime.Parse(Request.Form["eventEndDate"]);
            Guid eventGuid = Guid.NewGuid();

            // Make the insertion into Event
            SqlCommand eventInsertion = new SqlCommand("insert into Event values (@EventGUID, @EventName, @EventDescription, @StartDate, @EndDate)");
            eventInsertion.Parameters.AddWithValue("@EventGUID", eventGuid);
            eventInsertion.Parameters.AddWithValue("EventName", eventName);
            eventInsertion.Parameters.AddWithValue("@EventDescription", eventDescription);
            eventInsertion.Parameters.AddWithValue("@StartDate", eventStartDate);
            eventInsertion.Parameters.AddWithValue("@EndDate", eventEndDate);

            SQLWorker.ExecuteNonQuery(eventInsertion);

            // Insert into Individual
            Guid individualGuid = Guid.NewGuid();

            SqlCommand individualInsertion = new SqlCommand("insert into Individual values (@IndividualGUID, @EventGUID, @UserGUID, @UserType)");
            individualInsertion.Parameters.AddWithValue("@IndividualGUID", individualGuid);
            individualInsertion.Parameters.AddWithValue("@EventGUID", eventGuid);
            individualInsertion.Parameters.AddWithValue("@UserGUID", this.LoggedInUser.UserGuid);
            individualInsertion.Parameters.AddWithValue("@UserType", "01");

            SQLWorker.ExecuteNonQuery(individualInsertion);

            // Insert into EventSearch
            Guid searchKey = Guid.NewGuid();

            SqlCommand searchCommand = new SqlCommand("insert into EventSearch (EventSearchGUID, EventSearchKey, EventGUID) values (newid(), @EventSearchKey, @EventGUID)");
            searchCommand.Parameters.AddWithValue("@EventSearchKey", searchKey);
            searchCommand.Parameters.AddWithValue("@EventGUID", eventGuid);

            SQLWorker.ExecuteNonQuery(searchCommand);

            // Send the user to the Event Editor for their new event
            Response.Redirect($"/EventEditor/{eventGuid}/{this.LoggedInUser.UserGuid}");
        }
    }
}
