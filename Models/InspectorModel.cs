using SQLite;

namespace TrafficPolice
{
    public class InspectorModel
    {
        [PrimaryKey]
        public int Guid { get; set; }

        [Unique]
        public string Username { get; set; }

        [NotNull]
        public string Password { get; set; }
    }
}
