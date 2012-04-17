using System.Collections.Generic;
using NUnit.Framework;

namespace CSharpCodeGen.Tests
{
    [TestFixture]
    public class CollectionInitializerTests
    {
        [Test]
        public void Initializer_Test_Int32Array()
        {
            var intArray = new[] { 1, 2, 3, 4, 5 };
            Assert.AreEqual("new Int32[] { 1, 2, 3, 4, 5 }", intArray.InitializationCode());
        }

        [Test]
        public void Initializer_Test_ObjectArray()
        {
            var objArray = new[]
            {
                new Car { Model = "Mini Cooper", Wheels = 4 },
                new Car { Model = "Robin Reliant", Wheels = 3 },
            };
            Assert.AreEqual("new Car[] { new Car\n{\n\tWheels = 4,\n\tModel = \"Mini Cooper\"\n}, new Car\n{\n\tWheels = 3,\n\tModel = \"Robin Reliant\"\n} }", objArray.InitializationCode());
        }

        [Test]
        public void Initializer_Test_Dictionary()
        {
            var dictionary = new Dictionary<string, int>
            {
                { "Ford Focus", 1998 },
                { "Land Rover", 1978 }
            };
            Assert.AreEqual("new Dictionary<String,Int32>\r\n{\r\n\t{ \"Ford Focus\", 1998 },\n\t{ \"Land Rover\", 1978 }\r\n}", dictionary.InitializationCode());
        }
    }
}