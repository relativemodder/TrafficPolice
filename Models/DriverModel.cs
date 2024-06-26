﻿using SQLite;

namespace TrafficPolice
{
    public class DriverModel
    {
        [PrimaryKey]
        public int Guid { get; set; }

        [NotNull]
        public string Surname { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string MiddleName { get; set; }

        [NotNull]
        public string Passport { get; set; }

        [NotNull]
        public string RegistrationAddress { get; set; }

        [NotNull]
        public string LivingAddress { get; set; }

        public string? WorksAt { get; set; }

        public string? WorksAs { get; set; }

        [NotNull]
        public string PhoneNumber { get; set; }

        [NotNull]
        public string Email { get; set; }

        [NotNull]
        public int PhotoId { get; set; }

        public string? Notes { get; set; }

        public override string ToString()
        {
            return $"№{Guid} {Surname} {Name} {MiddleName}";
        }
    }
}
