using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPolice
{
    public class PhotoModel
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public byte[] BinaryContent { get; set; }

        [NotNull]
        public string MimeType { get; set; }
    }
}
