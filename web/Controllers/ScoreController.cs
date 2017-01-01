using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.Models;

namespace web.Controllers
{
    public class ScoreController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {


            var ctx = new xscoreEntities2();
            var modal = new List<categoryModel>();

            var q = from c2 in ctx.categories
                    orderby c2.category_name ascending
                    select new categoryModel
                    {
                        categories_id = c2.categories_id,
                        category_name = c2.category_name,
                        domain = c2.domain,
                        SelectItem = new SelectListItem { Value = c2.categories_id.ToString(), Text = c2.category_name }
                    };

            modal = (List<categoryModel>)q.ToList();

        

            return View(modal);
        }

        #region old_stuff
        //  [Authorize]
        //   [ValidateAntiForgeryToken]
        /*public ActionResult Objects(ViewModelCategories m)
        {

            if (m.theList == null)
            {
               // m.err = "Either you are medaling with the values, or this object has been deleted";
                return View(m);
            }

            var ctx = new xscoreEntities2();
            //var model = new @object();

            var ret =
            from c in ctx.objects
            orderby c.object_name ascending
            where c.categories_id == (long)m.theList.SelectedValue
            select new
            {
                Id = c.object_id,
                Name = c.object_name
            };

            if (ret == null)
            {
                ViewBag.sErrorMessage = "Either you are medaling with the values, or this object has been deleted";
                return View(ViewBag);
            }

            var vmodel = new ViewModelObjects();
            //vmodel.theList = new SelectList(ret, "Id", "Name");

            return View(vmodel);
     //   }*/
        #endregion

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Objects(FormCollection m)
        {
            var ctx = new xscoreEntities2();
            var modelV = new EnterViewModel();
            long object_id = 0;
            long event_id = 0;
            long score = 0;
            string sSubmitted = "";
            //how do i detect which drop down was selected
            //how do it get its TEXT value so i dont have to do this....


            try
            {
                if (Session["category_id"] == null)
                {
                    Session["category_id"] = Convert.ToInt64(m["listCat"]);
                }
              
                 object_id =  Convert.ToInt64(m["Objects"]);
                 event_id = Convert.ToInt64(m["Events"]);
                 score = Convert.ToInt64(m["Score"]);
                 sSubmitted = m["hfield"];
            }
            catch { }

            if(sSubmitted == "submit")
            {
                var t = new transaction();
                t.categories_id = Convert.ToInt64(Session["category_id"]);
                t.domain = 1;
                t.events_id = event_id;
                t.object_id = object_id;
                t.sub_category_id = 1;
                t.user_profile_id = 1;
                t.time_stamp = DateTime.Now;
                ctx.transactions.Add(t);
                ctx.SaveChanges();
                return View();
            }

            if (m.Count < 1)
            {
                    ViewBag.err = "no items for this category";
                    return View(ViewBag);
            }


            long v = Convert.ToInt64(Session["category_id"]);


            //dont want to refresh this list (keep user picked item) when user chooses event type

            var list = (from q in ctx.objects
                            orderby q.object_name ascending
                            where q.categories_id == v 
                            select new SelectListItem()
                            {
                                Text = q.object_name,
                                Value = q.object_id.ToString()
                            }
                            ).ToList();

            if(object_id > 0)
            {
                //this....
                var sret = (from q in ctx.objects where q.categories_id == v && q.object_id == object_id
                            select q.object_name);

                list.Insert(0,new SelectListItem { Value = object_id.ToString(), Text = sret.FirstOrDefault() });
            }

            modelV.ObjectSelectItem = list;


            if (list == null)
            {
                ViewBag.sErrorMessage = "no item for this category";
                return View(ViewBag);
            }

                

                //after picking event type i dont want to refresh this list either, i want it to stay on what they picked but, it needs to change the rating levels below
                var list2 = (from q in ctx.events
                             orderby q.event_name ascending
                             where q.categories_id == v
                             select new SelectListItem()
                             {
                                 Text = q.event_name,
                                 Value = q.event_id.ToString()
                             }
                             ).ToList();

            if (event_id > 0)
            {
                var sret = (from q in ctx.events
                            where q.categories_id == v && q.event_id == event_id
                            select q.event_name);

                list2.Insert(0, new SelectListItem { Value = event_id.ToString(), Text = sret.FirstOrDefault() });
            }
            modelV.EventSelectItem = list2;
            

                

            var rating = (from q in ctx.events
                         where q.event_id == event_id
                         select q.rating_level );


             var a =  new List<SelectListItem> {new SelectListItem { Text = "1", Value= "1"},
                                                new SelectListItem { Text = "2", Value= "2"},
                                                new SelectListItem { Text = "3", Value= "3"} };


            switch (rating.FirstOrDefault())
            {
                case 0:
                    a = new List<SelectListItem> {new SelectListItem { Text = "-1", Value= "-1"},
                                                new SelectListItem { Text = "-2", Value= "-2"},
                                                new SelectListItem { Text = "-3", Value= "-3"} };
                                                break;
                case 1:
                    a = new List<SelectListItem> {new SelectListItem { Text = "1", Value= "1"},
                                                new SelectListItem { Text = "2", Value= "2"},
                                                new SelectListItem { Text = "3", Value= "3"} };
                                                break;
                case 2:
                    a = new List<SelectListItem> {new SelectListItem { Text = "-1", Value= "-1"},
                                                new SelectListItem { Text = "-2", Value= "-2"},
                                                new SelectListItem { Text = "-3", Value= "-3"},
                                                new SelectListItem { Text = "1", Value= "1"},
                                                new SelectListItem { Text = "2", Value= "2"},
                                                new SelectListItem { Text = "3", Value= "3"} };
                                                break;
            }



            modelV.ScoreSelectItem = a.ToList();
            


            return View(modelV);
        }



      

    }
}