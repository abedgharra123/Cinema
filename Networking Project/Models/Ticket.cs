using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        
        public string Movietitle { get; set; }

        public int Hall { get; set; }

        public DateTime Date { get; set; }

        public int Seat { get; set; }

        public int Price { get; set; }

        public int Userid { get; set; }
    }
}