using System;
using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.TourManager
{
    public class Pair : IEquatable<Pair>
    {
        [Required]
        public string Participant1 { get; set; }

        [Required]
        public string Participant2 { get; set; }

        public bool HasCrossing(Pair other)
        {
            return Participant1 == other.Participant1 || Participant1 == other.Participant2 ||
                   Participant2 == other.Participant1 || Participant2 == other.Participant2;
        }

        public bool Equals(Pair other)
        {
            return Participant1 == other.Participant1 && Participant2 == other.Participant2 ||
                   Participant1 == other.Participant2 && Participant2 == other.Participant1;
        }
    }
}
