namespace Static.PreRenderer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class GenerateOutput 
        : IClassFixture<AppTestFixture>
    {
        private readonly AppTestFixture _fixture;
        private readonly HttpClient _client;
        private readonly string _outputPath;

        public GenerateOutput(AppTestFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.CreateDefaultClient();

            var config = _fixture.Services.GetRequiredService<IConfiguration>();
            _outputPath = config["RenderOutputDirectory"];
        }

        public static IEnumerable<object[]> GetPagesToPreRender()
            => PrerenderRouteHelper
                .GetRoutes(typeof(EventHorizon.Platform.Docs.App).Assembly) // Pass in the WebAssembly app's Assembly
                .Select(config => new object[] { config }); // wrap each route as an object array to satisfy xUnit


        [Theory, Trait("Category", "PreRender")]
        [MemberData(nameof(GetPagesToPreRender))]
        public async Task Render(string route)
        {
            // strip the initial / off
            var renderPath = route[1..];

            // create the output directory
            var relativePath = Path.Combine(_outputPath, renderPath);
            var outputDirectory = Path.GetFullPath(relativePath);
            Directory.CreateDirectory(outputDirectory);

            // Build the output file path
            var filePath = Path.Combine(outputDirectory, "index.html");

            // Call the prerendering API, and write the contents to the file
            var result = await _client.GetStreamAsync(route);
            using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

            await result.CopyToAsync(file);
            
            // Generate GZipped File
            using (var compressedFileStream = new FileStream(filePath + ".gz", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    result.CopyTo(compressionStream);
                }
            }
            
            // Generate Brotli File
            using (var brotliCompressedFileStream = new FileStream(filePath + ".br", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var brotliCompressionStream = new BrotliStream(brotliCompressedFileStream, CompressionMode.Compress))
                {
                    result.CopyTo(brotliCompressionStream);
                }
            }
        }
    }
}
