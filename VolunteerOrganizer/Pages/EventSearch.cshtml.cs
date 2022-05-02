using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using VolunteerOrganizer.Library;

namespace VolunteerOrganizer.Pages
{
    public class EventSearchModel : PageModel
    {
        public bool SearchExecuted { get; set; }
        public bool DataObtained { get; set; }
        public DataTable queryData { get; set; }

        public void OnGet()
        {
            DataObtained = false;
        }
        public void OnPost()
        {
            string searchKey = Request.Form["SearchKey"];

            SqlCommand command = new SqlCommand(
                "select EventName, EventDescription " +
                "from Event " +
                "where EventGUID in " +
                "( " +
                    "select EventGUID " +
                    "from EventSearch " +
                    "where EventSearchKey = @searchKey " +
                ")");
            command.Parameters.AddWithValue("@searchKey", searchKey);
            try
            {
                this.queryData = SQLWorker.ExecuteQuery(command);
                this.SearchExecuted = true;

                if (this.queryData.Rows.Count == 0)
                {
                    this.DataObtained = false;
                }
                else
                {
                    this.DataObtained = true;
                }
            }
            catch
            {
                this.SearchExecuted = false;
            }
        }
    }
}
