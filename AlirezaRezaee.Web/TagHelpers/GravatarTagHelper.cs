using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.TagHelpers
{
    public enum GravatarMode
    {
        NotFound,
        mp, // previously Known as 'mm'
        identicon,
        monsterid,
        wavatar,
        retro,
        robohash,
        blank
    }

    public enum GravatarRating
    {
        g,
        pg,
        r,
        x
    }


    [HtmlTargetElement("img-gravatar")]
    public class GravatarTagHelper : TagHelper
    {
        //See: https://en.gravatar.com/site/implement/images/
        [HtmlAttributeName("gravatar-email")]
        public string Email { get; set; }
        [HtmlAttributeName("gravatar-mode")]
        public GravatarMode Mode { get; set; } = GravatarMode.mp;
        [HtmlAttributeName("gravatar-rating")]
        public GravatarRating Rating { get; set; } = GravatarRating.g;
        [HtmlAttributeName("gravatar-size")]
        public int Size { get; set; } = 50;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.ASCII.GetBytes(Email));
            var hash = BitConverter.ToString(result).Replace("-", "").ToLower();
            var url = $"http://gravatar.com/avatar/{hash}";
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("s", Size.ToString());
            queryBuilder.Add("d", GetModeValue(Mode));
            queryBuilder.Add("r", Rating.ToString());
            url += queryBuilder.ToQueryString();
            output.TagName = "img";
            output.Attributes.SetAttribute("src", url);
            output.TagMode = TagMode.StartTagOnly;
        }

        private static string GetModeValue(GravatarMode mode) => (mode == GravatarMode.NotFound) ? "404" : mode.ToString().ToLower();
    }
}
