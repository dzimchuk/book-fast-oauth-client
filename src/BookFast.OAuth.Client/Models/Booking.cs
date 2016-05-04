using System;
using System.ComponentModel.DataAnnotations;

namespace BookFast.OAuth.Client.Models
{
    public class Booking
    {
        [Display(Name = "Facility")]
        public string FacilityName { get; set; }

        [Display(Name = "Accommmodation")]
        public string AccommodationName { get; set; }

        [Display(Name = "From")]
        public DateTimeOffset FromDate { get; set; }

        [Display(Name = "To")]
        public DateTimeOffset ToDate { get; set; }
    }
}