using Microsoft.Data.SqlClient;
using System.Data;
using VolunteerOrganizer.Library.Enumerations;

namespace VolunteerOrganizer.Library
{
    public class User
    {
        #region Properties

        // This class does not have a property for the password, as there isn't a need for it on the front end

        public Guid UserGuid { get; set; }
        public String UserEmail { get; set; }
        public UserType UserType { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
            this.UserGuid = new Guid();
            this.UserEmail = String.Empty;
            this.UserType = UserType.Volunteer;
        }

        /// <summary>
        /// Constructor for User with already defined data
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userEmail"></param>
        /// <param name="userType"></param>
        public User(Guid userGuid, string userEmail, UserType userType)
        {
            this.UserGuid = userGuid;
            this.UserEmail = userEmail;
            this.UserType = userType;
        }

        public User(Guid userGuid)
        {
            SqlCommand commnad = new SqlCommand("select top 1 UserGUID, UserEmail, UserType from UserData where UserGUID = @UserGUID");
            commnad.Parameters.AddWithValue("@UserGUID", userGuid);

            DataTable queryResult = SQLWorker.ExecuteQuery(commnad);

            this.UserGuid = (Guid)queryResult.Rows[0][0];
            this.UserEmail = (string)queryResult.Rows[0][1];

            if ((string)queryResult.Rows[0][2] == "01")
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
