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
            this.IndividualGUID = Guid.NewGuid();
            this.EventGUID = Guid.NewGuid();
            this.UserGUID = Guid.NewGuid();
            this.UserType = UserType.Volunteer;
        }

        /// <summary>
        /// Constructor for a Volunteer where all data is already present
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="guid"></param>
        public Individual(Guid volunteerGuid, Guid eventGuid, Guid userGuid, UserType userType)
        {
            this.IndividualGUID = volunteerGuid;
            this.EventGUID = eventGuid;
            this.UserGUID = userGuid;
            this.UserType = userType;
            
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

        public string GetName()
        {
            string individualName = "";

            // Query the database to get the individual's name
            SqlCommand nameQuery = new SqlCommand("select FirstName + ' ' + LastName as UserName from UserData where UserGUID = @UserGUID");
            nameQuery.Parameters.AddWithValue("@UserGUID", this.UserGUID);

            DataTable nameResult = SQLWorker.ExecuteQuery(nameQuery);

            individualName = (string)nameResult.Rows[0][0];

            return individualName;
        }
    }
}
