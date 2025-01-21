using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DropdownWithDependency.Controllers
{
    public class DropdownController : Controller
    {
        // İl listesi
        public IActionResult GetCities()
        {
            var cities = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "İstanbul" },
            new SelectListItem { Value = "2", Text = "Ankara" }
        };
            return Json(cities);
        }

        // İl'e bağlı ilçeler
        public IActionResult GetDistricts(int parentValue)
        {
            var districts = new List<SelectListItem>();

            if (parentValue == 1) // İstanbul
            {
                districts.Add(new SelectListItem { Value = "1", Text = "Kadıköy" });
                districts.Add(new SelectListItem { Value = "2", Text = "Üsküdar" });
            }
            else if (parentValue == 2) // Ankara
            {
                districts.Add(new SelectListItem { Value = "3", Text = "Çankaya" });
                districts.Add(new SelectListItem { Value = "4", Text = "Keçiören" });
            }

            return Json(districts);
        }


        public IActionResult GetDropdownData(int parentId, int level)
        {
            // Örnek veriler
            var data = new List<SelectListItem>();

            if (level == 1) // İlçeler
            {
                if (parentId == 1) // İstanbul
                {
                    data.Add(new SelectListItem { Value = "1", Text = "Kadıköy" });
                    data.Add(new SelectListItem { Value = "2", Text = "Üsküdar" });
                }
                else if (parentId == 2) // Ankara
                {
                    data.Add(new SelectListItem { Value = "3", Text = "Çankaya" });
                    data.Add(new SelectListItem { Value = "4", Text = "Keçiören" });
                }
            }
            else if (level == 2) // Okullar
            {
                if (parentId == 1) // Kadıköy
                {
                    data.Add(new SelectListItem { Value = "1", Text = "Kadıköy Lisesi" });
                    data.Add(new SelectListItem { Value = "2", Text = "Kadıköy Ortaokulu" });
                }
                else if (parentId == 3) // Çankaya
                {
                    data.Add(new SelectListItem { Value = "3", Text = "Çankaya Anadolu Lisesi" });
                }
            }
            else if (level == 3) // Sınıflar
            {
                if (parentId == 1) // Kadıköy Lisesi
                {
                    data.Add(new SelectListItem { Value = "1", Text = "9. Sınıf" });
                    data.Add(new SelectListItem { Value = "2", Text = "10. Sınıf" });
                }
                else if (parentId == 3) // Çankaya Anadolu Lisesi
                {
                    data.Add(new SelectListItem { Value = "3", Text = "11. Sınıf" });
                }
            }

            return Json(data);
        }
    }

}
