using NUnit.Framework;

namespace CSharpCodeGen.Tests
{
    [TestFixture]
    public class PrimitiveInitializerTests
    {
        [Test]
        public void Initializer_Test_String()
        {
            const string str = "Hello, World!";
            Assert.AreEqual("\"Hello, World!\"", str.InitializationCode());
        }

        [Test]
        public void Initializer_Test_Int32()
        {
            const int num = 12345;
            Assert.AreEqual("12345", num.InitializationCode());
        }

        [Test]
        public void Initializer_Test_Double()
        {
            const double num = 3.14159;
            Assert.AreEqual("3.14159D", num.InitializationCode());
        }

        [Test]
        public void Initializer_Test_Decimal()
        {
            const decimal num = 3.14159M;
            Assert.AreEqual("3.14159M", num.InitializationCode());
        }

        [Test]
        public void Initializer_Test_Bool()
        {
            const bool boolean = true;
            Assert.AreEqual("true", boolean.InitializationCode());
        }
    }
}