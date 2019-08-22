using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Accusoft.PrizmDoc.Tests
{
  [TestClass]
  public class Constructor_Tests
  {
    [TestMethod]
    public void Can_create_an_instance_for_PDC()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", "MY_API_KEY");

      Assert.AreEqual("https://api.accusoft.com/", helper.client.BaseAddress.ToString());
      Assert.AreEqual(true, helper.client.DefaultRequestHeaders.Contains("Acs-Api-Key"));
      Assert.AreEqual("MY_API_KEY", helper.client.DefaultRequestHeaders.GetValues("Acs-Api-Key").Single());
    }

    [TestMethod]
    public void Can_create_an_instance_for_self_hosted()
    {
      var helper = new DocumentProcessingHelper("http://mylocalinstance:18681");

      Assert.AreEqual("http://mylocalinstance:18681/", helper.client.BaseAddress.ToString());
      Assert.AreEqual(false, helper.client.DefaultRequestHeaders.Contains("Acs-Api-Key"));
    }
  }
}
