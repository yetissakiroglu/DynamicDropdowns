using Microsoft.AspNetCore.Mvc.Rendering;

namespace DropdownWithDependency.HtmlHelpers
{
    public class DropdownConfig
    {
        public string Label { get; set; } // Dropdown için etiket (opsiyonel)
        public List<SelectListItem> InitialItems { get; set; } // Başlangıç elemanları
        public string SelectedValue { get; set; } // Varsayılan seçili değer
    }
}
