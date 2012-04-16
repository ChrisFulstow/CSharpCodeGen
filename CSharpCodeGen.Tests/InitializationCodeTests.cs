using NUnit.Framework;

namespace CSharpCodeGen.Tests
{
    [TestFixture]
    public class InitializationCodeTests
    {
        [Test]
        public void Initializer_Test_String()
        {
            // arrange
            const string str = "Hello, World!";

            // act
            var code = str.InitializationCode();

            // assert
            Assert.AreEqual("\"Hello, World!\"", code);
        }
    }
}