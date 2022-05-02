using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VolunteerOrganizer.Pages
{
    public class UserAccountPageModel : PageModel
    {
        public Guid? userGuid { get; set; }
        public void OnGet()
        {
            // When accessing this page, a User GUID should be placed into the URL. If not, don't display anything
            if (Guid.TryParse(Request.Query["userGuid"], out Guid user))
            {
                this.userGuid = user;
            }

        }
    }
}
