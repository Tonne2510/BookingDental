using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking_Dental_Clinic.Models;
using PagedList;

namespace Booking_Dental_Clinic.Areas.Admin.Controllers
{
    public class InvoiceManagerController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: Admin/InvoiceManager
        public ActionResult Index(int? page)
        {
            int pageSize = 5; // Số lượng hóa đơn hiển thị trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại

            var hoaDons = db.HoaDons.Include(h => h.AspNetUser).Include(h => h.GoiDichVu).Include(h => h.HinhThucThanhToan).Include(h => h.NhaSi)
                            .OrderByDescending(h => h.ID_HoaDon); // Sắp xếp danh sách hóa đơn theo ID giảm dần

            var pagedHoaDons = hoaDons.ToPagedList(pageNumber, pageSize);

            return View(pagedHoaDons);
        }
        // GET: Admin/InvoiceManager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }
        // GET: Admin/InvoiceManager/Approve/5
        public ActionResult Approve(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            // Perform the approval logic here, such as updating the status
            hoaDon.TrangThai = true;

            db.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Admin/InvoiceManager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: Admin/InvoiceManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);
            db.HoaDons.Remove(hoaDon);
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
    }
}
