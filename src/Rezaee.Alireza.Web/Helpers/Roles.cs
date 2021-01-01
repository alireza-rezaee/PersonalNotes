using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Helpers
{
    public static class Roles
    {
        // ~/Controllers
        #region PostController (Counts: 9)
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

        #region TagsController (Counts: 3)
        public const string TagCreate = "افزودن برچسب";
        public const string TagEdit = "ویرایش برچسب";
        public const string TagDelete = "حذف برچسب";
        #endregion

        #region PinsController (Counts: 1)
        public const string Pin = "سنجاق مطلب (عادی)";
        #endregion

        #region PosterPinsController (Counts: 1)
        public const string PosterPin = "سنجاق مطلب (پوستر)";
        #endregion

        #region PagesController (Counts: 3)
        public const string PageCreate = "افزودن برگه";
        public const string PageEdit = "ویرایش برگه";
        public const string PageDelete = "حذف برگه";
        #endregion

        #region LinksController (Counts: 3)
        public const string LinkCreate = "افزودن پیوند";
        public const string LinkEdit = "ویرایش پیوند";
        public const string LinkDelete = "حذف پیوند";
        #endregion

        #region BlocksController (Counts: 4)
        public const string BlockCreate = "افزودن بلاک";
        public const string BlockEdit = "ویرایش بلاک";
        public const string BlockVisibility = "نمایش یا عدم نمایش بلاک";
        public const string BlockDelete = "حذف بلاک";
        #endregion

        // ~/Areas/Admin
        #region RolesController (Counts: 4)
        public const string RolesList = "مشاهده نقش ها";
        public const string RoleCreate = "افزودن نقش";
        public const string RoleEdit = "ویرایش نقش";
        public const string RoleDelete = "حذف نقش";
        #endregion

        // ~/Areas/Admin
        #region UsersController (Counts: 5)
        public const string UsersList = "مشاهده کاربران";
        public const string UserDetails = "مشاهده مشخصات کاربران";
        public const string UserCreate = "افزودن کاربر";
        public const string UserEdit = "ویرایش کاربر";
        public const string UserDelete = "حذف کاربر";
        #endregion

        // ~/Areas/Admin
        #region UserRolesController (Counts: 3)
        public const string UserRolesList = "مشاهده انتسابات";
        public const string UserRoleAssign = "انتساب نقش به کاربر";
        public const string UserRoleUnassign = "سلب نقش از کاربر";
        #endregion

        #region MessagesController (Counts: 3)
        public const string MessageDelete = "حذف پیام";
        public const string MessagesList = "فهرست پیام ها";
        public const string MessagesSetReadOrNot = "فهرست پیام ها";
        #endregion

        // ~/Areas/Admin
        #region LogsController (Counts: 1)
        public const string LogsList = "مشاهده تاریخچه لاگ";
        #endregion

        // ~/Areas/Admin
        #region FilesController (Counts: 4)
        public const string FilesList = "فهرست فایل‌ها";
        public const string FileUpload = "بارگذاری فایل";
        public const string FileRemove = "حذف فایل";
        public const string FileRename = "تغییر نام فایل";
        #endregion

        // ~/Areas/Admin
        #region PersonalizationController (Counts: 1)
        public const string Personalize = "شخصی‌سازی سایت";
        #endregion
    }
}
