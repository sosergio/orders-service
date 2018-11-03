using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OrdersService.ApiClient
{
    public class SslHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly string _thumbprint;
        public SslHttpClientFactory(string certificateThumbprint)
        {
            if (string.IsNullOrEmpty(certificateThumbprint))
                throw new ArgumentNullException("certificateThumbprint", "SslHttpClientFactory constructor argument 'certificateThumbprint' cannot be null or empty");

            _thumbprint = certificateThumbprint;
        }

        public override HttpClient CreateHttpClient(HttpMessageHandler messageHandler)
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,
            };
            var cert = FindCertificateWithThumbprint(_thumbprint);
            if (cert == null)
            {
                throw new ApplicationException("SslHttpClientFactory CreateHttpClient | A certificate with the given thumbprint key couldn't be found.");
            }
            handler.ClientCertificates.Add(cert);

            handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            return new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

        }

        public X509Certificate2 FindCertificateWithThumbprint(string thumbprint)
        {
            X509Certificate2 certificate = null;
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certCollection.Count > 0)
            {
                certificate = certCollection[0];
            }
            certStore.Close();
            return certificate;
        }
    }
}
