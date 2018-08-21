using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoltDan.Models
{
    public static class HtmlExtensions
    {
#if false
        public static MvcHtmlString BELabelValueFormGroup<TModel>(this HtmlHelper<TModel> html,
                                                       string labelText,
                                                       string valueText,
                                                       int leftCol = 2,
                                                       bool isModal = false)
        {
            var rightCol = 12 - leftCol;
            var labelClasses = $"control-label col-md-{leftCol} beScreenFieldLabel";
            var inDivClasses = $"col-md-{rightCol}";
            //var labelForClasses = "control-label beScreenFieldLabel";
            //if (isModal) txtBoxClasses += " input-sm";

            var inDiv = new TagBuilder("div");
            inDiv.AddCssClass(inDivClasses);
            inDiv.InnerHtml += $"<h5>{valueText}</h5>";
            //$"<span style='vertical-align: middle;'>{valueText}</span>"; //html.TextBoxFor(expression, svsfcm, new { @class = txtBoxClasses, @placeholder = textPlaceHolder }).ToHtmlString();
            //html.DisplayFor(expression, new { @class = labelForClasses }).ToHtmlString();

            var topDiv = new TagBuilder("div");
            topDiv.AddCssClass("form-group");
            if (isModal) topDiv.AddCssClass("beModalFormGroup");

            topDiv.InnerHtml += html.Label(labelText, new { @class = labelClasses });
            topDiv.InnerHtml += inDiv;

            var x = topDiv.ToString();
            return MvcHtmlString.Create(topDiv.ToString());
        }
#endif
    }

}