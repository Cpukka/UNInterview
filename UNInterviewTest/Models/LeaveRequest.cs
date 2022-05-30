using System;
using System.ComponentModel.DataAnnotations;

namespace UNInterviewTest.Models
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Employeee Name")]
        public int EmployeeId { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Employeee Name")]
        public Employee Employee { get; set; }

    }
}