namespace VolunteerOrganizer.Library
{
    public class Task
    {
        #region Properties

        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public List<DateTime> DatesOccuring { get; set; }
        public List<Volunteer> AssignedVolunteers { get; set; }
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
            this.AssignedVolunteers = new List<Volunteer>();
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
        public Task(string name, string description, List<DateTime> datesOccuring, List<Volunteer> volunteers, Guid taskGuid, Guid eventGuid)
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
            // This constructor will eventually get the Task information from the SQL database once that's set up
            // This is just here as a placeholder to stop VS from yelling at me for an empty constructor
            this.TaskName = string.Empty;
            this.TaskDescription = String.Empty;
            this.DatesOccuring = new List<DateTime>();
            this.AssignedVolunteers = new List<Volunteer>();
            this.TaskGUID = new Guid();
            this.EventGUID = new Guid();
        }

        #endregion
    }
}
