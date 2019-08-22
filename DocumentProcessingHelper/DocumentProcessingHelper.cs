using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Accusoft.PrizmDoc.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Accusoft.PrizmDoc
{
  public class DocumentProcessingHelper
  {
    internal readonly PrizmDocRestClient client;

    public DocumentProcessingHelper(string baseAddress) : this(baseAddress, null) { }
    public DocumentProcessingHelper(Uri baseAddress) : this(baseAddress, null) { }
    public DocumentProcessingHelper(string baseAddress, string apiKey) : this(new Uri(baseAddress), apiKey) { }
    public DocumentProcessingHelper(Uri baseAddress, string apiKey)
    {
      client = new PrizmDocRestClient(baseAddress);

      if (apiKey != null)
      {
        client.DefaultRequestHeaders.Add("Acs-Api-Key", apiKey);
      }
    }

    public async Task<IEnumerable<WorkFile>> Convert(string localFilePath, OutputFormat outputFormat)
    {
      using (var localFileReadStream = File.OpenRead(localFilePath))
      {
        return await Convert(localFileReadStream, outputFormat);
      }
    }

    public async Task<IEnumerable<WorkFile>> Convert(Stream inputDocument, OutputFormat outputFormat)
    {
      var session = client.CreateAffinitySession();

      // Upload the input file
      var inputWorkFile = await session.PostWorkFile(inputDocument, "txt");

      // Build the input JSON
      var stringWriter = new StringWriter();
      using (var jsonWriter = new JsonTextWriter(stringWriter))
      {
        // start req JSON
        jsonWriter.WriteStartObject();

        // start input object
        jsonWriter.WritePropertyName("input");
        jsonWriter.WriteStartObject();

        // sources
        jsonWriter.WritePropertyName("sources");
        jsonWriter.WriteStartArray();
        jsonWriter.WriteStartObject();
        jsonWriter.WritePropertyName("fileId");
        jsonWriter.WriteValue(inputWorkFile.FileId);
        jsonWriter.WriteEndObject();
        jsonWriter.WriteEndArray();

        // dest
        jsonWriter.WritePropertyName("dest");
        jsonWriter.WriteStartObject();
        jsonWriter.WritePropertyName("format");
        jsonWriter.WriteValue(Enum.GetName(typeof(OutputFormat), outputFormat).ToLowerInvariant());
        jsonWriter.WriteEndObject();

        // _features
        if (outputFormat == OutputFormat.Docx)
        {
          jsonWriter.WritePropertyName("_features");
          jsonWriter.WriteStartObject();
          jsonWriter.WritePropertyName("pdfToDocx");
          jsonWriter.WriteStartObject();
          jsonWriter.WritePropertyName("enabled");
          jsonWriter.WriteValue(true);
          jsonWriter.WriteEndObject();
          jsonWriter.WriteEndObject();
        }

        // end input object
        jsonWriter.WriteEndObject();

        // end req JSON
        jsonWriter.WriteEndObject();
      }

      var json = stringWriter.ToString();

      // Start the conversion
      using (var response = await session.PostAsync("/v2/contentConverters", new StringContent(json, Encoding.UTF8, "application/json")))
      {
        response.EnsureSuccessStatusCode();
        json = await response.Content.ReadAsStringAsync();
      }

      var process = JObject.Parse(json);
      var processId = (string)process["processId"];

      // Wait for the conversion to complete
      using (var response = await session.GetFinalProcessStatusAsync($"/v2/contentConverters/{processId}"))
      {
          response.EnsureSuccessStatusCode();
          json = await response.Content.ReadAsStringAsync();
      }

      process = JObject.Parse(json);

      // Did the process error?
      if ((string)process["state"] != "complete")
      {
          throw new Exception("The conversion failed:\n" + json);
      }

      return process["output"]["results"].Children().Select(result => new WorkFile((string)result["fileId"], session));
    }
  }
}
