using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Botticelli.Framework.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Security;

public static class Certificates
{
    public static IHttpClientBuilder AddServerCertificates(this IHttpClientBuilder builder, BotSettings? settings) =>
        builder.ConfigurePrimaryHttpMessageHandler(() =>
        {
            if (settings?.SecuritySettings?.DisableSecurity is true)
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        (_, _, _, policyErrors) => true
                };

            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certificate = store.Certificates
                .FirstOrDefault(c => c.FriendlyName == settings!.SecuritySettings?.BotCertificateName);

            if (certificate == null) throw new NullReferenceException("Can't find a server certificate!");

            return new HttpClientHandler
            {
                ClientCertificates = { certificate },
                ServerCertificateCustomValidationCallback =
                    (_, cert, _, policyErrors) =>
                    {
#if DEBUG
                        return true;
#endif
                        if (settings.SecuritySettings?.AllowSelfSignedServerCertificate is true)
                            return true;

                        if (!cert.Thumbprint.Equals(settings.SecuritySettings?.ServerCertificateThumbprint, StringComparison.OrdinalIgnoreCase))
                            return false;
                        
                        if (policyErrors == SslPolicyErrors.None)
                        {
                            return true;
                        }
                        else if (policyErrors != SslPolicyErrors.RemoteCertificateChainErrors)
                        {
                            // Name mismatch or no cert
                            return false;
                        }

                        return false;
                    }
            };
        });
}