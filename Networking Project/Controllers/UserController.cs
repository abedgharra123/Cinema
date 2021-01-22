using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Networking_Project.Models;
using System.Security.Cryptography;
using Networking_Project.VM;
using Networking_Project.Dal;
using System.Data.Entity;
using System.Globalization;

namespace Networking_Project.Controllers
{
    public class UserController : Controller
    {
        private DB_Entities _db = new DB_Entities();
        // GET: Home
        public ActionResult Index()
        {
            //removing ended movies and tickets
            using (MovieDal mdb = new MovieDal())
            {
                foreach (Movie movie in mdb.Movies.ToList<Movie>())
                {
                    int res = DateTime.Compare(movie.Date, DateTime.Now);
                    if (res < 0)
                    {

                        mdb.Movies.Remove(movie);
                        mdb.SaveChanges();
                        TicketDal tdb = new TicketDal();
                        foreach (Ticket t in tdb.Tickets.ToList<Ticket>())
                        {
                            if (t.Movietitle == movie.Title && t.Hall == movie.Hall && DateTime.Compare(movie.Date, t.Date) == 0)
                            {
                                tdb.Tickets.Remove(t);
                                tdb.SaveChanges();
                            }
                        }
                    }
                }
            }

            //creating id for user
            if (Session["idUser"] == null)
            {
                Random rnd = new Random();
                int id = rnd.Next(1000);
                Session["idUser"] = id;
            }
            using (MovieDal mdb = new MovieDal())
            {
                return View("Home", mdb.Movies.ToList<Movie>().Where(x => x.Sale == null));
            }

        }
        //sorting movies

        public ActionResult Index99(int from,int to,bool Decrease=false, bool Increase = false)
        {
            MovieDal mdb = new MovieDal();
            List<Movie> lm = new List<Movie>();

            string[] formats = {"DD/MM/yyyy"};

            //price range
            foreach (Movie x in mdb.Movies.ToList<Movie>().Where(m => m.Sale == null && m.Price > from && m.Price < to))
            {
                ViewBag.range = "from " + from.ToString() + " to " + to.ToString();
                //both catefory and date
                if (Request.Form["Select Category"] != null && Request.Form["Select Category"].ToString() != "All" && DateTime.TryParse(Request.Form["date1"], out DateTime date1) == true)
                {
                    ViewBag.category = Request.Form["Select Category"].ToString();
                    ViewBag.date5 = date1.ToString();
                    if (x.Category == Request.Form["Select Category"].ToString() && x.Date.Year == date1.Year && x.Date.Month == date1.Month && x.Date.Day == date1.Day)
                    {
                        lm.Add(x);
                    }
                }

                //sort by category alone
                else if (Request.Form["Select Category"] != null && Request.Form["Select Category"].ToString() != "All")
                {
                    ViewBag.category = Request.Form["Select Category"].ToString();
                    if (Request.Form["Select Category"].ToString() != "All")
                    {
                        if (x.Category == Request.Form["Select Category"].ToString())
                        {
                            lm.Add(x);
                        }
                    }
                }
                //sort date alone
                else if (DateTime.TryParse(Request.Form["date1"], out DateTime date11) == true)
                {
                    if(x.Date.Year == date11.Year && x.Date.Month == date11.Month && x.Date.Day == date11.Day)
                    {
                        ViewBag.date5 = date11.ToString();
                        lm.Add(x);
                    }
                }

                else
                    lm.Add(x);
            }
            //price decrease .. increase
            if (Decrease)
                lm = lm.OrderBy(q => q.Price).Reverse().ToList();
            if (Increase)
                lm = lm.OrderBy(q => q.Price).ToList();
            return View("Home",lm);
        }

        //movie details

        public ActionResult Details(int id)
        {
            using (MovieDal mdb = new MovieDal())
            {
                return View( mdb.Movies.Where(x => x.mid==id).FirstOrDefault());
            }
        }
        //edit movie
        public ActionResult Edit(int id)
        {
            using (MovieDal mdb = new MovieDal())
            {
                return View(mdb.Movies.Where(x => x.mid == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Edit(int id , Movie c)
        {
            try
            {
                using (MovieDal mdb = new MovieDal())
                {
                    c.Picture = "~/UploadedFiles/" + c.Picture.ToString();
                    mdb.Entry(c).State = EntityState.Modified;
                    mdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        //sales
        public ActionResult PutOnSale(int id)
        {
            using (MovieDal mdb = new MovieDal())
            {
                return View(mdb.Movies.Where(x => x.mid == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult PutOnSale(int id,int sale)
        {
            try
            {
                using (MovieDal mdb = new MovieDal())
                {
                    Movie c = mdb.Movies.Where(x => x.mid == id).FirstOrDefault();
                    c.Sale = sale;
                    mdb.Entry(c).State = EntityState.Modified;
                    mdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Sales()
        {
            if (Session["idUser"] == null)
            {
                Random rnd = new Random();
                int id = rnd.Next(1000);
                Session["idUser"] = id;

            }
            using (MovieDal mdb = new MovieDal())
            {
                return View(mdb.Movies.ToList<Movie>().Where(x=>x.Sale != null));
            }
        }

        //delete movie
        public ActionResult Delete(int id)
        {
            using (MovieDal mdb = new MovieDal())
            {
                return View(mdb.Movies.Where(x => x.mid == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Delete(int id,Movie c)
        {
            try
            {
                using (MovieDal mdb = new MovieDal())
                {
                    Movie m = mdb.Movies.Where(x => x.mid == id).FirstOrDefault();
                    mdb.Movies.Remove(m);
                    mdb.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
        // home screen
        public ActionResult Home()
        {
            if(Session["idUser"] == null)
            {
                Random rnd = new Random();
                int id = rnd.Next(1000);
                Session["idUser"] = id;

            }
            using (MovieDal mdb = new MovieDal())
            {
                return View(mdb.Movies.ToList<Movie>().Where(x => x.Sale == null));
            }
        }
        //most popular
        public ActionResult Mostpopular()
        {
            MostDal mostdb = new MostDal();
            MovieDal mdb = new MovieDal();
            List<Movie> lm = new List<Movie>();
            foreach (Most most in mostdb.Mosts.ToList<Most>().OrderBy(x => x.rate).Reverse())
            {
                foreach (Movie movie in mdb.Movies.ToList<Movie>())
                {
                    if (movie.Title == most.Title && movie.Sale == null)
                        lm.Add(movie);
                }
            }
            return View("Home",lm);
        }






        //user login signup
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Users.Add(_user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();


        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = _db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().idUser;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }


            //create a string MD5
            public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}