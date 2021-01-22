using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class Hall
    {
        [Key]
        public int Hid { get; set; }

        [Required]
        [DisplayName("Number of Seats")]
        public int number_of_seats{ get; set; }

        [Required]
        [DisplayName("Hall Number")]
        public int HallNumber { get; set; }

    }
}