using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Library
{
    public class Project
    {
        #region Properties

        public string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public List<ProjectInstance> ProjectInstances { get; set; }
        public Guid ProjectGUID { get; set; }
        public Guid EventGUID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Project()
        {
            this.ProjectName = string.Empty;
            this.ProjectDescription = String.Empty;
            this.ProjectInstances = new List<ProjectInstance>();
            this.ProjectGUID = Guid.NewGuid();
            this.EventGUID = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor for when data is already present upon object creation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="datesOccuring"></param>
        /// <param name="volunteers"></param>
        /// <param name="taskGuid"></param>
        /// <param name="eventGuid"></param>
        public Project(string name, string description, List<ProjectInstance> projectInstances, Guid taskGuid, Guid eventGuid)
        {
            this.ProjectName = name;
            this.ProjectDescription = description;
            this.ProjectInstances = projectInstances;
            this.ProjectGUID = taskGuid;
            this.EventGUID = eventGuid;
        }

        /// <summary>
        /// Constructor to obtain Task data from SQL database
        /// </summary>
        /// <param name="projectGuid"></param>
        public Project(Guid projectGuid)
        {
            SqlCommand projectQuery = new SqlCommand(
                "select top 1 " +
                    "Task.TaskGUID, " +
                    "Task.EventGUID, " +
                    "Task.TaskName, " +
                    "Task.TaskDescription " +
                "from Task " +
                "where TaskGUID = @TaskGUID");
            projectQuery.Parameters.AddWithValue("@TaskGUID", projectGuid);

            DataTable projectResult = SQLWorker.ExecuteQuery(projectQuery);

            // Parse DataTable and assign values to object
            this.ProjectGUID = (Guid)projectResult.Rows[0][0];
            this.EventGUID = (Guid)projectResult.Rows[0][1];
            this.ProjectName = (string)projectResult.Rows[0][2];
            this.ProjectDescription = projectResult.Rows[0][3].ToString();

            // Get all instances associated to this project
            this.ProjectInstances = new List<ProjectInstance>();

            SqlCommand instanceQuery = new SqlCommand("select InstanceGUID from TaskInstance where TaskGUID = @TaskGUID");
            instanceQuery.Parameters.AddWithValue("@TaskGUID", projectGuid);

            DataTable instanceResult = SQLWorker.ExecuteQuery(instanceQuery);

            for (int i = 0; i < instanceResult.Rows.Count; i++)
            {
                this.ProjectInstances.Add(new ProjectInstance((Guid)instanceResult.Rows[i][0]));
            }
        }

        #endregion
    }
}
