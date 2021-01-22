using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class Cart
    {
        public int id { get; set; }
        public int seat { get; set; }
        public DateTime date { get; set; }
        public int amount { get; set; }
        public int movieid { get; set; }
    }
}