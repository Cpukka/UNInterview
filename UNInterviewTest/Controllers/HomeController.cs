using System.Linq;
using System.Web.Mvc;

using UNInterviewTest.Models;

namespace UNInterviewTest.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //protected void btnLeave_Click(object sender, EventArgs e)
        //{

        //    lblError.Text = "";
        //    int employeeId = Convert.ToInt32(drpEmployee.SelectedValue);


        //    if (String.IsNullOrWhiteSpace(txtStartDate.Text))
        //    {
        //        lblError.Text = ("Start Date is required");
        //        return;
        //    }
        //    if (String.IsNullOrWhiteSpace(txtEndDate.Text))
        //    {
        //        lblError.Text = ("End Date is required");
        //        return;
        //    }

        //    DateTime startDate;
        //    if (!DateTime.TryParse(txtStartDate.Text, out startDate))
        //    {
        //        lblError.Text = ("Start Date is not valid");
        //        return;
        //    }

        //    DateTime endDate;
        //    if (!DateTime.TryParse(txtEndDate.Text, out endDate))
        //    {
        //        lblError.Text = ("End Date is not valid");
        //        return;
        //    }

        //    if (startDate > endDate)
        //    {
        //        lblError.Text = ("End Date should not be less than start date");
        //        return;
        //    }



        //    try
        //    {
        //        using (var context = new CompanyContext())
        //        {
        //            var leaveRequests = context.LeaveRequests.ToList();


        //            var employeeList = (from l in context.LeaveRequests
        //                                join o in context.Employees on l.EmployeeId equals o.Id
        //                                select new LeaveRequestViewModel
        //                                {
        //                                    FirstName = o.FirstName,
        //                                    LastName = o.LastName,
        //                                    StartDate = l.StartDate,
        //                                    EndDate = l.EndDate,
        //                                    Department = o.Department
        //                                }).ToList();



        //            #region Validate 1
        //            // An employee should not have multiple leave request whose dates overlap 

        //            var employeeleaveRequests = leaveRequests.Where(w => w.EmployeeId == employeeId).ToList();

        //            if (employeeleaveRequests.Count > 0)
        //            {
        //                if (employeeleaveRequests.Any(a => a.StartDate.Date <= endDate.Date && startDate.Date <= a.EndDate.Date))
        //                {
        //                    lblError.Text = ("An employee should not have multiple leave request whose dates overlap");
        //                    return;
        //                }
        //            }
        //            #endregion



        //            #region Validate 2
        //            // An employee should not be allowed to make a leave request that overlaps with another employee of the same department 

        //            var currentEmployeeDepartment = context.Employees.Where(w => w.Id == employeeId).FirstOrDefault();
        //            var employeeRequestList = employeeList.Where(o => o.Department == currentEmployeeDepartment.Department);



        //            if (employeeRequestList.Any(a => a.StartDate.Date <= endDate.Date && startDate.Date <= a.EndDate.Date))

        //            {
        //                lblError.Text = ("An employee should not be allowed to make a leave request that overlaps with another employee of the same department");
        //                return;
        //            }
        //            #endregion


        //            #region Validate 3

        //            // An employee should not be allowed to make a leave request if the end of their previous leave request is less than one month. 

        //            var lastLeaveRequest = leaveRequests.Where(w => w.EmployeeId == employeeId).OrderByDescending(o => o.EndDate).FirstOrDefault();

        //            if (lastLeaveRequest != null)
        //            {
        //                int monthDiff = GetMonthDifference(startDate.Date, lastLeaveRequest.EndDate.Date);

        //                if (monthDiff < 1)
        //                {
        //                    lblError.Text = ("An employee should not be allowed to make a leave request if the end of their previous leave request is less than one month");
        //                    return;
        //                }
        //            }

        //            #endregion



        //            LeaveRequest leaveRequest = new LeaveRequest
        //            {
        //                EmployeeId = employeeId,
        //                StartDate = startDate,
        //                EndDate = endDate
        //            };

        //            context.LeaveRequests.Add(leaveRequest);
        //            context.SaveChanges();

        //            txtStartDate.Text = "";
        //            txtEndDate.Text = "";
        //            lblError.Text = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblError.Text = (ex.Message);
        //    }
        //}

    }
}