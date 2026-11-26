using System;
using System.ComponentModel.DataAnnotations;

namespace KeelteKoolV2.Models
{
    public class StudentCreateUpdateViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Eesnimi on kohustuslik")]
        [Display(Name = "Eesnimi")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Perenimi on kohustuslik")]
        [Display(Name = "Perenimi")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-post on kohustuslik")]
        [EmailAddress(ErrorMessage = "Vigane e-posti aadress")]
        [Display(Name = "E-post")]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}