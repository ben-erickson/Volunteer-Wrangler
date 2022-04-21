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
            SqlCommand constructorCommand = new SqlCommand("select top 1 * from Event where EventGUID = @EventGUID");
            constructorCommand.Parameters.AddWithValue("@EventGUID", eventGuid);

            DataTable queryResult = SQLWorker.ExecuteQuery(constructorCommand);

            // Parse the DataTable to get Event Values
            this.EventGUID = (Guid)queryResult.Rows[0][0];
            this.EventName = (string)queryResult.Rows[0][1];
            this.EventDescription = queryResult.Rows[0][2].ToString();
            this.StartDate = (DateTime)queryResult.Rows[0][3];
            this.EndDate = (DateTime)queryResult.Rows[0][4];
        }

        #endregion
    }
}
