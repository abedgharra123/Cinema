using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Networking_Project.Models
{
    public class Movie
    {
        [Key]
        public int mid { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Title length should be between 2 and 30")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat( ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int Hall { get; set; }

        [Required]
        public string Description { get; set; }
        [DisplayName("Image")]
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string Picture { get; set; }

        public string Category { get; set; }

        public int? Sale { get; set; }
    }
    public enum Categoty
    {
        All,
        Action,
        Comedy,
        Drama,
        Fantasy,
        Horror,
        Mystery,
        Romance
    }
}
