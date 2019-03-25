using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentACar.Areas.Uposlenik.Controllers;

namespace RentACar_UnitTest
{
    [TestClass]
    public class rentcar_test
    {
        [TestMethod]
        public void DashboarUposlenikTest()
        {
            DashboardController dc = new DashboardController();
            Assert.IsNotNull(dc.Index());
        }
    }
}
