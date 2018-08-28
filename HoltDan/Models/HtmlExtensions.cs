using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoltDan.Models
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString DanSong<TModel>(this HtmlHelper<TModel> html,
                                                       string fileName,
                                                       string title,
                                                       string notes)
        {
            var inDiv = new TagBuilder("div");
            inDiv.AddCssClass("danSong");
            inDiv.InnerHtml += $"<div class='danTitle'>{title}</div><div class='danNotes'>{notes}</div>";
            //var topDiv = new TagBuilder("div");
            //topDiv.AddCssClass("form-group");
            //if (isModal) topDiv.AddCssClass("beModalFormGroup");

            //topDiv.InnerHtml += html.Label(labelText, new { @class = labelClasses });
            //topDiv.InnerHtml += inDiv;

            //var x = topDiv.ToString();
            return MvcHtmlString.Create(inDiv.ToString());
        }
    }

}