namespace Jannara_Ecommerce.Utilities.WrapperClasses
{
    public class GetImageUrlsResult
    {
        public GetImageUrlsResult(string physicalUrl, string relativeUrl)
        {
            PhysicalUrl = physicalUrl;
            RelativeUrl = relativeUrl;
        }

        public string PhysicalUrl { get; set; }
        public string RelativeUrl { get; set; }
    }
}
