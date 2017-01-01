using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web.Models;
using Newtonsoft.Json.Linq;

namespace web.Controllers
{
    public class transactionsController : Controller
    {
        private xscoreEntities2 db = new xscoreEntities2();

        // GET: transactions
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Index()
        {

            var a = (from q in db.transactions
                     join q2 in db.categories on q.categories_id equals q2.categories_id
                     join q3 in db.objects on q.object_id equals q3.object_id
                     join q4 in db.events on q.events_id equals q4.event_id
                     

                     select new transactionView()
                     {
                         id = q.id,
                         category_id = q.categories_id,
                         category_name = q2.category_name,
                         domain = q.domain,
                         events_id = q4.event_id,
                         events_name = q4.event_name,
                         object_name = q3.object_name,
                         object_id = q.object_id,
                         score = q.score,
                         sub_category_id = q.sub_category_id,
                         time_stamp = q.time_stamp,
                         user_profile_id = q.user_profile_id

                     });

            return View(await a.ToListAsync());
        }

        // GET: transactions/Details/5
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaction transaction = await db.transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: transactions/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "categories_id,sub_category_id,object_id,events_id,score,user_profile_id,domain,time_stamp")] transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.transactions.Add(transaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(transaction);
        }
        [HttpGet]
        [Authorize]
        // GET: transactions/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var func = new transactionView();

            var a = (from q in db.transactions.ToList()
                     where q.id == id
                     join q2 in db.categories on q.categories_id equals q2.categories_id
                     join q3 in db.objects on q.object_id equals q3.object_id
                     join q4 in db.events on q.events_id equals q4.event_id


                     select new transactionView()
                     {
                         id = q.id,
                         category_id = q.categories_id,
                         category_name = q2.category_name,
                         domain = q.domain,
                         events_id = q4.event_id,
                         events_name = q4.event_name,
                         object_name = q3.object_name,
                         object_id = q.object_id,
                         score = q.score,
                         sub_category_id = q.sub_category_id,
                         time_stamp = q.time_stamp,
                         user_profile_id = q.user_profile_id,
                         //   categories = db.categories.Select(u => new SelectListItem { Text = u.category_name,Value = u.categories_id.ToString() }),
                         categories = func.GetCategories(db,q2),
                         //objects = db.objects.Where(x => x.categories_id == q.categories_id).Select(u => new SelectListItem { Text = u.object_name, Value = u.object_id.ToString() }),
                         objects = func.GetObjects(db,q3),
                         events = func.GetEvents(db,q4)
                         // events = db.events.Where(x => x.categories_id == q.categories_id).Select(u => new SelectListItem { Text = u.event_name, Value = u.event_id.ToString() })
                     }).ToList().AsEnumerable();
           // var modal = new transactionView();

            if (a == null)
            {
                return HttpNotFound();
            }
            return View(a);
        }


// POST: transactions/Edit/5
// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,categories_id,sub_category_id,object_id,events_id,score,user_profile_id,domain,time_stamp")] FormCollection m)
        {
            long id = 0;
            var t = new transaction();
            if (ModelState.IsValid)
            {
               
                long object_id = 0;
                long event_id = 0;
                long category_id = 0;
                int score = 0;
         
                string sSubmitted = "";
                //how do i detect which drop down was selected
                //how do it get its TEXT value so i dont have to do this....


                try
                {

                    category_id = Convert.ToInt64(m["Categories"]);
                    object_id = Convert.ToInt64(m["Objects"]);
                    event_id = Convert.ToInt64(m["Events"]);
                    score = Convert.ToInt32(m["Score"]);
                    id = Convert.ToInt32(m["item.id"]);
                }
                catch { }
                t.id = id;
                t.categories_id = category_id;
                t.object_id = object_id;
                t.events_id = event_id;
                t.score = score;

                db.Entry(t).State = EntityState.Modified;
                await db.SaveChangesAsync();
                
            }
            return RedirectToAction("Index");
          
        }

        // GET: transactions/Delete/5
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaction transaction = await db.transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            transaction transaction = await db.transactions.FindAsync(id);
            db.transactions.Remove(transaction);
            await db.SaveChangesAsync();
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
