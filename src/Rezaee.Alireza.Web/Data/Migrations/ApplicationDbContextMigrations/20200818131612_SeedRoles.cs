using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Roles for [Post]
            migrationBuilder.Sql(@"INSERT INTO [AspNetRoles](
                [Id],
                [Name],
                [NormalizedName],
                [ConcurrencyStamp])

                VALUES
                -- Posts Controller
                (N'000baeb7-6636-45bc-86f2-985d5502df1f',
                N'حذف مطلب از نوع نشانه‌دار',
                N'حذف مطلب از نوع نشانه‌دار',
                N'ddc59627-8a7d-4830-8984-f3bd5c752adb'),

                (N'19269bc5-9aae-498b-999d-a8c83a23f432',
                N'ویرایش مطلب از نوع نشانه‌دار',
                N'ویرایش مطلب از نوع نشانه‌دار',
                N'91667442-c212-437d-875d-61d6c216d29e'),

                (N'49ceac2d-a75a-499d-b036-5481ae1427bc',
                N'حذف مطلب از نوع مقاله',
                N'حذف مطلب از نوع مقاله',
                N'066b9f99-11b2-4da7-aa2d-8fbb5c16972f'),

                (N'6ca4e67f-2e4b-448a-87a3-94a7fe9f1046',
                N'ویرایش مطلب از نوع بازنشر',
                N'ویرایش مطلب از نوع بازنشر',
                N'780daee2-3c61-45cd-9bc5-5b21dcc02fda'),

                (N'730e060d-0eb6-47fe-b6ef-ba0012f04792',
                N'حذف مطلب از نوع بازنشر',
                N'حذف مطلب از نوع بازنشر',
                N'd6fe3209-9553-408d-b351-8a2f8e753378'),

                (N'7f62f7be-4d6f-4609-87c6-0a2984e53aca',
                N'افزودن مطلب از نوع بازنشر',
                N'افزودن مطلب از نوع بازنشر',
                N'91d196c1-9563-4025-ae93-1598e91a7e0e'),

                (N'bc29c02d-693b-49dc-be29-9ecb393d7626',
                N'افزودن مطلب از نوع نشانه‌دار',
                N'افزودن مطلب از نوع نشانه‌دار',
                N'fb308ea0-cbd8-44f3-984f-27907e6d51f1'),

                (N'f0094989-5a82-466f-9b56-c22a0ef6c527',
                N'ویرایش مطلب از نوع مقاله',
                N'ویرایش مطلب از نوع مقاله',
                N'b64a0820-30a1-49c0-a051-4e3dc585b639'),

                (N'fa1a03d5-5c25-4077-a9b2-df583404c19c',
                N'افزودن مطلب از نوع مقاله',
                N'افزودن مطلب از نوع مقاله',
                N'ed870d83-c89d-435d-9fb5-effaa2f0452e'),



                -- Tags Controller
                (N'340bc7b5-6037-498e-9497-d62325308de3',
                N'افزودن برچسب',
                N'افزودن برچسب',
                N'10be15ef-b080-4514-bf65-d9ef977877d1'),
                
                (N'ae4400fb-7423-4771-94e8-793b4181d25c',
                N'ویرایش برچسب',
                N'ویرایش برچسب',
                N'f2c7559b-c326-47d0-a035-46ca68db46f8'),

                (N'9709613b-5afb-4bb9-a4e5-c117321ea53e',
                N'حذف برچسب',
                N'حذف برچسب',
                N'2e94e3f9-866f-4b87-99e3-c744b5c5ee8c'),



                -- Pins Controller
                (N'e0d59a39-4723-4007-b6a3-6118d32a1748',
                N'سنجاق مطلب (عادی)',
                N'سنجاق مطلب (عادی)',
                N'022faf0a-800d-4f7c-8d54-852a685f8f65'),



                -- PosterPins Controller
                (N'47e2f5c9-81f9-41b2-b4ef-394b824585a6',
                N'سنجاق مطلب (پوستر)',
                N'سنجاق مطلب (پوستر)',
                N'd0153c5e-80b2-43eb-a86b-6e04d6b2c1b5'),



                -- Pages Controller
                (N'2165aa25-ece4-4698-8ff6-198bc93bbf23',
                N'افزودن برگه',
                N'افزودن برگه',
                N'6876cca4-3f0e-4a9b-b311-bb0b8dc0a61d'),

                (N'f34f3ad1-6cee-4f1e-a7a7-a8a28566e566',
                N'ویرایش برگه',
                N'ویرایش برگه',
                N'3047be39-8052-4c8b-b7ec-e8428268e76f'),

                (N'86c40a5c-3f06-4f6b-b851-04da2e1a6a23',
                N'حذف برگه',
                N'حذف برگه',
                N'b83fce68-5a14-4043-a38e-7ce78b69380a'),



                -- Links Controller
                (N'ed995fbf-106c-4899-9806-0bc93a63a095',
                N'ویرایش پیوند',
                N'ویرایش پیوند',
                N'915a2253-1322-4986-8ef0-1519cb273c71'),

                (N'9516612b-f9c7-4a95-ae41-a4d502c420e4',
                 N'افزودن پیوند',
                N'افزودن پیوند',
                 N'1c73badf-d455-4b5e-bc29-7547abc2ac95'),

                (N'b0093da7-c643-4ed2-8835-47f18340ca0b',
                N'حذف پیوند',
                N'حذف پیوند',
                N'921f5a55-aa88-45c1-878a-72b0e8e76a2b'),



                -- Blocks Controller
                (N'4ff698ed-76c6-4aa6-83c9-c52394a0980a',
                N'افزودن بلاک',
                N'افزودن بلاک',
                N'960cec12-9035-40d1-b345-87dd03b0ff76'),

                (N'3d645f3b-44ea-4a94-93cc-1450f4dd688a',
                N'ویرایش بلاک',
                N'ویرایش بلاک',
                N'cc24343a-50a6-4139-858c-0cdf265970f8'),

                (N'870e9263-36b0-4dfc-b20a-e262a2d2ac81',
                N'نمایش یا عدم نمایش بلاک',
                N'نمایش یا عدم نمایش بلاک',
                N'260af390-e573-4148-9bd6-6e7a59a0bbba'),

                (N'f863b641-6f44-49c0-926f-778cb0996448',
                N'حذف بلاک',
                N'حذف بلاک',
                N'22cfc5ba-1f44-42c3-95bf-9538350847a6'),



                -- Roles Controller
                (N'5bdc43e5-1fc0-472a-b452-8245c2a7225c',
                N'مشاهده نقش ها',
                N'مشاهده نقش ها',
                N'334457e9-d10c-420c-89df-61e26c5bd2a3'),

                (N'f414a5a5-3de3-4669-b3de-2717cde466b3',
                N'افزودن نقش',
                N'افزودن نقش',
                N'9c57ffcc-d4c0-4aaa-95e0-c99281fe1f9f'),

                (N'20937e9c-eb37-4a90-9476-675f6e8347ca',
                N'ویرایش نقش',
                N'ویرایش نقش',
                N'45e77c21-7867-40aa-b0b3-83aba8712d41'),

                (N'dcf1dc2f-bbfd-4dec-b036-d3edea85117d',
                N'حذف نقش',
                N'حذف نقش',
                N'2bd4833a-4051-42e2-809b-f093386632e5'),



                -- Users Controller
                (N'84877561-6566-4c95-89fa-2e1a9b5099c1',
                N'مشاهده کاربران',
                N'مشاهده کاربران',
                N'376793d8-8411-43ad-b224-fdc3b88dfa22'),

                (N'a19e2e74-0a09-4ce5-a03c-725ed29b6545',
                N'افزودن کاربر',
                N'افزودن کاربر',
                N'9a333e7d-ecf4-4007-8991-6fbb11d46d3b'),

                (N'b1158d32-7b4f-4ea8-a89c-fd29f7629580',
                N'ویرایش کاربر',
                N'ویرایش کاربر',
                N'c2243b31-3c2c-44cf-96fa-d3a3c699c437'),

                (N'85540f71-77ca-4c6e-a2ac-331f3666e3a5',
                N'حذف کاربر',
                N'حذف کاربر',
                N'5ff6955f-7d36-4127-9382-b4171f170b8d'),



                -- UserRoles Controller
                (N'42dde1b9-d1a5-492d-907e-e78026b51a1e',
                N'مشاهده انتسابات',
                N'مشاهده انتسابات',
                N'669023c0-e24d-4b7a-81ed-9cac88b0e355'),

                (N'bccfce2f-0ebc-45cb-97f4-9d13e8f91019',
                N'انتساب نقش به کاربر',
                N'انتساب نقش به کاربر',
                N'7f6cb2f4-e8e4-4fd7-97d0-fff7c602f487'),

                (N'e4f0eea5-9b1a-4fe2-b593-8d56bac9f8c4',
                N'سلب نقش از کاربر',
                N'سلب نقش از کاربر',
                N'1bd31133-da5e-4427-b14d-7ec567f78bcc')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM [AspNetRoles] 
                WHERE

                -- Posts Controller
                Id = N'000baeb7-6636-45bc-86f2-985d5502df1f' OR
                Id = N'19269bc5-9aae-498b-999d-a8c83a23f432' OR
                Id = N'49ceac2d-a75a-499d-b036-5481ae1427bc' OR
                Id = N'6ca4e67f-2e4b-448a-87a3-94a7fe9f1046' OR
                Id = N'730e060d-0eb6-47fe-b6ef-ba0012f04792' OR
                Id = N'7f62f7be-4d6f-4609-87c6-0a2984e53aca' OR
                Id = N'bc29c02d-693b-49dc-be29-9ecb393d7626' OR
                Id = N'f0094989-5a82-466f-9b56-c22a0ef6c527' OR
                Id = N'fa1a03d5-5c25-4077-a9b2-df583404c19c' OR

                -- Tags Controller
                Id = N'340bc7b5-6037-498e-9497-d62325308de3' OR
                Id = N'ae4400fb-7423-4771-94e8-793b4181d25c' OR
                Id = N'9709613b-5afb-4bb9-a4e5-c117321ea53e' OR

                -- Pins Controller
                Id = N'e0d59a39-4723-4007-b6a3-6118d32a1748' OR

                -- PosterPins Controller
                Id = N'47e2f5c9-81f9-41b2-b4ef-394b824585a6' OR

                -- Pages Controller
                Id = N'2165aa25-ece4-4698-8ff6-198bc93bbf23' OR
                Id = N'f34f3ad1-6cee-4f1e-a7a7-a8a28566e566' OR
                Id = N'86c40a5c-3f06-4f6b-b851-04da2e1a6a23' OR

                -- Links Controller
                Id = N'ed995fbf-106c-4899-9806-0bc93a63a095' OR
                Id = N'9516612b-f9c7-4a95-ae41-a4d502c420e4' OR
                Id = N'b0093da7-c643-4ed2-8835-47f18340ca0b' OR

                -- Blocks Controller
                Id = N'4ff698ed-76c6-4aa6-83c9-c52394a0980a' OR
                Id = N'3d645f3b-44ea-4a94-93cc-1450f4dd688a' OR
                Id = N'870e9263-36b0-4dfc-b20a-e262a2d2ac81' OR
                Id = N'f863b641-6f44-49c0-926f-778cb0996448' OR

                -- Roles Controller
                Id = N'5bdc43e5-1fc0-472a-b452-8245c2a7225c' OR
                Id = N'f414a5a5-3de3-4669-b3de-2717cde466b3' OR
                Id = N'20937e9c-eb37-4a90-9476-675f6e8347ca' OR
                Id = N'dcf1dc2f-bbfd-4dec-b036-d3edea85117d' OR

                -- Users Controller
                Id = N'84877561-6566-4c95-89fa-2e1a9b5099c1' OR
                Id = N'a19e2e74-0a09-4ce5-a03c-725ed29b6545' OR
                Id = N'b1158d32-7b4f-4ea8-a89c-fd29f7629580' OR
                Id = N'85540f71-77ca-4c6e-a2ac-331f3666e3a5' OR

                -- UserRoles Controller
                Id = N'42dde1b9-d1a5-492d-907e-e78026b51a1e' OR
                Id = N'bccfce2f-0ebc-45cb-97f4-9d13e8f91019' OR
                Id = N'e4f0eea5-9b1a-4fe2-b593-8d56bac9f8c4'");
        }
    }
}
