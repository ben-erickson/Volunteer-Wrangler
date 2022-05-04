using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Library
{
    public class Task
    {
        #region Properties

        public string TaskName { get; set; }
        public string? TaskDescription { get; set; }
        public List<DateTime> DatesOccuring { get; set; }
        public List<Individual> AssignedVolunteers { get; set; }
        public Guid TaskGUID { get; set; }
        public Guid EventGUID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Task()
        {
            this.TaskName = string.Empty;
            this.TaskDescription = String.Empty;
            this.DatesOccuring = new List<DateTime>();
            this.AssignedVolunteers = new List<Individual>();
            this.TaskGUID = new Guid();
            this.EventGUID = new Guid();
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
        public Task(string name, string description, List<DateTime> datesOccuring, List<Individual> volunteers, Guid taskGuid, Guid eventGuid)
        {
            this.TaskName = name;
            this.TaskDescription = description;
            this.DatesOccuring = datesOccuring;
            this.AssignedVolunteers = volunteers;
            this.TaskGUID = taskGuid;
            this.EventGUID = eventGuid;
        }

        /// <summary>
        /// Constructor to obtain Task data from SQL database
        /// </summary>
        /// <param name="taskGuid"></param>
        public Task(Guid taskGuid)
        {
            SqlCommand command = new SqlCommand("select top 1 * from Task where TaskGUID = @TaskGUID");
            command.Parameters.AddWithValue("@TaskGUID", taskGuid);

            DataTable queryResult = SQLWorker.ExecuteQuery(command);

            // Parse DataTable and assign values to object
            this.TaskGUID = (Guid)queryResult.Rows[0][0];
            this.EventGUID = (Guid)queryResult.Rows[0][1];
            this.TaskName = (string)queryResult.Rows[0][2];
            this.TaskDescription = queryResult.Rows[0][3].ToString();

            this.AssignedVolunteers = new List<Individual>();
            this.DatesOccuring = new List<DateTime>();
        }

        #endregion
    }
}
