using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calendar.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        #region Load Events - Kullanıcının var olan planlarını yükler.

        /// <summary>
        /// Load Events
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEvents()
        {
            using (EventsEntities db = new EventsEntities())
            {
                var data = db.Events.ToList();

                return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        #endregion

        #region Add/Update New Event - Kullanıcının yeni plan kaydetmesini veya güncellemesini sağlar.
        /// <summary>
        /// Add/Update Events
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveEvent(Events a)
        {
            var status = false;
            using (EventsEntities db = new EventsEntities())
            {

                if (a.EventId > 0)
                {
                    var evnt = db.Events.Where(b => b.EventId == a.EventId).FirstOrDefault();
                    if (evnt != null)
                    {
                        evnt.EventName = a.EventName;
                        evnt.StartTime = a.StartTime;
                        evnt.EndTime = a.EndTime;
                    }
                }
                else
                {
                    db.Events.Add(a);
                }
                db.SaveChanges();

                status = true;
            }
            return new JsonResult { Data = new {  status } };
        }
        #endregion

        #region Delete Event - Kullanıcının planı silmesi için.
        /// <summary>
        /// Delete Event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteEvent(int eventId)
        {
            var status = false;
            using (EventsEntities db = new EventsEntities())
            {
                var ev = db.Events.Where(a => a.EventId == eventId).FirstOrDefault();
                if (ev != null)
                {
                    db.Events.Remove(ev);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status } };
        }
        #endregion
    }
}