namespace OrdersService.ApiClient
{
    public class OrdersApiConfig
    {
        public string Reference { get; set; }
        public string BaseUrl { get; set; }
        public bool UseSslCertificate { get; set; }
        public string CertificateThumbprint { get; set; }
    }
}
