namespace Jannara_Ecommerce.Utilities
{
    public static class ImageUrlHelper
    {

        public static string? ToAbsoluteUrl(string? relativeUrl, string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
            {
                return null;
            }

            if (Uri.TryCreate(relativeUrl, UriKind.Absolute, out _))
            {
                return relativeUrl;
            }

            // Add "Uploads" prefix because files are stored inside wwwroot/Uploads
            return $"{baseUrl.TrimEnd('/')}/Uploads/{relativeUrl.TrimStart('/')}";
        }

    }

}
