using NUnit.Framework;

namespace Octet.Lib.UnitTests
{
    [TestFixture]
    public class StringExtenionsTest
    {
        //
        // Note: not invoking as extension method to avoid confusion with native System.String.IsNullOrEmpty
        // 

        [Test]
        public void When_Input_Null_Returns_True()
        {
            Assert.IsTrue(StringExtensions.IsNullOrEmpty(null));
        }

        [Test]
        public void When_Input_Empty_Returns_True()
        {
            Assert.IsTrue(StringExtensions.IsNullOrEmpty(""));
        }

        [Test]
        public void When_Input_Is_Space_Returns_False()
        {
            Assert.IsFalse(StringExtensions.IsNullOrEmpty(" "));
        }

        [Test]
        public void When_Input_Non_Empty_Returns_False()
        {
            Assert.IsFalse(StringExtensions.IsNullOrEmpty("some string"));
        }
    }
}
