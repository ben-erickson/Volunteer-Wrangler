using Microsoft.Data.SqlClient;
using System.Data;

namespace VolunteerOrganizer.Library
{
    public class Event
    {
        #region Properties

        public string EventName { get; set; }
        public string? EventDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid EventGUID { get; set; }
        public string SearchKey { get; set; }

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
            this.EventGUID = Guid.NewGuid();
            this.SearchKey = string.Empty;
        }

        /// <summary>
        /// Constructor for when all data is already present at time of object construction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="eventGuid"></param>
        public Event(string name, string description, DateTime startDate, DateTime endDate, Guid eventGuid, string searchKey)
        {
            this.EventName = name;
            this.EventDescription = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.EventGUID = eventGuid;
            this.SearchKey = searchKey;
        }

        /// <summary>
        /// Constructor to obtain Event data from SQL database
        /// </summary>
        /// <param name="eventGuid"></param>
        public Event(Guid eventGuid)
        {
            SqlCommand constructorCommand = new SqlCommand(
                "select top 1 EventGUID, EventName, EventDescription, StartDate, EndDate " +
                "from Event " +
                "where EventGUID = @EventGUID");
            constructorCommand.Parameters.AddWithValue("@EventGUID", eventGuid);

            DataTable queryResult = SQLWorker.ExecuteQuery(constructorCommand);

            // Parse the DataTable to get Event Values
            this.EventGUID = (Guid)queryResult.Rows[0][0];
            this.EventName = (string)queryResult.Rows[0][1];
            this.EventDescription = queryResult.Rows[0][2].ToString();
            this.StartDate = (DateTime)queryResult.Rows[0][3];
            this.EndDate = (DateTime)queryResult.Rows[0][4];

            // Get the search key associated with this event
            SqlCommand searchKeyCommand = new SqlCommand("select EventSearchKey from EventSearch where EventGUID = @EventGUID");
            searchKeyCommand.Parameters.AddWithValue("@EventGUID", eventGuid);

            DataTable searchKeyResult = SQLWorker.ExecuteQuery(searchKeyCommand);

            this.SearchKey = String.Empty;
            if (searchKeyResult.Rows.Count > 0)
            {
                this.SearchKey = (string)searchKeyResult.Rows[0][0];
            }
        }

        #endregion

        public List<(string, Guid)> GetAssignedVolunteers()
        {
            // Get all of the volunteers signed up for this event
            List<(string, Guid)> assignedVolunteers = new System.Collections.Generic.List<(string, Guid)>();

            SqlCommand volunteerQuery = new SqlCommand(
                "select " +
                    "isnull(UserData.FirstName, '') + ' ' + isnull(UserData.LastName, '') as UserName, " +
                    "UserData.UserGuid " +
                "from UserData " +
                "inner join Individual on UserData.UserGUID = Individual.UserGUID " +
                "where Individual.EventGUID = @EventGUID " +
                "and Individual.UserType = '02'");
            volunteerQuery.Parameters.AddWithValue("@EventGUID", this.EventGUID);

            DataTable voluteerResult = SQLWorker.ExecuteQuery(volunteerQuery);

            for (int i = 0; i < voluteerResult.Rows.Count; i++)
            {
                assignedVolunteers.Add(((string)voluteerResult.Rows[i][0], (Guid)voluteerResult.Rows[i][1]));
            }

            return assignedVolunteers;
        }
    }
}
