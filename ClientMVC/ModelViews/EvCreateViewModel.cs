using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClientMvc.ModelViews
{
    public class EvCreateViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Main information")]
        public string? Message { get; set; }

        [Required]
        [Display(Name = "Start time")]
        public DateTime BeginTime { get; set; }
        [Required]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }

        public double? Lattitude { get; set; }
        public double? Longtitude { get; set; }

        [Display(Name = "Location")]
        public string? LocationName { get; set; }

        public Guid UserId { get; set; }
    }
}
