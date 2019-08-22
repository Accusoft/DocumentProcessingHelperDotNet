using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Accusoft.PrizmDoc.Net.Http;
using Newtonsoft.Json.Linq;

namespace Accusoft.PrizmDoc
{
  internal static class AffinitySessionExtensions
  {
    internal static async Task<WorkFile> PostWorkFile(this AffinitySession session, Stream document, string fileExtension = "txt")
    {
      // Remove leading period on fileExtension if present.
      if (fileExtension != null && fileExtension.StartsWith("."))
      {
        fileExtension = fileExtension.Substring(1);
      }

      string json;
      using (var response = await session.PostAsync($"/PCCIS/V1/WorkFile?FileExtension={fileExtension}", new StreamContent(document)))
      {
        response.EnsureSuccessStatusCode();
        json = await response.Content.ReadAsStringAsync();
      }

      var workFileInfo = JObject.Parse(json);
      var workFile = new WorkFile((string)workFileInfo["fileId"], session);

      return workFile;
    }
  }
}
