using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace DropdownWithDependency.HtmlHelpers
{
    public static class CustomHtmlHelpers
    {
        public static IHtmlContent DropdownWithDependency(
            this IHtmlHelper htmlHelper,
            string parentDropdownId, // Ana dropdown (ör: il)
            string childDropdownId, // Bağımlı dropdown (ör: ilçe)
            IEnumerable<SelectListItem> parentItems, // İlk dropdown için veriler
            string childDataUrl, // Ajax çağrısı yapılacak URL
            string parentSelectedValue = null, // Ana dropdown için seçili değer
            string childSelectedValue = null // Bağımlı dropdown için seçili değer
        )
        {
            // Ana dropdown
            var parentSelectTag = new TagBuilder("select");
            parentSelectTag.Attributes.Add("id", parentDropdownId);
            parentSelectTag.Attributes.Add("name", parentDropdownId);
            parentSelectTag.Attributes.Add("class", "form-control");

            // Dropdown'a seçenekleri ekle
            foreach (var item in parentItems)
            {
                var optionTag = new TagBuilder("option");
                optionTag.Attributes.Add("value", item.Value);
                if (item.Value == parentSelectedValue)
                    optionTag.Attributes.Add("selected", "selected");
                optionTag.InnerHtml.Append(item.Text);
                parentSelectTag.InnerHtml.AppendHtml(optionTag);
            }

            // Bağımlı dropdown (boş bırakılıyor, çünkü Ajax ile doldurulacak)
            var childSelectTag = new TagBuilder("select");
            childSelectTag.Attributes.Add("id", childDropdownId);
            childSelectTag.Attributes.Add("name", childDropdownId);
            childSelectTag.Attributes.Add("class", "form-control");

            // JavaScript kodu (Ajax ile veri yükleme)
            var scriptTag = new TagBuilder("script");
            scriptTag.InnerHtml.AppendHtml($@"
            document.getElementById('{parentDropdownId}').addEventListener('change', function() {{
                var parentValue = this.value;
                fetch('{childDataUrl}?parentValue=' + parentValue)
                    .then(response => response.json())
                    .then(data => {{
                        var childDropdown = document.getElementById('{childDropdownId}');
                        childDropdown.innerHTML = '';
                        data.forEach(function(item) {{
                            var option = document.createElement('option');
                            option.value = item.value;
                            option.textContent = item.text;
                            childDropdown.appendChild(option);
                        }});
                        if ('{childSelectedValue}' !== '') {{
                            childDropdown.value = '{childSelectedValue}';
                        }}
                    }});
            }});
        ");

            // Wrapper div
            var wrapperDiv = new TagBuilder("div");
            wrapperDiv.InnerHtml.AppendHtml(parentSelectTag);
            wrapperDiv.InnerHtml.AppendHtml(childSelectTag);
            wrapperDiv.InnerHtml.AppendHtml(scriptTag);

            return wrapperDiv;
        }




        public static IHtmlContent DynamicDropdowns(
      this IHtmlHelper htmlHelper,
      string baseDropdownId, // Ana dropdown ID'si için bir prefix
      List<DropdownConfig> dropdownConfigs, // Dropdown yapılandırmaları
      string fetchDataUrl // Veri getirmek için ortak bir URL
  )
        {
            var wrapperDiv = new TagBuilder("div");

            // Dropdown'ları oluştur
            for (int i = 0; i < dropdownConfigs.Count; i++)
            {
                var dropdownConfig = dropdownConfigs[i];
                var dropdownId = $"{baseDropdownId}_{i}";

                var selectTag = new TagBuilder("select");
                selectTag.Attributes.Add("id", dropdownId);
                selectTag.Attributes.Add("name", dropdownId);
                selectTag.Attributes.Add("class", "form-control");

                if (dropdownConfig.InitialItems != null)
                {
                    foreach (var item in dropdownConfig.InitialItems)
                    {
                        var optionTag = new TagBuilder("option");
                        optionTag.Attributes.Add("value", item.Value);
                        if (item.Value == dropdownConfig.SelectedValue)
                            optionTag.Attributes.Add("selected", "selected");
                        optionTag.InnerHtml.Append(item.Text);
                        selectTag.InnerHtml.AppendHtml(optionTag);
                    }
                }

                wrapperDiv.InnerHtml.AppendHtml(selectTag);
            }

            // JavaScript kodu
            var scriptTag = new TagBuilder("script");
            scriptTag.InnerHtml.AppendHtml($@"
                  (function() {{
                      var configs = {System.Text.Json.JsonSerializer.Serialize(dropdownConfigs)};
                      var baseId = '{baseDropdownId}';
                      var fetchUrl = '{fetchDataUrl}';

                      configs.forEach(function(config, index) {{
                          if (index === 0) return; // İlk dropdown bağımsızdır

                          var parentDropdownId = baseId + '_' + (index - 1);
                          var currentDropdownId = baseId + '_' + index;

                          document.getElementById(parentDropdownId).addEventListener('change', function() {{
                              var parentValue = this.value;
                              fetch(fetchUrl + '?parentId=' + parentValue + '&level=' + index)
                                  .then(response => response.json())
                                  .then(data => {{
                                      var currentDropdown = document.getElementById(currentDropdownId);
                                      currentDropdown.innerHTML = '';
                                      currentDropdown.appendChild(new Option('Seçiniz', ''));
                                      data.forEach(function(item) {{
                                          var option = document.createElement('option');
                                          option.value = item.value;
                                          option.textContent = item.text;
                                          currentDropdown.appendChild(option);
                                      }});

                                      // Alt dropdown'ları temizle
                                      for (var i = index + 1; i < configs.length; i++) {{
                                          var childDropdown = document.getElementById(baseId + '_' + i);
                                          if (childDropdown) childDropdown.innerHTML = '<option value=\"">Seçiniz</option>';}}}});}});}});}})();");

            wrapperDiv.InnerHtml.AppendHtml(scriptTag);
            return wrapperDiv;
        }



    }



}
