/********************************************************************************/
/* Projeto: Biblioteca ZeusMDFe                                                 */
/* Biblioteca C# para emissão de Manifesto Eletrônico Fiscal de Documentos      */
/* (https://mdfe-portal.sefaz.rs.gov.br/                                        */
/*                                                                              */
/* Direitos Autorais Reservados (c) 2014 Adenilton Batista da Silva             */
/*                                       Zeusdev Tecnologia LTDA ME             */
/*                                                                              */
/*  Você pode obter a última versão desse arquivo no GitHub                     */
/* localizado em https://github.com/adeniltonbs/Zeus.Net.NFe.NFCe               */
/*                                                                              */
/*                                                                              */
/*  Esta biblioteca é software livre; você pode redistribuí-la e/ou modificá-la */
/* sob os termos da Licença Pública Geral Menor do GNU conforme publicada pela  */
/* Free Software Foundation; tanto a versão 2.1 da Licença, ou (a seu critério) */
/* qualquer versão posterior.                                                   */
/*                                                                              */
/*  Esta biblioteca é distribuída na expectativa de que seja útil, porém, SEM   */
/* NENHUMA GARANTIA; nem mesmo a garantia implícita de COMERCIABILIDADE OU      */
/* ADEQUAÇÃO A UMA FINALIDADE ESPECÍFICA. Consulte a Licença Pública Geral Menor*/
/* do GNU para mais detalhes. (Arquivo LICENÇA.TXT ou LICENSE.TXT)              */
/*                                                                              */
/*  Você deve ter recebido uma cópia da Licença Pública Geral Menor do GNU junto*/
/* com esta biblioteca; se não, escreva para a Free Software Foundation, Inc.,  */
/* no endereço 59 Temple Street, Suite 330, Boston, MA 02111-1307 USA.          */
/* Você também pode obter uma copia da licença em:                              */
/* http://www.opensource.org/licenses/lgpl-license.php                          */
/*                                                                              */
/* Zeusdev Tecnologia LTDA ME - adenilton@zeusautomacao.com.br                  */
/* http://www.zeusautomacao.com.br/                                             */
/* Rua Comendador Francisco josé da Cunha, 111 - Itabaiana - SE - 49500-000     */
/********************************************************************************/

using System;
using System.IO;
using System.Text;
using DFe.Assinatura;
using DFe.Configuracao;
using DFe.DocumentosEletronicos.Entidades;
using DFe.DocumentosEletronicos.Flags;
using DFe.DocumentosEletronicos.ManipuladorDeXml;
using DFe.DocumentosEletronicos.MDFe.Classes.Informacoes.Modal.Aereo;
using DFe.DocumentosEletronicos.MDFe.Classes.Informacoes.Modal.Aquaviario;
using DFe.DocumentosEletronicos.MDFe.Classes.Informacoes.Modal.Ferroviario;
using DFe.DocumentosEletronicos.MDFe.Classes.Informacoes.Modal.Rodoviario;
using DFe.DocumentosEletronicos.MDFe.Validacao;
using CertificadoDigital = DFe.CertificadosDigitais.CertificadoDigital;
using MDFEletronico = DFe.DocumentosEletronicos.MDFe.Classes.Informacoes.MDFe;

namespace DFe.DocumentosEletronicos.MDFe.Classes.Extensoes
{
    public static class ExtMDFe
    {
        public static MDFEletronico Valida(this MDFEletronico mdfe, DFeConfig dfeConfig)
        {
            if (mdfe == null) throw new ArgumentException("Erro de assinatura, MDFe esta null");

            var xmlMdfe = FuncoesXml.ClasseParaXmlString(mdfe);

            switch (dfeConfig.VersaoServico)
            {
                case VersaoServico.Versao100:
                    Validador.Valida(xmlMdfe, "MDFe_v1.00.xsd", dfeConfig);
                    break;
                case VersaoServico.Versao300:
                    Validador.Valida(xmlMdfe, "MDFe_v3.00.xsd", dfeConfig);
                    break;
            }

            var tipoModal = mdfe.InfMDFe.infModal.Modal.GetType();
            var xmlModal = FuncoesXml.ClasseParaXmlString(mdfe.InfMDFe.infModal);


            if (tipoModal == typeof (rodo))
            {
                switch (dfeConfig.VersaoServico)
                {
                    case VersaoServico.Versao100:
                        Validador.Valida(xmlModal, "MDFeModalRodoviario_v1.00.xsd", dfeConfig);
                        break;
                    case VersaoServico.Versao300:
                        Validador.Valida(xmlModal, "MDFeModalRodoviario_v3.00.xsd", dfeConfig);
                        break;
                }
            }

            if (tipoModal == typeof (aereo))
            {
                switch (dfeConfig.VersaoServico)
                {
                    case VersaoServico.Versao100:
                        Validador.Valida(xmlModal, "MDFeModalAereo_v1.00.xsd", dfeConfig);
                        break;
                    case VersaoServico.Versao300:
                        Validador.Valida(xmlModal, "MDFeModalAereo_v3.00.xsd", dfeConfig);
                        break;
                }
            }

            if (tipoModal == typeof (aquav))
            {
                switch (dfeConfig.VersaoServico)
                {
                    case VersaoServico.Versao100:
                        Validador.Valida(xmlModal, "MDFeModalAquaviario_v1.00.xsd", dfeConfig);
                        break;
                    case VersaoServico.Versao300:
                        Validador.Valida(xmlModal, "MDFeModalAquaviario_v3.00.xsd", dfeConfig);
                        break;
                }
            }

            if (tipoModal == typeof (ferrov))
            {
                switch (dfeConfig.VersaoServico)
                {
                    case VersaoServico.Versao100:
                        Validador.Valida(xmlModal, "MDFeModalFerroviario_v1.00.xsd", dfeConfig);
                        break;
                    case VersaoServico.Versao300:
                        Validador.Valida(xmlModal, "MDFeModalFerroviario_v3.00.xsd", dfeConfig);
                        break;
                }
            }

            return mdfe;
        }

        public static MDFEletronico Assina(this MDFEletronico mdfe, DFeConfig dfeConfig, CertificadoDigital certificadoDigital)
        {
            if(mdfe == null) throw new ArgumentException("Erro de assinatura, MDFe esta null");

            var modeloDocumentoFiscal = mdfe.InfMDFe.ide.mod;
            var tipoEmissao = (int) mdfe.InfMDFe.ide.tpEmis;
            var codigoNumerico = mdfe.InfMDFe.ide.cMDF;
            var estado = mdfe.InfMDFe.ide.cUF;
            var dataEHoraEmissao = mdfe.InfMDFe.ide.dhEmi;
            var cnpj = mdfe.InfMDFe.emit.CNPJ;
            var numeroDocumento = mdfe.InfMDFe.ide.nMDF;
            int serie = mdfe.InfMDFe.ide.serie;

            var dadosChave = ChaveFiscal.ObterChave(estado, dataEHoraEmissao, cnpj, modeloDocumentoFiscal, serie, numeroDocumento, tipoEmissao, codigoNumerico);

            mdfe.InfMDFe.Id = "MDFe" + dadosChave.Chave;
            mdfe.InfMDFe.versao = dfeConfig.VersaoServico;
            mdfe.InfMDFe.ide.cDV = dadosChave.DigitoVerificador;

            var assinatura = AssinaturaDigital.Assina(mdfe, mdfe.InfMDFe.Id, certificadoDigital, dfeConfig);

            mdfe.Signature = assinatura;

            return mdfe;
        }

        public static string XmlString(this MDFEletronico mdfe)
        {
            return FuncoesXml.ClasseParaXmlString(mdfe);
        }

        public static void SalvarXmlEmDisco(this MDFEletronico mdfe, DFeConfig dfeConfig, string nomeArquivo = null)
        {
            if (dfeConfig.NaoSalvarXml()) return;

            if (string.IsNullOrEmpty(nomeArquivo))
                nomeArquivo = Path.Combine(dfeConfig.CaminhoSalvarXml, new StringBuilder(mdfe.Chave()).Append("-mdfe.xml").ToString());

            FuncoesXml.ClasseParaArquivoXml(mdfe, nomeArquivo);
        }

        public static string Chave(this MDFEletronico mdfe)
        {
            var chave = mdfe.InfMDFe.Id.Substring(4, 44);
            return chave;
        }

        public static string CNPJEmitente(this MDFEletronico mdfe)
        {
            var cnpj = mdfe.InfMDFe.emit.CNPJ;

            return cnpj;
        }

        public static Estado UFEmitente(this MDFEletronico mdfe)
        {
            var estadoUf = mdfe.InfMDFe.emit.enderEmit.UF;

            return estadoUf;
        }

        public static long CodigoIbgeMunicipioEmitente(this MDFEletronico mdfe)
        {
            var codigo = mdfe.InfMDFe.emit.enderEmit.cMun;

            return codigo;
        }
    }
}
