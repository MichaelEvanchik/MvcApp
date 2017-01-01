using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Models
{
    public class ViewModelCategories
    {
        private readonly List<category> _category;

        [Display(Name = "Categories")]
        public int SelectedCategoryId { get; set; }



        public IEnumerable<SelectListItem> CategoryItems
        {

            get
            {
                var allFlavors = _category.Where(s => s.categories_id == 1)
               .Select(s => new SelectListItem
               {
                   Value = s.category_name,
                   Text = s.category_name
               });
                return allFlavors;

            }
        }

    }



    public class categoryModel : category
    {
        public SelectListItem SelectItem { get; set; }
    }

    public class objectModel : @object
    {
        public SelectListItem SelectItem { get; set; }
    }
    public class eventModel : @event
    {
        public SelectListItem SelectItem { get; set; }
    }

    public class slist
    {
        public SelectListItem SelectItem { get; set; }
    }
}

