using NUnit.Framework;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //Assert.Fail();
            string[] p = { "NFDtoNFC.exe", 
                "--directory", @"C:\Users\AK\Desktop\git\autobiography", 
                "--content", "true",
                "--filter", ".json", 
                "--recursive", "true",
                "--normalize", "nfc" };
            NFDtoNFC.Program.EntryPoint(p);
        }
    }
}