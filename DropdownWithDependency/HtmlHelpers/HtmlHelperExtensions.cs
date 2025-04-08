using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
namespace DropdownWithDependency.HtmlHelpers
{

    public static class HtmlHelperExtensions
    {
        public static IHtmlContent EnumCheckboxGroupFor<TEnum>(
            this IHtmlHelper htmlHelper,
            string name,
            string targetDivId,
            string triggerValue,
            string inputIdToMakeRequired)
            where TEnum : struct, Enum
        {
            var enumType = typeof(TEnum);
            var values = Enum.GetValues(enumType);

            var sb = new StringBuilder();
            sb.AppendLine("<div class='enum-checkbox-group'>");

            foreach (var val in values)
            {
                var intVal = ((int)val).ToString();
                var label = Enum.GetName(enumType, val);
                var checkboxId = $"{name}_{intVal}";

                sb.AppendLine($@"
<label for='{checkboxId}' style='margin-right:10px;'>
    <input type='checkbox' name='{name}' value='{intVal}' id='{checkboxId}' class='enum-checkbox' />
    {label}
</label>");
            }

            sb.AppendLine("</div>");

            sb.AppendLine($@"
<script>
document.addEventListener('DOMContentLoaded', function () {{
    const checkboxes = document.querySelectorAll('.enum-checkbox[name=""{name}""]');
    const target = document.getElementById('{targetDivId}');
    const input = document.getElementById('{inputIdToMakeRequired}');

    function updateView() {{
        let selectedValue = null;
        checkboxes.forEach(cb => {{
            if (cb.checked) {{
                selectedValue = cb.value;
            }} else {{
                cb.checked = false;
            }}
        }});

        if (selectedValue === '{triggerValue}') {{
            target.style.display = 'block';
            input.setAttribute('required', 'required');
        }} else {{
            target.style.display = 'none';
            input.removeAttribute('required');
            input.value = '';
        }}
    }}

    checkboxes.forEach(cb => {{
        cb.addEventListener('change', function () {{
            checkboxes.forEach(c => {{ if (c !== cb) c.checked = false; }});
            updateView();
        }});
    }});

    updateView();
}});
</script>");

            return new HtmlString(sb.ToString());
        }
    }

  





}


