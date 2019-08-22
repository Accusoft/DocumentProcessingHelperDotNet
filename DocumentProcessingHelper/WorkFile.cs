using System.IO;
using System.Threading.Tasks;
using Accusoft.PrizmDoc.Net.Http;

namespace Accusoft.PrizmDoc
{
  public class WorkFile
  {
    private readonly AffinitySession session;

    internal WorkFile(string fileId, AffinitySession session)
    {
      this.FileId = fileId;
      this.session = session;
    }

    public string FileId { get; }

    public async Task SaveToFile(string localFilePath)
    {
      using (var res = await session.GetAsync($"/PCCIS/V1/WorkFile/{FileId}"))
      {
        res.EnsureSuccessStatusCode();
        using (var fileStream = File.OpenWrite(localFilePath))
        {
          await res.Content.CopyToAsync(fileStream);
        }
      }
    }
  }
}
