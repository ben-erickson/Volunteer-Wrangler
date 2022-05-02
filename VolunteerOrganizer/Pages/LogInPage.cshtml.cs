using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VolunteerOrganizer.Library;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Pages
{
    public class LogInPageModel : PageModel
    {
        public bool LogInError { get; set; }
        public void OnGet()
        {
            this.LogInError = false;
        }

        public void OnPost()
        {
            string userEmail = Request.Form["userEmailText"];
            string unhashedPassword = Request.Form["userEmailPassword"];

            // Hash the given password from the request
            string userPassword = HashManager.HashString(unhashedPassword);

            SqlCommand command = new SqlCommand("select UserGuid, UserType from UserData where UserEmail = @UserEmail and UserPassword = @UserPassword");
            command.Parameters.AddWithValue("@UserEmail", userEmail);
            command.Parameters.AddWithValue("@UserPassword", userPassword);

            DataTable queryResult = SQLWorker.ExecuteQuery(command);

            // If there is exactly one result, that means that the user input correct data, and we can go to the landing page
            if (queryResult.Rows.Count == 1)
            {

            }
            else
            {
                // NOTE: this will also be run if there are two users with the same email and password. This is a minor issue,
                // but I should disallow the same email to be used twice in the registration page.
                this.LogInError = true;
            }
        }
    }
}
