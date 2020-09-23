using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Helpers
{
    public static class Roles
    {
        // ~/Controllers
        #region PostController
        //Posts Controller

        //[Create]
        public const string PostCreateArticle = "افزودن مطلب از نوع مقاله";
        public const string PostCreateShare = "افزودن مطلب از نوع بازنشر";
        public const string PostCreateMarkdown = "افزودن مطلب از نوع نشانه‌دار";

        //[Edit]
        public const string PostEditArticle = "ویرایش مطلب از نوع مقاله";
        public const string PostEditShare = "ویرایش مطلب از نوع بازنشر";
        public const string PostEditMarkdown = "ویرایش مطلب از نوع نشانه‌دار";

        //[Delete]
        public const string PostDeleteArticle = "حذف مطلب از نوع مقاله";
        public const string PostDeleteShare = "حذف مطلب از نوع بازنشر";
        public const string PostDeleteMarkdown = "حذف مطلب از نوع نشانه‌دار";
        #endregion

        #region TagsController
        public const string TagCreate = "افزودن برچسب";
        public const string TagEdit = "ویرایش برچسب";
        public const string TagDelete = "حذف برچسب";
        #endregion

        #region PinsController
        public const string Pin = "سنجاق مطلب (عادی)";
        #endregion

        #region PosterPinsController
        public const string PosterPin = "سنجاق مطلب (پوستر)";
        #endregion

        #region PagesController
        public const string PageCreate = "افزودن برگه";
        public const string PageEdit = "ویرایش برگه";
        public const string PageDelete = "حذف برگه";
        #endregion

        #region LinksController
        public const string LinkCreate = "افزودن پیوند";
        public const string LinkEdit = "ویرایش پیوند";
        public const string LinkDelete = "حذف پیوند";
        #endregion

        #region BlocksController
        public const string BlockCreate = "افزودن بلاک";
        public const string BlockEdit = "ویرایش بلاک";
        public const string BlockVisibility = "نمایش یا عدم نمایش بلاک";
        public const string BlockDelete = "حذف بلاک";
        #endregion

        // ~/Areas/Admin/Controllers
        #region RolesController
        public const string RolesList = "مشاهده نقش ها";
        public const string RoleCreate = "افزودن نقش";
        public const string RoleEdit = "ویرایش نقش";
        public const string RoleDelete = "حذف نقش";
        #endregion

        #region UsersController
        public const string UsersList = "مشاهده کاربران";
        public const string UserDetails = "مشاهده مشخصات کاربران";
        public const string UserCreate = "افزودن کاربر";
        public const string UserEdit = "ویرایش کاربر";
        public const string UserDelete = "حذف کاربر";
        #endregion

        #region UserRolesController
        public const string UserRolesList = "مشاهده انتسابات";
        public const string UserRoleAssign = "انتساب نقش به کاربر";
        public const string UserRoleUnassign = "سلب نقش از کاربر";
        #endregion
    }
}
