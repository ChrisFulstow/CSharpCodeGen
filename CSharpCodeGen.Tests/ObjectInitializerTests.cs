using NUnit.Framework;

namespace CSharpCodeGen.Tests
{
    internal class Car
    {
        public int Wheels { get; set; }
        public string Model { get; set; }
    }

    [TestFixture]
    public class ObjectInitializerTests
    {
        [Test]
        public void Initializer_Test_Object()
        {
            var car = new Car { Model = "Mini Cooper", Wheels = 4 };
            Assert.AreEqual("new Car\n{\n\tWheels = 4,\n\tModel = \"Mini Cooper\"\n}", car.InitializationCode());
        }
    }
}