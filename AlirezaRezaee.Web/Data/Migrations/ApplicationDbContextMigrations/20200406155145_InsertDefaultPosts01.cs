using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class InsertDefaultPosts01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
               INSERT INTO[Posts]([Title], [PublishDateTime], [LatestUpdateDateTime], [Summary], [ThumbnailUrl], [UrlTitle]) VALUES
                   (N'سلام بر مهدی عج',
                   '2020-04-05',
                   NULL,
                   N'text',
                   N'/uploads/articles/mahdi.jpg',
                   N'mahdi'),

                   (N'چهره نورانی علی اکبر ع',
                   '2020-04-05',
                   NULL,
                   N'چهره-علی-اکبر-ع',
                   N'/uploads/articles/چهره-علی-اکبر-ع.jpg',
                   N'چهره-علی-اکبر-ع'),

                   (N'سامانه غربالگری کرونا - وزارت بهداشت',
                   '2020-04-05',
                   NULL,
                   N'برای ورود و ثبت اطلاعات در سامانه غربالگری کرونا به نشانی salamat.gov.ir اینجا کلیک کنید.',
                   N'/uploads/shares/salamat.gov.ir.png',
                   N'سامانه-غربالگری-کرونا'),

				    (N'بیانات مقام معظم رهبری در سخنرانی نوروزی ۱۳۹۹',
                   '2020-03-22',
                   NULL,
                   N'برای مشاهده مشروح بیانات اینجا کلیک کنید.',
                   N'/uploads/shares/سخنرانی-نوروزی-رهبری-1399.jpg',
                   N'بیانات-نوروزی-مقام-معظم-رهبری-1399')

                GO

               INSERT INTO[Articles]([HtmlContent], [SourcesUrl], [CoverUrl], [PostId]) VALUES
                   (N'<p>🌹🌸🌹<br>امام زمان (عج):</p><p>اراده ى حتمى خداوند بر این قرار گرفته است که ـ دیر یا زود ـ پایان حق، پیروزى، و پایان باطل، نابودى باشد.</p><p>📚بحارالأنوار، ج۵۳، ص۱۹۳</p>',
                   N'http://atrequran.ir/%d8%b3%d9%84%d8%a7%d9%85-%d8%a8%d8%b1-%d9%85%d9%87%d8%af%db%8c-%d8%b9%d8%ac/',
                   N'/uploads/articles/mahdi.jpg',
                   1),

                   (N'<p>ای که بر روشنای چهره ی خود نور پیغمبر سحر داری<br>نوری از آفتاب روشن تر رویی از ماه خوبتر داری</p><p>تو کدامین گلی که دیدن تو صلواتی محمدی دارد<br>چقدر بر بهشت چهره ی خود رنگ و بوی پیامبر داری</p><p>هجرتت از مدینه شد آغاز کربلا شاهد سلوک تو بود<br>کوفه چون شام ماند مبهوتت تا کجاها سر سفر داری</p><p>باوری سرخ بود و جاری شد اولسنا علی الحق از لب تو<br>چه غرور آفرین و بشکوه است مقصدی که تو در نظر داری</p><p>با لب تشنه بودی و می سوخت در تف کربلا پر جبریل<br>وقت معراج شد چه معراجی ای که از زخم بال و پر داری</p><p>از میان تمام اهل جهان عرش پایین پا نصیب تو شد<br>عشق می داند و جنون که چقدر شوق پابوسی پدر داری</p><p>شوق پابوسی تو را داریم حسرت آن ضریح شش گوشه<br>گوشه چشمی  عنایتی لطفی تو که از حال ما خبر داری</p><p>در مدیح تو از مدایح تو یا علی هر چه بیشتر گفتیم<br>با نگاهی پر از عطش دیدیم حسن ناگفته بیشتر داری</p><p>سیدمحمد جواد شرافت</p>',
                   N'http://atrequran.ir/%d8%b3%d9%84%d8%a7%d9%85-%d8%a8%d8%b1-%d9%85%d9%87%d8%af%db%8c-%d8%b9%d8%ac/',
                   N'/uploads/articles/چهره-علی-اکبر-ع.jpg',
                   2)
               GO

               INSERT INTO[Shares]([RedirectToUrl], [PostId]) VALUES
                   (N'http://salamat.gov.ir', 3),
                   (N'http://farsi.khamenei.ir/news-content?id=45225', 4)
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Articles] GO DELETE FROM [Shares] GO DELETE FROM [Posts]");
        }
    }
}