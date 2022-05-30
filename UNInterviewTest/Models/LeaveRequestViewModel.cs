using System;

namespace UNInterviewTest.Models
{
    public class LeaveRequestViewModel
    {
        public int EmployeeId { get; set; }
        public int LeaveRequestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}