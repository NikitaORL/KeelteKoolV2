using System;
using System.ComponentModel.DataAnnotations;

namespace KeelteKoolV2.Models
{
    public class StudentDeleteViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Eesnimi")]
        public string FirstName { get; set; }

        [Display(Name = "Perenimi")]
        public string LastName { get; set; }

        [Display(Name = "E-post")]
        public string Email { get; set; }
    }
}