using System.Text.RegularExpressions;

public static class BlobNaming
{
    public static string Sanitize(string fileName)
    {
        var name = Path.GetFileNameWithoutExtension(fileName).ToLowerInvariant();
        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        var clean = Regex.Replace(name, "[^a-z0-9]+", "-").Trim('-');
        if (clean.Length == 0) clean = "file";

        return clean + ext;
    }
}