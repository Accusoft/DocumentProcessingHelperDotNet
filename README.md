# DocumentProcessingHelper for .NET (BETA)

**(BETA)** Simple .NET helper for document processing, powered by PrizmDoc Server. You can use this helper with either [PrizmDoc Cloud](https://cloud.accusoft.com) or your own self-hosted PrizmDoc Server.

If you don't have your own PrizmDoc Server instance, the easiest way to get started is with [PrizmDoc Cloud](https://cloud.accusoft.com). Sign up for a free trial account to get an API key at https://cloud.accusoft.com/.

**Requires .NET Core 1.0 or higher or .NET Framework 4.6.1 or higher.**

## Installation

Add the [DocumentProcessingHelper NuGet package](https://www.nuget.org/packages/PrizmDocRestClient/) to your project.

This will add a new `Accusoft.PrizmDoc` namespace containing the `DocumentProcessingHelper` class.

## Example Usage

### Converting a JPEG to PDF

```csharp
using Accusoft.PrizmDoc;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication
{
    public static class Program
    {
        public static void Main()
        {
            MainAsync().Wait();
        }

        public static async Task MainAsync()
        {
            var helper = new DocumentProcessingHelper("https://api.accusoft.com", "YOUR_API_KEY");

            var output = await helper.Convert("input.jpeg", OutputFormat.Pdf);

            await output.First().SaveToFile("output.pdf");
        }
    }
}
```

### Converting a PDF to PNG

```csharp
using Accusoft.PrizmDoc;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication
{
    public static class Program
    {
        public static void Main()
        {
            MainAsync().Wait();
        }

        public static async Task MainAsync()
        {
            var helper = new DocumentProcessingHelper("https://api.accusoft.com", "YOUR_API_KEY");

            var output = (await helper.Convert("input.pdf", OutputFormat.Png)).ToList();

            for (var i = 0; i < output.Length; i++)
            {
              await output[i].SaveToFile("page-" + (i+1) + ".png");
            }
        }
    }
}
```
