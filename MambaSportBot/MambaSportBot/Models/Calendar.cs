using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MambaSportBot.Models
{
    public class Calendar
    {
        public string Date { get; set; }
        public string Tournament { get; set; }
        public string Opponent { get; set; }
        public string Field { get; set; }
        public string Score { get; set; }
    }
}
