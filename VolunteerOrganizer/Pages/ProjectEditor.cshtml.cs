using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VolunteerOrganizer.Library;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Pages
{
    public class ProjectEditorModel : PageModel
    {
        public bool ModifyingProject { get; set; }
        public bool NewProject { get; set; }
        public bool AddingInstance { get; set; }
        public Project EditingProject { get; set; }
        public Event AssociatedEvent { get; set; }

        public ProjectEditorModel()
        {
            this.ModifyingProject = false;
            this.NewProject = false;
            this.AddingInstance = false;
            this.EditingProject = new Project();
            this.AssociatedEvent = new Event();
        }

        public void OnGet(string eventId, string? projectId)
        {
            this.SetValues(eventId, projectId);
        }

        public void OnPostEditProject(string eventId, string? projectId)
        {
            this.SetValues(eventId, projectId);

            // Set editing to true to edit the current project
            this.ModifyingProject = true;
        }

        public void OnPostUpdateProject(string eventId, string? projectId)
        {
            this.SetValues(eventId, projectId);

            // Capture the values of the old project
            Guid projectGuid = this.EditingProject.ProjectGUID;
            Guid eventGuid = this.AssociatedEvent.EventGUID;
            List<ProjectInstance> projectInstances = this.EditingProject.ProjectInstances;
            string projectName = Request.Form["projectNameText"];
            string projectDescription = Request.Form["projectDescriptionText"];

            // If it is a new project, insert
            if (this.NewProject)
            {
                SqlCommand projectInsert = new SqlCommand(
                    "insert into Task (TaskGUID, EventGUID, TaskName, TaskDescription) " +
                    "values(@TaskGUID, @EventGUID, @TaskName, @TaskDescription)");
                projectInsert.Parameters.AddWithValue("@TaskGUID", projectGuid);
                projectInsert.Parameters.AddWithValue("@EventGUID", eventGuid);
                projectInsert.Parameters.AddWithValue("@TaskName", projectName);
                projectInsert.Parameters.AddWithValue("@TaskDescription", projectDescription);

                SQLWorker.ExecuteNonQuery(projectInsert);

                // Reload the project editor with the new project loaded
                Response.Redirect($"/ProjectEditor/{eventGuid}/{projectGuid}/");
            }
            // If it is not a new project, update
            else
            {
                SqlCommand projectUpdate = new SqlCommand(
                    "update Task " +
                    "set TaskName = @TaskName, TaskDescription = @TaskDescription " +
                    "where TaskGUID = @TaskGUID");
                projectUpdate.Parameters.AddWithValue("@TaskName", projectName);
                projectUpdate.Parameters.AddWithValue("@TaskDescription", projectDescription);
                projectUpdate.Parameters.AddWithValue("@TaskGUID", projectGuid);

                SQLWorker.ExecuteNonQuery(projectUpdate);
            }

            // Update the current project to the updated values
            this.EditingProject = new Project(projectName, projectDescription, projectInstances, projectGuid, eventGuid);
            this.ModifyingProject = false;
        }

        public void OnPostReturnButton (string eventId, string? projectId)
        {
            this.SetValues(eventId, projectId);

            // Get the organizer's UserGUID, then send the user back to the Event Editor
            // This is a scuffed method, but since the only way to get here is by being the organizer
            // of the current event, it will have to do for now. I'll return with a better strat if there
            // is time later

            Guid eventGuid = this.AssociatedEvent.EventGUID;

            // Get the UserGuid associated to the organizer of the event
            SqlCommand userQuery = new SqlCommand("select top 1 UserGUID from Individual where EventGUID = @EventGUID");
            userQuery.Parameters.AddWithValue("@EventGUID", eventGuid);

            DataTable userResult = SQLWorker.ExecuteQuery(userQuery);

            Guid userGuid = (Guid)userResult.Rows[0][0];

            Response.Redirect($"/EventEditor/{eventGuid}/{userGuid}");
        }

        public void OnPostNewInstance (string eventId, string? projectId)
        {
            this.SetValues(eventId, projectId);
            this.AddingInstance = true;
        }

        public void OnPostAddInstance (string eventId, string? projectId)
        {
            this.SetValues(eventId, projectId);

            DateTime instanceTime = DateTime.Parse(Request.Form["newInstanceTime"]);
            string[] instanceVolunteers = Request.Form["volunteerCheckbox"];
            Guid instanceGuid = Guid.NewGuid();

            // Make the insertion into ProjectInstance
            SqlCommand instanceCommand = new SqlCommand(
                "insert into TaskInstance " +
                "(InstanceGUID, TaskGUID, StartDateTime) " +
                "values(@InstanceGUID, @TaskGUID, @InstanceTime)");
            instanceCommand.Parameters.AddWithValue("@InstanceGUID", instanceGuid);
            instanceCommand.Parameters.AddWithValue("@TaskGUID", this.EditingProject.ProjectGUID);
            instanceCommand.Parameters.AddWithValue("@InstanceTime", instanceTime);

            SQLWorker.ExecuteNonQuery(instanceCommand);

            for (int i = 0; i < instanceVolunteers.Length; i++)
            {
                SqlCommand volunteerCommand = new SqlCommand(
                    "insert into VolunteerAssignment " +
                    "(AssignmentGUID, InstanceGUID, IndividualGUID) " +
                    "values(newid(), @InstanceGUID, @IndividualGUID)");
                volunteerCommand.Parameters.AddWithValue("@InstanceGUID", instanceGuid);
                volunteerCommand.Parameters.AddWithValue("@IndividualGUID", Guid.Parse(instanceVolunteers[i]));

                SQLWorker.ExecuteNonQuery(volunteerCommand);
            }

            this.AddingInstance = false;
        }

        private void SetValues (string eventId, string? projectId)
        {
            this.AssociatedEvent = new Event(Guid.Parse(eventId));

            // If projectId is not null, assign the values
            if (projectId == null)
            {
                this.ModifyingProject = true;
                this.NewProject = true;
            }
            else
            {
                this.EditingProject = new Project(Guid.Parse(projectId));
                this.ModifyingProject = false;
                this.NewProject = false;
            }
        }
    }
}
