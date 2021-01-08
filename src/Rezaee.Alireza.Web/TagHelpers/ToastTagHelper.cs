using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.TagHelpers
{
    public enum ToastType
    {
        Error,
        Warning,
        Success,
        Information
    }

    [HtmlTargetElement("toast")]
    public class ToastTagHelper : TagHelper
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public ToastType Type { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", $"toast text-white {PrintColor(Type)}");
            output.Attributes.SetAttribute("data-bs-autohide", "false");

            output.Content.SetHtmlContent("<div class=\"toast-header\">\n"
                + PrintIcon(Type)
                + $"<strong class=\"ms-auto\">{Title}</strong>"
                + $"<small class=\"text-muted\" data-time=\"{DateTime.Now}\">الآن</small>"
                + "<button type=\"button\" class=\"btn-close ms-0 me-2\" data-bs-dismiss=\"toast\" aria-label=\"Close\"></button>"
                + $"</div>\n<div class=\"toast-body\">\n{Message}\n</div>");
        }

        private static string PrintIcon(ToastType type) => type switch
        {
            ToastType.Error => "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-x-circle-fill ms-2 text-danger\" viewBox=\"0 0 16 16\"><path d=\"M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293 5.354 4.646z\"/></svg>",
            ToastType.Warning => "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-info-circle-fill ms-2 text-warning\" viewBox=\"0 0 16 16\"><path d=\"M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412l-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z\"/></svg>",
            ToastType.Success => "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-check2-circle ms-2 text-success\" viewBox=\"0 0 16 16\"><path d=\"M2.5 8a5.5 5.5 0 0 1 8.25-4.764.5.5 0 0 0 .5-.866A6.5 6.5 0 1 0 14.5 8a.5.5 0 0 0-1 0 5.5 5.5 0 1 1-11 0z\"/><path d=\"M15.354 3.354a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0l7-7z\"/></svg>",
            ToastType.Information => "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-info-circle-fill ms-2\" viewBox=\"0 0 16 16\"><path d=\"M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412l-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z\"/></svg>",
            _ => throw new NotImplementedException()
        };

        private static string PrintColor(ToastType type) => type switch
        {
            ToastType.Error => "bg-danger",
            ToastType.Warning => "bg-warning",
            ToastType.Success => "bg-success",
            ToastType.Information => "bg-secondary",
            _ => throw new NotImplementedException(),
        };
    }
}
