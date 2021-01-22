using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Networking_Project.Models;
namespace Networking_Project.VM
{
    public class MovieViewModel
    {
        public List<Movie> Movies { get; set; }
        public Movie Movie { get; set; }
    }
}