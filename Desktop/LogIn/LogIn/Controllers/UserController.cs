using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using LogIn.Models;
using LogIn.Filter;

namespace LogIn.Controllers
{
  
    public class UserController : Controller
    {
        P402Entities db = new P402Entities();
        // GET: User
        public ActionResult Login()
        {
            return View();
        }
       [Auth]
        public ActionResult Profile()
        {

            User usr = Session["User"] as User;
            return View(usr);
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            User usr = db.Users.FirstOrDefault(u => u.Email == email);
            if (usr != null)
            {
                if (Crypto.VerifyHashedPassword(usr.Password, password))
                {
                    Session["Login"] = true;
                    Session["User"] = usr;
                    return RedirectToAction("index", "home");
                }
            }

            Session["LoginError"] = true;
            return RedirectToAction("login");
        }

       
    }

}