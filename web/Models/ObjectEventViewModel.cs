using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Models
{
    public class EnterViewModel
    { 
        long event_id { get; set; }
        string event_name { get; set; }

        public IList<SelectListItem> EventSelectItem { get; set; }

        long object_id { get; set; }
        string object_name { get; set; }
        public IList<SelectListItem> ObjectSelectItem { get; set; }

        long rating_level { get; set; }
        string rating_text { get; set; }
        public IList<SelectListItem> ScoreSelectItem { get; set; }
    }
}