using Microsoft.Data.SqlClient;
using System.Data;
using VolunteerOrganizer.Library.Enumerations;

namespace VolunteerOrganizer.Library
{
    public class Individual
    {
        #region Properties

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
            this.IndividualGUID = volunteerGuid;
            this.EventGUID = eventGuid;
        }

        /// <summary>
        /// Constructor to obtain Volunteer data from SQL database
        /// </summary>
        /// <param name="individualGUID"></param>
        public Individual(Guid individualGUID)
        {
            SqlCommand command = new SqlCommand("select top 1 IndividualGUID, EventGUID, UserGUID, UserType from Individual where IndividualGUID = @IndividualGUID");
            command.Parameters.AddWithValue("@VolunteerGUID", individualGUID);

            DataTable queryResult = SQLWorker.ExecuteQuery(command);

            // Parse the DataTable and assign values to object
            this.IndividualGUID = (Guid)queryResult.Rows[0][0];
            this.EventGUID = (Guid)queryResult.Rows[0][1];
            this.UserGUID = (Guid)queryResult.Rows[0][2];

            if (queryResult.Rows[0][3].ToString() == "01")
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
