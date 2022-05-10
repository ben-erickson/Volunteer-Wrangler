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
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PhoneNumber { get; set;
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
            this.UserGuid = Guid.NewGuid();
            this.UserEmail = String.Empty;
            this.FirstName = String.Empty;
            this.LastName = String.Empty;
            this.PhoneNumber = String.Empty;
        }

        /// <summary>
        /// Constructor for User with already defined data
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userEmail"></param>
        /// <param name="userType"></param>
        public User(Guid userGuid, string userEmail, string firstName, string lastName, string phoneNumber)
        {
            this.UserGuid = userGuid;
            this.UserEmail = userEmail;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
        }

        public User(Guid userGuid)
        {
            SqlCommand commnad = new SqlCommand("select top 1 UserGUID, UserEmail, FirstName, LastName, PhoneNumber from UserData where UserGUID = @UserGUID");
            commnad.Parameters.AddWithValue("@UserGUID", userGuid);

            DataTable queryResult = SQLWorker.ExecuteQuery(commnad);

            this.UserGuid = (Guid)queryResult.Rows[0][0];
            this.UserEmail = (string)queryResult.Rows[0][1];
            this.FirstName = queryResult.Rows[0][2].ToString() ?? string.Empty;
            this.LastName = queryResult.Rows[0][3].ToString() ?? string.Empty;
            this.PhoneNumber = queryResult.Rows[0][4].ToString() ?? string.Empty;
        }

        #endregion
    }
}
