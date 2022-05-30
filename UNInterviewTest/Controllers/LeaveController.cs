using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using UNInterviewTest.Models;

namespace UNInterviewTest.Controllers
{
    public class LeaveController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Leave
        public ActionResult Index()
        {
            var leaveRequests = db.LeaveRequests.Include(l => l.Employee);
            return View(leaveRequests.ToList());
        }

        // GET: Leave/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leaveRequest = db.LeaveRequests.Where(w => w.Id == id).Include(l => l.Employee).FirstOrDefault();
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequest);
        }

        // GET: Leave/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeId,StartDate,EndDate")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                if (ValidateLeave(leaveRequest))
                {
                    db.LeaveRequests.Add(leaveRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName", leaveRequest.EmployeeId);
            return View(leaveRequest);
        }

        // GET: Leave/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LeaveRequest leaveRequest = db.LeaveRequests.Find(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName", leaveRequest.EmployeeId);
            return View(leaveRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeId,StartDate,EndDate")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {

                db.Entry(leaveRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FullName", leaveRequest.EmployeeId);
            return View(leaveRequest);
        }

        // GET: Leave/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leaveRequest = db.LeaveRequests.Where(w => w.Id == id).Include(l => l.Employee).FirstOrDefault();

            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequest);
        }

        // POST: Leave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveRequest leaveRequest = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaveRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        protected bool ValidateLeave(LeaveRequest leaveRequest)
        {

            int employeeId = Convert.ToInt32(leaveRequest.EmployeeId);


            if (String.IsNullOrWhiteSpace(leaveRequest.StartDate.ToString()))
            {
                ModelState.AddModelError(string.Empty, "Start Date is required");
                return false;
            }
            if (String.IsNullOrWhiteSpace(leaveRequest.EndDate.ToString()))
            {
                ModelState.AddModelError(string.Empty, "End Date is required");
                return false;
            }

            DateTime startDate;
            if (!DateTime.TryParse(leaveRequest.StartDate.ToString(), out startDate))
            {
                ModelState.AddModelError(string.Empty, "Start Date is not valid");
                return false;
            }

            DateTime endDate;
            if (!DateTime.TryParse(leaveRequest.EndDate.ToString(), out endDate))
            {
                ModelState.AddModelError(string.Empty, "End Date is not valid");
                return false;
            }

            if (startDate > endDate)
            {
                ModelState.AddModelError(string.Empty, "End Date should not be less than start date");
                return false;
            }




            try
            {

                var leaveRequests = db.LeaveRequests.ToList();


                var employeeList = (from l in leaveRequests
                                    join o in db.Employees on l.EmployeeId equals o.Id
                                    select new LeaveRequestViewModel
                                    {
                                        FirstName = o.FirstName,
                                        LastName = o.LastName,
                                        StartDate = l.StartDate,
                                        EndDate = l.EndDate,
                                        Department = o.Department
                                    }).ToList();




                var employeeleaveRequests = leaveRequests.Where(w => w.EmployeeId == employeeId).ToList();

                if (employeeleaveRequests.Count > 0)
                {
                    if (employeeleaveRequests.Any(a => a.StartDate.Date <= endDate.Date && startDate.Date <= a.EndDate.Date))
                    {
                        ModelState.AddModelError(string.Empty, "An employee should not have multiple leave request whose dates overlap");
                        return false;
                    }

                    var currentEmployeeDepartment = db.Employees.Where(w => w.Id == employeeId).FirstOrDefault();
                    var employeeRequestList = employeeList.Where(o => o.Department == currentEmployeeDepartment.Department);



                    if (employeeRequestList.Any(a => a.StartDate.Date <= endDate.Date && startDate.Date <= a.EndDate.Date))

                    {
                        ModelState.AddModelError(string.Empty, "An employee should not be allowed to make a leave request that overlaps with another employee of the same department");
                        return false;
                    }



                    var lastLeaveRequest = leaveRequests.Where(w => w.EmployeeId == employeeId).OrderByDescending(o => o.EndDate).FirstOrDefault();

                    if (lastLeaveRequest != null)
                    {
                        int monthDiff = GetMonthDifference(startDate.Date, lastLeaveRequest.EndDate.Date);

                        if (monthDiff < 1)
                        {
                            ModelState.AddModelError(string.Empty, "An employee should not be allowed to make a leave request if the end of their previous leave request is less than one month");
                            return false;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return false;
            }

            return true;
        }
        protected int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
    }
}
