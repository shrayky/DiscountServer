using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Configuration
{
    public class CertificateManager
    {
        private readonly string _certPath;
        private readonly string _certPassword;

        private CertificateManager(string certPath, string certPassword)
        {
            _certPath = certPath;
            _certPassword = certPassword;
        }

        public static X509Certificate2 GetCertificate(string certPath, string certPassword)
        {
            var manager = new CertificateManager(certPath, certPassword);

            return manager.LoadCertificate();
        }

        private X509Certificate2 LoadCertificate()
        {
            X509Certificate2? certificate = null;

            if (File.Exists(_certPath))
            {
                certificate = new X509Certificate2(_certPath, _certPassword);
                certificate = certificate.NotAfter < DateTime.Now ? null : certificate;
            }

            certificate ??= GenerateSelfSignedCertificate("localhost");

            return certificate;
        }

        private X509Certificate2 GenerateSelfSignedCertificate(string subjectName)
        {
            using var rsa = RSA.Create(2048);
            var request = new CertificateRequest($"CN={subjectName}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var certificate = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

            File.WriteAllBytes(_certPath, certificate.Export(X509ContentType.Pfx, _certPassword));

            return new X509Certificate2(certificate.Export(X509ContentType.Pfx), "", X509KeyStorageFlags.Exportable);
        }

    }
}
