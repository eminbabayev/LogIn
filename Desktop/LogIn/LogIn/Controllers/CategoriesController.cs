using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogIn.Models;
using LogIn.Filter;
namespace LogIn.Controllers

{
    [Auth]
    public class CategoriesController : Controller
    {
        P402Entities db = new P402Entities();
        // GET: Categories
        public ActionResult Index()
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("login", "user");
            }
            User usr = Session["User"] as User;
            User user = db.Users.Find(usr.Id);
            return View(user);
        }
        public ActionResult Create()
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("login", "user");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(string Name)
        {
            User usr = Session["User"] as User;

            if (db.Categories.FirstOrDefault(c => c.UserId == usr.Id && c.Name == Name) != null)
            {
                Session["AlreadyHas"] = true;
                return RedirectToAction("index");
            }

            Category cnt = new Category
            {
                Name = Name,
                UserId = usr.Id
            };
            db.Categories.Add(cnt);
            db.SaveChanges();
            Session["ActionDone"] = true;
            return RedirectToAction("index");
        }

        public ActionResult Edit(int id)
        {

            Category cnt = db.Categories.Find(id);
            if (cnt == null)
            {
                return HttpNotFound();
            }
            User usr = Session["User"] as User;
            if (usr.Id != cnt.UserId)
            {
                return HttpNotFound();
            }


            return View(cnt);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            db.Entry(category).State = System.Data.Entity.EntityState.Modified;
            db.Entry(category).Property(c => c.UserId).IsModified = false;
            db.SaveChanges();
            return RedirectToAction("index");
        }


        public ActionResult Delete(int id)
        {

            Category cnt = db.Categories.Find(id);
            if (cnt == null)
            {
                return HttpNotFound();
            }

            User usr = Session["User"] as User;
            if (usr.Id != cnt.UserId)
            {
                return HttpNotFound();
            }

            if (cnt.Payments.Count > 0)
            {
                Session["DeleteInfo"] = true;
                return RedirectToAction("index");
            }

            db.Categories.Remove(cnt);
            db.SaveChanges();

            Session["ActionDone"] = true;
            return RedirectToAction("index");
        }

    }
}