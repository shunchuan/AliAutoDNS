using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CaoSCAutoDNSTest
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestMain()
        {
            CaoSCAutoDNS.Methods.ToDo toDo = new CaoSCAutoDNS.Methods.ToDo();
            toDo.Run();
        }
    }
}
