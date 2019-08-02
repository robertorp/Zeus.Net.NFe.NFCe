using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DFe.Configuracao;
using DFe.DocumentosEletronicos.CTe.Classes.Extensoes;
using DFe.DocumentosEletronicos.CTe.Classes.Informacoes.Tipos;
using DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes;
using DFe.DocumentosEletronicos.CTe.Wsdl.Enderecos.Helpers;

namespace DFe.DocumentosEletronicos.CTe.CTeOS.Extensoes
{
    public static class extCteOs
    {
        public static infCTeSupl QrCode(this CTeOS cteOs, X509Certificate2 certificadoDigital,
            DFeConfig config,
        Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            var qrCode = new StringBuilder(UrlHelper.ObterUrlServico(config).QrCode);
            qrCode.Append("?");
            qrCode.Append("chCTe=").Append(cteOs.Chave());
            qrCode.Append("&");
            qrCode.Append("tpAmb=").Append(cteOs.AmbienteSefazInt());

            if (cteOs.InfCte.ide.tpEmis != tpEmis.teNormal)
            {
                var assinatura = Convert.ToBase64String(CreateSignaturePkcs1(certificadoDigital, encoding.GetBytes(cteOs.Chave())));
                qrCode.Append("&sign=");
                qrCode.Append(assinatura);
            }

            return new infCTeSupl
            {
                qrCodCTe = qrCode.ToString()
            };
        }

        private static byte[] CreateSignaturePkcs1(X509Certificate2 certificado, byte[] Value)

        {
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificado.PrivateKey;

            RSAPKCS1SignatureFormatter rsaF = new RSAPKCS1SignatureFormatter(rsa);

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] hash = null;

            hash = sha1.ComputeHash(Value);

            rsaF.SetHashAlgorithm("SHA1");

            return rsaF.CreateSignature(hash);

        }
    }
}