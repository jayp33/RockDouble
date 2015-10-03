using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RockDouble
{
    [TestClass]
    public class MyTestClass
    {
        [TestMethod]
        public void RocklandWebsiteShouldContainROCKLAND_offline()
        {
            var html = new HtmlAgilityPack.HtmlDocument();
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\TestData";
            html.Load(path + @"\ROCKLAND - Mach an und laut!.html");
            Assert.IsTrue(html.DocumentNode.ChildNodes["html"].InnerText.Contains("ROCKLAND"));
        }
    }
}
