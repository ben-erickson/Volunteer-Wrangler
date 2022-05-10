using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VolunteerOrganizer.Library;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Pages
{
    public class InstanceEditorModel : PageModel
    {
        public bool ModifyingInstance { get; set; }
        public bool NewInstance { get; set; }
        public ProjectInstance WorkingInstance { get; set; }
        public Project WorkingProject { get; set; }

        public InstanceEditorModel()
        {
            this.ModifyingInstance = false;
            this.NewInstance = false;
            this.WorkingInstance = new ProjectInstance();
            this.WorkingProject = new Project();
        }

        public void OnGet(string projectId, string? instanceId)
        {
            this.SetValues(projectId, instanceId);
        }

        public void OnPostModifyInstance(string projectId, string? instanceId)
        {
            this.SetValues(projectId, instanceId);

            this.ModifyingInstance = true;
        }

        public void OnPostSaveInstance(string projectId, string? instanceId)
        {
            this.SetValues(projectId, instanceId);
        }

        private void SetValues(string projectId, string? instanceId)
        {
            this.WorkingProject = new Project(Guid.Parse(projectId));

            // If the instanceId is null, we are creating a new one
            if (instanceId == null)
            {
                this.ModifyingInstance = true;
                this.NewInstance = true;
            }
            else
            {
                this.WorkingInstance = new ProjectInstance(Guid.Parse(projectId));
                this.ModifyingInstance = false;
                this.NewInstance = false;
            }
        }
    }
}
