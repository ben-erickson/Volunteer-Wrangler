using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VolunteerOrganizer.Library;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Pages
{
    public class VolunteerViewModel : PageModel
    {
        public User VolunteerUser { get; set; }
        public Event AssociatedEvent { get; set; }

        // ProjectName, ProjectDescription, ProjectTime
        public List<(string, string?, DateTime)> VolunteerProjects { get; set; }

        public VolunteerViewModel()
        {
            this.VolunteerUser = new User();
            this.AssociatedEvent = new Event();
            this.VolunteerProjects = new List<(string, string?, DateTime)>();
        }

        public void OnGet(string eventId, string individualId)
        {
            this.SetValues(eventId, individualId);
        }

        private void SetValues(string eventId, string individualId)
        {
            this.AssociatedEvent = new Event(Guid.Parse(eventId));
            this.VolunteerUser = new User(Guid.Parse(individualId));

            // Get the project data for the current volunteer

            // Get all of the instances that this volunteer is assigned to
            SqlCommand instanceCommand = new SqlCommand(
                "select VolunteerAssignment.InstanceGUID " +
                "from VolunteerAssignment " +
                "inner join Individual on VolunteerAssignment.IndividualGUID = Individual.IndividualGUID " +
                "where Individual.UserGUID = @UserGUID");
            instanceCommand.Parameters.AddWithValue("@UserGUID", individualId);

            DataTable instanceResults = SQLWorker.ExecuteQuery(instanceCommand);

            // Get the project data for each instance
            for (int i = 0; i < instanceResults.Rows.Count; i++)
            {
                SqlCommand assignmentCommand = new SqlCommand(
                    "select Task.TaskName, Task.TaskDescription, TaskInstance.StartDateTime " +
                    "from Task " +
                    "inner join TaskInstance on Task.TaskGUID = TaskInstance.TaskGUID " +
                    "where InstanceGUID = @InstanceGUID");
                assignmentCommand.Parameters.AddWithValue("@InstanceGUID", instanceResults.Rows[i][0]);

                DataTable assignmentResults = SQLWorker.ExecuteQuery(assignmentCommand);

                string projectName = (string)assignmentResults.Rows[0][0];
                string? projectDescription = (string?)assignmentResults.Rows[0][1];
                DateTime projectTime = (DateTime)assignmentResults.Rows[0][2];

                this.VolunteerProjects.Add((projectName, projectDescription, projectTime));
            }
        }
    }
}
