using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AlirezaRezaee.PersonalNotes.WeblogApp.Extensions;

namespace AlirezaRezaee.UnitTests.PersonalNotes.WeblogApp.Extensions
{
    public class IntExtensionsTest
    {
        #region PersianToEnglish
        [Fact]
        public void EnglishToPersianShould_ReturnPersianizedString_WhenInputContainsOnlyEnglishChars()
            => Assert.True(IntExtensions.EnglishToPersian(1234567890) == "۱۲۳۴۵۶۷۸۹۰");
        #endregion

        #region ToGuid
        [Fact]
        public void ToGuidShould_ReturnGuidedInteger_WhenInputIsZero()
            => Assert.True(IntExtensions.ToGuid(0).ToString() == "00000000-0000-0000-0000-000000000000");

        [Fact]
        public void ToGuidShould_ReturnGuidedInteger()
            => Assert.True(IntExtensions.ToGuid(1234567890).ToString() == "499602d2-0000-0000-0000-000000000000");
        #endregion
    }
}
