using Networking_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Networking_Project.Dal;
using Networking_Project.VM;
using System.IO;

namespace Networking_Project.Controllers
{
    public class MovieController : Controller
    {


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Movie c)
        {

            c.Picture = "~/UploadedFiles/" + c.Picture.ToString();
            using (MovieDal mdb = new MovieDal())
            {
                foreach(Movie movie in mdb.Movies.ToList<Movie>())
                {
                    if (movie.Hall == c.Hall && DateTime.Compare(movie.Date, c.Date) == 0)
                    {
                        ViewBag.x = "This DateTime and Hall is choosen allready !";
                        return View();
                    }
                }
            }
            try
            {
                using (MovieDal mdb = new MovieDal())
                {

                    mdb.Movies.Add(c);
                    mdb.SaveChanges();
                    using (MostDal modb = new MostDal())
                    {
                        if (modb.Mosts.ToList<Most>().Where(x => x.Title == c.Title).Count() == 0)
                        {
                            Most m = new Most();
                            m.Title = c.Title;
                            modb.Mosts.Add(m);
                            modb.SaveChanges();
                        }

                    }

                }
                
                ViewBag.x = " Movie has been added Succecfully!";
                return RedirectToAction("Index","User");
            }
            catch
            {
                ViewBag.x = "Wrong input!";
                return View();
            }
        }
        public ActionResult MoviesJson()
        {
            MovieDal c = new MovieDal();
            List<Movie> obj = c.Movies.ToList<Movie>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
       
    }
}