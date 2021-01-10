using AlirezaRezaee.PersonalNotes.WeblogApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AlirezaRezaee.UnitTests.PersonalNotes.WeblogApp.Extensions
{
    public class StringExtensionsTest
    {
        #region EnglishNumberToPersian
        [Fact]
        public void EnglishNumberToPersianShould_ReturnPersianizedString_WhenInputContainsOnlyEnglishChars()
            => Assert.True(StringExtensions.EnglishNumberToPersian("1234567890") == "۱۲۳۴۵۶۷۸۹۰");

        [Fact]
        public void EnglishNumberToPersianShould_ReturnPersianizedString_WhenInputContainsNonAsciiChar()
            => Assert.True(StringExtensions.EnglishNumberToPersian("123 漢字 890") == "۱۲۳ 漢字 ۸۹۰");

        [Fact]
        public void EnglishNumberToPersianShould_ReturnEmpty_IfInputIsEmpty()
            => Assert.True(StringExtensions.EnglishNumberToPersian(string.Empty) == string.Empty);

        [Fact]
        public void EnglishNumberToPersianShould_ThrowArgumentNullException_IfInputIsNull()
            => Assert.Throws<ArgumentNullException>(() => StringExtensions.EnglishNumberToPersian(null));
        #endregion

        #region PersianNumberToEnglish
        [Fact]
        public void PersianNumberToEnglishShould_ReturnPersianizedString_WhenInputContainsOnlyEnglishChars()
            => Assert.True(StringExtensions.PersianNumberToEnglish("۱۲۳۴۵۶۷۸۹۰") == "1234567890");

        [Fact]
        public void PersianNumberToEnglishShould_ReturnPersianizedString_WhenInputContainsNonAsciiChar()
            => Assert.True(StringExtensions.PersianNumberToEnglish("۱۲۳ سلام ۴۵۶") == "123 سلام 456");

        [Fact]
        public void PersianNumberToEnglishShould_ReturnEmpty_IfInputIsEmpty()
            => Assert.True(StringExtensions.PersianNumberToEnglish(string.Empty) == string.Empty);

        [Fact]
        public void PersianNumberToEnglishShould_ThrowArgumentNullException_IfInputIsNull()
            => Assert.Throws<ArgumentNullException>(() => StringExtensions.PersianNumberToEnglish(null));
        #endregion

        #region ControllerName
        [Fact]
        public void ControllerName_ThrowArgumentException_IfNormal()
            => Assert.True(StringExtensions.ControllerName("BooksController") == "Books");

        [Fact]
        public void ControllerName_ThrowArgumentNullException_IfInputIsNull()
            => Assert.Throws<ArgumentNullException>(() => StringExtensions.ControllerName(null));

        [Fact]
        public void ControllerName_ThrowArgumentException_IfInputIsEmptyString()
            => Assert.Throws<ArgumentException>(() => StringExtensions.ControllerName(string.Empty));

        [Fact]
        public void ControllerName_ThrowArgumentException_IfInputNameDoesNotHaveController()
            => Assert.Throws<ArgumentException>(() => StringExtensions.ControllerName("SomeStringHere"));

        [Fact]
        public void ControllerName_ThrowArgumentException_IfInputNameCouldntBeClassName()
            => Assert.Throws<ArgumentException>(() => StringExtensions.ControllerName("I'am an impossible class name. Controller"));
        #endregion
    }
}
