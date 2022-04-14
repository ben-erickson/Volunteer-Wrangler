namespace VolunteerOrganizer.Library
{
    public class Volunteer
    {
        #region Properties

        public string VolunteerName { get; set; }
        public string VolunteerEmail { get; set; }
        public string VolunteerPhoneNumber { get; set; }
        public Guid VolunteerGUID { get; set; }
        public Guid EventGUID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Volunteer()
        {
            this.VolunteerName = string.Empty;
            this.VolunteerEmail = string.Empty;
            this.VolunteerPhoneNumber = string.Empty;
            this.VolunteerGUID = new Guid();
            this.EventGUID = new Guid();
        }

        /// <summary>
        /// Constructor for a Volunteer where all data is already present
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="guid"></param>
        public Volunteer(string name, string email, string phoneNumber, Guid volunteerGuid, Guid eventGuid)
        {
            this.VolunteerName = name;
            this.VolunteerEmail = email;
            this.VolunteerPhoneNumber = phoneNumber;
            this.VolunteerGUID = volunteerGuid;
            this.EventGUID = eventGuid;
        }

        /// <summary>
        /// Constructor to obtain Volunteer data from SQL database
        /// </summary>
        /// <param name="guid"></param>
        public Volunteer(Guid guid)
        {
            // Once the SQL database is established, this will make a query to obtain Volunteer data from database
            // This is just here as a placeholder to stop VS from yelling at me for an empty constructor
            this.VolunteerName = string.Empty;
            this.VolunteerEmail = string.Empty;
            this.VolunteerPhoneNumber = string.Empty;
            this.VolunteerGUID = new Guid();
            this.EventGUID = new Guid();
        }

        #endregion
    }
}
