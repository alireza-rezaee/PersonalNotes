using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class AssignRolesToAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Assign Roles to Admin (or vice versa)
            migrationBuilder.Sql(@"
                INSERT INTO [AspNetUserRoles](
                    [UserId], 
                    [RoleId])

                    VALUES

                    -- Posts Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'000baeb7-6636-45bc-86f2-985d5502df1f'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'19269bc5-9aae-498b-999d-a8c83a23f432'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'49ceac2d-a75a-499d-b036-5481ae1427bc'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'6ca4e67f-2e4b-448a-87a3-94a7fe9f1046'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'730e060d-0eb6-47fe-b6ef-ba0012f04792'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'7f62f7be-4d6f-4609-87c6-0a2984e53aca'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'bc29c02d-693b-49dc-be29-9ecb393d7626'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'f0094989-5a82-466f-9b56-c22a0ef6c527'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'fa1a03d5-5c25-4077-a9b2-df583404c19c'),

                    -- Tags Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'340bc7b5-6037-498e-9497-d62325308de3'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'ae4400fb-7423-4771-94e8-793b4181d25c'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'9709613b-5afb-4bb9-a4e5-c117321ea53e'),

                    -- Pins Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'e0d59a39-4723-4007-b6a3-6118d32a1748'),

                    -- PosterPins Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'47e2f5c9-81f9-41b2-b4ef-394b824585a6'),

                    -- Pages Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'2165aa25-ece4-4698-8ff6-198bc93bbf23'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'f34f3ad1-6cee-4f1e-a7a7-a8a28566e566'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'86c40a5c-3f06-4f6b-b851-04da2e1a6a23'),

                    -- Links Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'ed995fbf-106c-4899-9806-0bc93a63a095'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'9516612b-f9c7-4a95-ae41-a4d502c420e4'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'b0093da7-c643-4ed2-8835-47f18340ca0b'),

                    -- Blocks Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'4ff698ed-76c6-4aa6-83c9-c52394a0980a'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'3d645f3b-44ea-4a94-93cc-1450f4dd688a'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'870e9263-36b0-4dfc-b20a-e262a2d2ac81'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'f863b641-6f44-49c0-926f-778cb0996448'),

                    -- Roles Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'5bdc43e5-1fc0-472a-b452-8245c2a7225c'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'f414a5a5-3de3-4669-b3de-2717cde466b3'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'20937e9c-eb37-4a90-9476-675f6e8347ca'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'dcf1dc2f-bbfd-4dec-b036-d3edea85117d'),

                    -- Users Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'84877561-6566-4c95-89fa-2e1a9b5099c1'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'a19e2e74-0a09-4ce5-a03c-725ed29b6545'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'b1158d32-7b4f-4ea8-a89c-fd29f7629580'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'85540f71-77ca-4c6e-a2ac-331f3666e3a5'),

                    -- UserRoles Controller
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'42dde1b9-d1a5-492d-907e-e78026b51a1e'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'bccfce2f-0ebc-45cb-97f4-9d13e8f91019'),
                    (N'c3ad1d11-31fc-4079-94d4-e74ab1273112', N'e4f0eea5-9b1a-4fe2-b593-8d56bac9f8c4')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [AspNetUserRoles]

                WHERE

                 -- Posts Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'000baeb7-6636-45bc-86f2-985d5502df1f') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'19269bc5-9aae-498b-999d-a8c83a23f432') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'49ceac2d-a75a-499d-b036-5481ae1427bc') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'6ca4e67f-2e4b-448a-87a3-94a7fe9f1046') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'730e060d-0eb6-47fe-b6ef-ba0012f04792') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'7f62f7be-4d6f-4609-87c6-0a2984e53aca') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'bc29c02d-693b-49dc-be29-9ecb393d7626') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'f0094989-5a82-466f-9b56-c22a0ef6c527') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'fa1a03d5-5c25-4077-a9b2-df583404c19c') OR

                 -- Tags Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'340bc7b5-6037-498e-9497-d62325308de3') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'ae4400fb-7423-4771-94e8-793b4181d25c') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'9709613b-5afb-4bb9-a4e5-c117321ea53e') OR

                 -- Pins Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'e0d59a39-4723-4007-b6a3-6118d32a1748') OR

                 -- PosterPins Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'47e2f5c9-81f9-41b2-b4ef-394b824585a6') OR

                 -- Pages Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'2165aa25-ece4-4698-8ff6-198bc93bbf23') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'f34f3ad1-6cee-4f1e-a7a7-a8a28566e566') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'86c40a5c-3f06-4f6b-b851-04da2e1a6a23') OR

                 -- Links Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'ed995fbf-106c-4899-9806-0bc93a63a095') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'9516612b-f9c7-4a95-ae41-a4d502c420e4') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'b0093da7-c643-4ed2-8835-47f18340ca0b') OR

                -- Blocks Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'4ff698ed-76c6-4aa6-83c9-c52394a0980a') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'3d645f3b-44ea-4a94-93cc-1450f4dd688a') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'870e9263-36b0-4dfc-b20a-e262a2d2ac81') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'f863b641-6f44-49c0-926f-778cb0996448') OR

                -- Roles Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'5bdc43e5-1fc0-472a-b452-8245c2a7225c') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'f414a5a5-3de3-4669-b3de-2717cde466b3') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'20937e9c-eb37-4a90-9476-675f6e8347ca') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'dcf1dc2f-bbfd-4dec-b036-d3edea85117d') OR

                -- Users Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'84877561-6566-4c95-89fa-2e1a9b5099c1') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'a19e2e74-0a09-4ce5-a03c-725ed29b6545') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'b1158d32-7b4f-4ea8-a89c-fd29f7629580') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'85540f71-77ca-4c6e-a2ac-331f3666e3a5') OR

                -- UserRoles Controller
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'42dde1b9-d1a5-492d-907e-e78026b51a1e') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'bccfce2f-0ebc-45cb-97f4-9d13e8f91019') OR
                ([UserId] = N'c3ad1d11-31fc-4079-94d4-e74ab1273112' AND [RoleId] = N'e4f0eea5-9b1a-4fe2-b593-8d56bac9f8c4')");
        }
    }
}
