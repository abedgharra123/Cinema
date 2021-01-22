using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Networking_Project.Models
{
    public class Most
    {
        [Key]
        public string Title { get; set; }
        public int rate { get; set; }
    }
}