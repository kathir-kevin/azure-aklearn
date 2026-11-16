using Azure.Storage.Blobs;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "LearnApi is running");

app.MapGet("/api/products", async (IConfiguration config) =>
{
    var products = new List<object>();

    await using var conn = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    await conn.OpenAsync();

    await using var cmd = new SqlCommand("SELECT Id, Name, Price FROM Products", conn);
    await using var reader = await cmd.ExecuteReaderAsync();

    while (await reader.ReadAsync())
        products.Add(new { Id = reader.GetInt32(0), Name = reader.GetString(1), Price = reader.GetDecimal(2) });

    return Results.Ok(products);
});

app.MapPost("/api/upload", async (IFormFile file, IConfiguration config) =>
{
    var container = new BlobContainerClient(config.GetConnectionString("BlobStorage"), "uploads");
    await container.CreateIfNotExistsAsync();

    var blob = container.GetBlobClient($"{Guid.NewGuid()}-{file.FileName}");
    await using var stream = file.OpenReadStream();
    await blob.UploadAsync(stream);

    return Results.Ok(new { blob.Name, Url = blob.Uri.ToString() });
}).DisableAntiforgery();

app.Run();