using Microsoft.Data.SqlClient;
using System.Data;

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
        /// <param name="volunteerGuid"></param>
        public Volunteer(Guid volunteerGuid)
        {
            SqlCommand command = new SqlCommand("select top 1 * from Volunteer where VolunteerGUID = @VolunteerGUID");
            command.Parameters.AddWithValue("@VolunteerGUID", volunteerGuid);

            DataTable queryResult = SQLWorker.ExecuteQuery(command);

            // Parse the DataTable and assign values to object
            this.VolunteerGUID = (Guid)queryResult.Rows[0][0];
            this.EventGUID = (Guid)queryResult.Rows[0][1];
            this.VolunteerName = $"{(string)queryResult.Rows[0][2]} {(string)queryResult.Rows[0][3]}";
            this.VolunteerEmail = (string)queryResult.Rows[0][4];
            this.VolunteerPhoneNumber = (string)queryResult.Rows[0][5];
        }

        #endregion
    }
}
