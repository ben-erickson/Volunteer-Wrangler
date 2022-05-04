using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using VolunteerOrganizer.Library;

namespace VolunteerOrganizer.Pages
{
    public class UserRegistrationPageModel : PageModel
    {
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            this.Error = false;
            this.ErrorMessage = string.Empty;
        }

        public void OnPost()
        {
            string userEmail = Request.Form["userEmailText"];
            string unhashedPassword = Request.Form["userPasswordText"];
            string userType = Request.Form["userTypeDropdown"];
         
            // Check if there is an account associated to the user's email
            SqlCommand emailCheckCommand = new SqlCommand("select top 1 * from UserData where UserEmail = @UserEmail");
            emailCheckCommand.Parameters.AddWithValue("@UserEmail", userEmail);

            DataTable emailCheckResult = SQLWorker.ExecuteQuery(emailCheckCommand);

            if (emailCheckResult.Rows.Count == 0)
            {
                // There are no entries in UserData with the given email, so insert it into the table and continue
                string hashedPassword = HashManager.HashString(unhashedPassword);
                Guid newUserGuid = Guid.NewGuid();

                SqlCommand command = new SqlCommand("insert into UserData values (@UserGUID, @UserEmail, @HashedPassword)");
                command.Parameters.AddWithValue("@UserGUID", newUserGuid);
                command.Parameters.AddWithValue("@UserEmail", userEmail);
                command.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                command.Parameters.AddWithValue("@UserType", userType);

                SQLWorker.ExecuteNonQuery(command);

                Response.Redirect($"/LoggedInPage/{newUserGuid}");
            }
            else
            {
                // There is already an account with this email associated, so don't add another one
                this.Error = true;
                this.ErrorMessage = "There is already an account associated with this email.";
            }
        }
    }
}
