using SQLite;

namespace TrafficPolice
{
    public class InspectorModel
    {
        [PrimaryKey]
        public string Username { get; set; }

        [NotNull]
        public string Password { get; set; }
    }
}
