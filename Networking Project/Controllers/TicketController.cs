using Networking_Project.Dal;
using Networking_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Networking_Project.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        public ActionResult seats(int id)
        {
            using (MovieDal m = new MovieDal())
            {

                return View(m.Movies.ToList<Movie>().Where(x => x.mid == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult seats(int id, Movie m , int seat)
        {
            MovieDal mdb = new MovieDal();
            Movie m1 = mdb.Movies.ToList<Movie>().Where(x => x.mid == id).FirstOrDefault();

            //out of range seats
            HallDal hd = new HallDal();
            Hall h = hd.Halls.ToList<Hall>().Where(x => x.HallNumber == m1.Hall).FirstOrDefault();
            if(h.number_of_seats < seat || seat < 1)
            {
                ViewBag.check = "Out of range seat! , please choose a seat from 1 to "+h.number_of_seats.ToString();
                return View(m1);
            }


            //check if the seat is in occupied
            TicketDal tdb = new TicketDal();
            List<Ticket> t = tdb.Tickets.ToList<Ticket>();
            int count = 0;
            foreach(Ticket t1 in t)
            {
                if (t1.Seat == seat && t1.Hall == m1.Hall && t1.Date.Equals(m1.Date))
                    count++;
            }
            //create ticket
            if(count == 0)
            {

                Ticket tt = new Ticket();
                tt.Hall = m1.Hall;
                if (m1.Sale == null)
                    tt.Price = m1.Price;
                else
                    tt.Price = (int)m1.Sale;
                tt.Id = m1.mid;
                tt.Movietitle = m1.Title;
                tt.Date = m1.Date;
                tt.Seat = seat;
                tt.Userid = (int)Session["idUser"];
                tdb.Tickets.Add(tt);
                tdb.SaveChanges();
                ViewBag.check = "added to cart succefully !";
                //add rating to most popular
                MostDal ms = new MostDal();
                Most most = ms.Mosts.ToList<Most>().Where(x => x.Title == m1.Title).FirstOrDefault();
                most.rate += 1;
                ms.Entry(most).State = EntityState.Modified;
                ms.SaveChanges();
            }
            else
                ViewBag.check = "this seat in occupied !";
            return View(m1);
        }
        public ActionResult Cart()
        {
            if (Session["idUser"] == null)
            {
                Random rnd = new Random();
                int id = rnd.Next(1000);
                Session["idUser"] = id;
            }

            using (TicketDal mdb = new TicketDal())
            {
                int d = (int)Session["idUser"];
                return View(mdb.Tickets.ToList<Ticket>().Where(x => x.Userid == d));
            }
        }


        public ActionResult Delete(int id)
        {
            using (TicketDal mdb = new TicketDal())
            {
                return View(mdb.Tickets.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Delete(int id, Ticket c)
        {
            try
            {
                using (TicketDal mdb = new TicketDal())
                {
                    Ticket m = mdb.Tickets.Where(x => x.Id == id).FirstOrDefault();
                    mdb.Tickets.Remove(m);
                    mdb.SaveChanges();
                    return RedirectToAction("Cart");
                }
            }
            catch
            {
                return View();
            }
        }

    }
}