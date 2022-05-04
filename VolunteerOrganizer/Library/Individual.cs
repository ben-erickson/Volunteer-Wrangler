using Microsoft.Data.SqlClient;
using System.Data;
using VolunteerOrganizer.Library.Enumerations;

namespace VolunteerOrganizer.Library
{
    public class Individual
    {
        #region Properties

        public string IndividualName { get; set; }
        public string IndividualEmail { get; set; }
        public string IndividualPhoneNumber { get; set; }
        public Guid IndividualGUID { get; set; }
        public Guid EventGUID { get; set; }
        public Guid UserGUID { get; set; }
        public UserType UserType { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Individual()
        {
            this.IndividualName = string.Empty;
            this.IndividualEmail = string.Empty;
            this.IndividualPhoneNumber = string.Empty;
            this.IndividualGUID = new Guid();
            this.EventGUID = new Guid();
        }

        /// <summary>
        /// Constructor for a Volunteer where all data is already present
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="guid"></param>
        public Individual(string name, string email, string phoneNumber, Guid volunteerGuid, Guid eventGuid)
        {
            this.IndividualName = name;
            this.IndividualEmail = email;
            this.IndividualPhoneNumber = phoneNumber;
            this.IndividualGUID = volunteerGuid;
            this.EventGUID = eventGuid;
        }

        /// <summary>
        /// Constructor to obtain Volunteer data from SQL database
        /// </summary>
        /// <param name="volunteerGuid"></param>
        public Individual(Guid volunteerGuid)
        {
            SqlCommand command = new SqlCommand("select top 1 IndividualGUID, EventGUID, FirstName, LastName, Email, PhoneNumber, UserGUID, UserType from Volunteer where VolunteerGUID = @VolunteerGUID");
            command.Parameters.AddWithValue("@VolunteerGUID", volunteerGuid);

            DataTable queryResult = SQLWorker.ExecuteQuery(command);

            // Parse the DataTable and assign values to object
            this.IndividualGUID = (Guid)queryResult.Rows[0][0];
            this.EventGUID = (Guid)queryResult.Rows[0][1];
            this.IndividualName = $"{(string)queryResult.Rows[0][2]} {(string)queryResult.Rows[0][3]}";
            this.IndividualEmail = (string)queryResult.Rows[0][4];
            this.IndividualPhoneNumber = (string)queryResult.Rows[0][5];
            this.UserGUID = (Guid)queryResult.Rows[0][6];

            if (queryResult.Rows[0][7].ToString() == "01")
            {
                this.UserType = UserType.Organizer;
            }
            else
            {
                this.UserType = UserType.Volunteer;
            }
        }

        #endregion
    }
}
