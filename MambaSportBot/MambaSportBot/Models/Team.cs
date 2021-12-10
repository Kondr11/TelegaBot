using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MambaSportBot.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Liga { get; set; }
        public string Url { get; set; }
        public ICollection<Calendar> Calendar { get; set; }
        public ICollection<Player> Players { get; set; }
        public Team()
        {
            Players = new List<Player>();
            Calendar = new List<Calendar>();
        }
    }
}
