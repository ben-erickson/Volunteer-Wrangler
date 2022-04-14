namespace VolunteerOrganizer.Library
{
    public class Event
    {
        #region Properties

        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid EventGUID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Event()
        {
            this.EventName = string.Empty;
            this.EventDescription = string.Empty;
            this.StartDate = default(DateTime);
            this.EndDate = default(DateTime);
            this.EventGUID = new Guid();
        }

        /// <summary>
        /// Constructor for when all data is already present at time of object construction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="eventGuid"></param>
        public Event(string name, string description, DateTime startDate, DateTime endDate, Guid eventGuid)
        {
            this.EventName = name;
            this.EventDescription = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.EventGUID = eventGuid;
        }

        /// <summary>
        /// Constructor to obtain Event data from SQL database
        /// </summary>
        /// <param name="eventGuid"></param>
        public Event(Guid eventGuid)
        {
            // Once the SQL database is set up, this will obtain the object data from there
            // This is just here as a placeholder to stop VS from yelling at me for an empty constructor
            this.EventName = string.Empty;
            this.EventDescription = string.Empty;
            this.StartDate = default(DateTime);
            this.EndDate = default(DateTime);
            this.EventGUID = new Guid();
        }

        #endregion
    }
}
