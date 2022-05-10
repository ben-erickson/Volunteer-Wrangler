using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Library
{
    public class ProjectInstance
    {
        #region Parameters

        public Guid InstanceGUID { get; set; }
        public Guid ProjectGUID { get; set; }
        public DateTime InstanceTime { get; set; }
        public List<Individual?> AssignedVolunteers { get; set; }

        #endregion

        #region Constructors

        public ProjectInstance()
        {
            this.InstanceGUID = Guid.NewGuid();
            this.ProjectGUID = Guid.NewGuid();
            this.InstanceTime = default(DateTime);
            this.AssignedVolunteers = new List<Individual?>();
        }

        public ProjectInstance(Guid instanceGuid, Guid projectGuid, DateTime instanceTime, List<Individual?> assignedVolunteers)
        {
            this.InstanceGUID = instanceGuid;
            this.ProjectGUID = projectGuid;
            this.InstanceTime = instanceTime;
            this.AssignedVolunteers = assignedVolunteers;
        }

        public ProjectInstance(Guid instanceGuid)
        {
            // Query and assign instance values
            SqlCommand instanceQuery = new SqlCommand("select top 1 InstanceGUID, TaskGUID, StartDateTime from TaskInstance where InstanceGUID = @InstanceGUID");
            instanceQuery.Parameters.AddWithValue("@InstanceGUID", instanceGuid);

            DataTable instanceResult = SQLWorker.ExecuteQuery(instanceQuery);

            this.InstanceGUID = (Guid)instanceResult.Rows[0][0];
            this.ProjectGUID = (Guid)instanceResult.Rows[0][1];
            this.InstanceTime = (DateTime)instanceResult.Rows[0][2];

            // Query and assign assigned volunteers
            this.AssignedVolunteers = new List<Individual?>();

            SqlCommand volunteerQuery = new SqlCommand("select IndividualGuid from VolunteerAssignment where InstanceGUID = @InstanceGUID");
            volunteerQuery.Parameters.AddWithValue("@InstanceGUID", instanceGuid);

            DataTable volunteerResult = SQLWorker.ExecuteQuery(volunteerQuery);

            for (int i = 0; i < volunteerResult.Rows.Count; i++)
            {
                this.AssignedVolunteers.Add(new Individual((Guid)volunteerResult.Rows[i][0]));
            }
        }
        
        #endregion
    }
}
