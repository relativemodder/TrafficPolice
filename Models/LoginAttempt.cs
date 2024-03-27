using SQLite;

namespace TrafficPolice
{
    public class LoginAttempt
    {
        [PrimaryKey]
        public int Id { get; set; }
        public long Timestamp { get; set; }
    }
}
