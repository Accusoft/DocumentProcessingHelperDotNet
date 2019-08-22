using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Accusoft.PrizmDoc.Tests
{
  [TestClass]
  public class Conversion_tests
  {
    [TestInitialize]
    public void BeforeEach()
    {
      if (Directory.Exists("test-output"))
      {
        Directory.Delete("test-output", true);
      }
      if (!Directory.Exists("test-output"))
      {
        Directory.CreateDirectory("test-output");
      }
    }

    [TestCleanup]
    public void AfterEach()
    {
      if (Directory.Exists("test-output"))
      {
        Directory.Delete("test-output", true);
      }
    }

    [TestMethod]
    public async Task Can_convert_a_local_DOCX_file_to_PDF()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      var results = await helper.Convert("test.docx", OutputFormat.Pdf);

      Assert.AreEqual(1, results.Count());
      await results.First().SaveToFile("test-output/output.pdf");
      Assert.IsTrue(File.Exists("test-output/output.pdf"));
    }

    [TestMethod]
    public async Task Can_convert_a_local_PDF_file_to_DOCX()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      var results = await helper.Convert("test.pdf", OutputFormat.Docx);

      Assert.AreEqual(1, results.Count());
      await results.First().SaveToFile("test-output/output.docx");
      Assert.IsTrue(File.Exists("test-output/output.docx"));
    }

    [TestMethod]
    public async Task Can_convert_a_local_DOCX_file_to_TIFF()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      var results = await helper.Convert("test.docx", OutputFormat.Tiff);

      Assert.AreEqual(1, results.Count());
      await results.First().SaveToFile("test-output/output.tiff");
      Assert.IsTrue(File.Exists("test-output/output.tiff"));
    }

    [TestMethod]
    public async Task Can_convert_a_local_DOCX_file_to_JPEG()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      var results = (await helper.Convert("test.docx", OutputFormat.Jpeg)).ToList();

      Assert.AreEqual(2, results.Count);
      await results[0].SaveToFile("test-output/output-page-1.jpeg");
      Assert.IsTrue(File.Exists("test-output/output-page-1.jpeg"));
      await results[1].SaveToFile("test-output/output-page-2.jpeg");
      Assert.IsTrue(File.Exists("test-output/output-page-2.jpeg"));
    }

    [TestMethod]
    public async Task Can_convert_a_local_DOCX_file_to_PNG()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      var results = (await helper.Convert("test.docx", OutputFormat.Png)).ToList();

      Assert.AreEqual(2, results.Count);
      await results[0].SaveToFile("test-output/output-page-1.png");
      Assert.IsTrue(File.Exists("test-output/output-page-1.png"));
      await results[1].SaveToFile("test-output/output-page-2.png");
      Assert.IsTrue(File.Exists("test-output/output-page-2.png"));
    }





    [TestMethod]
    public async Task Can_convert_a_DOCX_stream_to_PDF()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      IEnumerable<WorkFile> results;
      using (var stream = File.OpenRead("test.docx"))
      {
        results = await helper.Convert(stream, OutputFormat.Pdf);
      }

      Assert.AreEqual(1, results.Count());
      await results.First().SaveToFile("test-output/output.pdf");
      Assert.IsTrue(File.Exists("test-output/output.pdf"));
    }

    [TestMethod]
    public async Task Can_convert_a_PDF_stream_to_DOCX()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      IEnumerable<WorkFile> results;
      using (var stream = File.OpenRead("test.pdf"))
      {
        results = await helper.Convert(stream, OutputFormat.Docx);
      }

      Assert.AreEqual(1, results.Count());
      await results.First().SaveToFile("test-output/output.docx");
      Assert.IsTrue(File.Exists("test-output/output.docx"));
    }

    [TestMethod]
    public async Task Can_convert_a_DOCX_stream_to_TIFF()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      IEnumerable<WorkFile> results;
      using (var stream = File.OpenRead("test.docx"))
      {
        results = await helper.Convert(stream, OutputFormat.Tiff);
      }

      Assert.AreEqual(1, results.Count());
      await results.First().SaveToFile("test-output/output.tiff");
      Assert.IsTrue(File.Exists("test-output/output.tiff"));
    }

    [TestMethod]
    public async Task Can_convert_a_DOCX_stream_to_JPEG()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      IList<WorkFile> results;
      using (var stream = File.OpenRead("test.docx"))
      {
        results = (await helper.Convert(stream, OutputFormat.Jpeg)).ToList();
      }

      Assert.AreEqual(2, results.Count);
      await results[0].SaveToFile("test-output/output-page-1.jpeg");
      Assert.IsTrue(File.Exists("test-output/output-page-1.jpeg"));
      await results[1].SaveToFile("test-output/output-page-2.jpeg");
      Assert.IsTrue(File.Exists("test-output/output-page-2.jpeg"));
    }

    [TestMethod]
    public async Task Can_convert_a_DOCX_stream_to_PNG()
    {
      var helper = new DocumentProcessingHelper("https://api.accusoft.com", System.Environment.GetEnvironmentVariable("API_KEY"));

      IList<WorkFile> results;
      using (var stream = File.OpenRead("test.docx"))
      {
        results = (await helper.Convert(stream, OutputFormat.Png)).ToList();
      }

      Assert.AreEqual(2, results.Count);
      await results[0].SaveToFile("test-output/output-page-1.png");
      Assert.IsTrue(File.Exists("test-output/output-page-1.png"));
      await results[1].SaveToFile("test-output/output-page-2.png");
      Assert.IsTrue(File.Exists("test-output/output-page-2.png"));
    }
  }
}
