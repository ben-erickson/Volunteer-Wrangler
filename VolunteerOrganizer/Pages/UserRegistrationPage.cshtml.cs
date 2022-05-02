using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using VolunteerOrganizer.Library;

namespace VolunteerOrganizer.Pages
{
    public class UserRegistrationPageModel : PageModel
    {
        public bool EmailError { get; set; }
        public bool PasswordError { get; set; }

        public void OnGet()
        {
            this.EmailError = false;
            this.PasswordError = false;
        }

        public void OnPost()
        {
            // TODO: Add a check that stops the user from registering with an email that is already used for an account
            string userEmail = Request.Form["userEmailText"];
            string unhashedPassword = Request.Form["userPasswordText"];
            string userType = Request.Form["userTypeDropdown"];

            string hashedPassword = HashManager.HashString(unhashedPassword);

            SqlCommand command = new SqlCommand("insert into UserRegistration values (newid(), @UserEmail, @HashedPassword, @UserType, null)");
            command.Parameters.AddWithValue("@UserEmail", userEmail);
            command.Parameters.AddWithValue("@HashedPassword", hashedPassword);
            command.Parameters.AddWithValue("@UserType", userType);

            SQLWorker.ExecuteNonQuery(command);

            // At this point, this should now redirect the user to the landing page for either the organizer or the volunteer
            // But as they don't exist yet, it will remain blank
        }
    }
}
