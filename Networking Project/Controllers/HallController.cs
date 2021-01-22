using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Networking_Project.Dal;
using Networking_Project.Models;
using Networking_Project.VM;
namespace Networking_Project.Controllers
{
    public class HallController : Controller
    {
        // GET: Hall
        public ActionResult Edit(int id)
        {
            using (HallDal mdb = new HallDal())
            {
                return View(mdb.Halls.Where(x => x.Hid == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, Hall c)
        {
            try
            {
                using (HallDal mdb = new HallDal())
                {
                    mdb.Entry(c).State = EntityState.Modified;
                    mdb.SaveChanges();
                }
                return RedirectToAction("Halls");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Hall c)
        {
            try
            {
                using (HallDal mdb = new HallDal())
                {
                    mdb.Halls.Add(c);
                    mdb.SaveChanges();
                }
                return RedirectToAction("Halls");
            }
            catch
            {
                return View();
            }
        }



        public ActionResult Halls()
        {

            using (HallDal mdb = new HallDal())
            {
                return View(mdb.Halls.ToList<Hall>());
            }
        }

        public ActionResult Delete(int id)
        {
            using (HallDal mdb = new HallDal())
            {
                return View(mdb.Halls.Where(x => x.Hid == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Delete(int id, Hall c)
        {
            try
            {
                using (HallDal mdb = new HallDal())
                {
                    Hall m = mdb.Halls.Where(x => x.Hid == id).FirstOrDefault();
                    mdb.Halls.Remove(m);
                    mdb.SaveChanges();
                    return RedirectToAction("Halls");
                }
            }
            catch
            {
                return View();
            }
        }

    }
}