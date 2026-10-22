using System;
using System.ComponentModel.DataAnnotations;

namespace KeelteKoolV2.Models
{
    public class StudentDetailsViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Eesnimi")]
        public string FirstName { get; set; }

        [Display(Name = "Perenimi")]
        public string LastName { get; set; }

        [Display(Name = "E-post")]
        public string Email { get; set; }

        [Display(Name = "Loomise kuupäev")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}