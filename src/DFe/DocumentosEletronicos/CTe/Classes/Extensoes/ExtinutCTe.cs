﻿/********************************************************************************/
/* Projeto: Biblioteca ZeusNFe                                                  */
/* Biblioteca C# para emissão de Nota Fiscal Eletrônica - NFe e Nota Fiscal de  */
/* Consumidor Eletrônica - NFC-e (http://www.nfe.fazenda.gov.br)                */
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
using System.Xml;
using DFe.Assinatura;
using DFe.CertificadosDigitais;
using DFe.Configuracao;
using DFe.DocumentosEletronicos.CTe.Classes.Servicos.Inutilizacao;
using DFe.DocumentosEletronicos.CTe.Validacao;
using DFe.DocumentosEletronicos.Flags;
using DFe.DocumentosEletronicos.ManipuladorDeXml;
using DFe.DocumentosEletronicos.ManipulaPasta;

namespace DFe.DocumentosEletronicos.CTe.Classes.Extensoes
{
    public static class ExtinutCTe
    {
        public static void Assinar(this inutCTe inutCTe, CertificadoDigital certificadoDigital, DFeConfig config)
        {
           inutCTe.Signature = AssinaturaDigital.Assina(inutCTe, inutCTe.infInut.Id,
                certificadoDigital, config);
        }


        /// <summary>
        ///     Converte o objeto inutCTe para uma string no formato XML
        /// </summary>
        /// <param name="pedInutilizacao"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto inutCTe</returns>
        public static string ObterXmlString(this inutCTe pedInutilizacao)
        {
            return FuncoesXml.ClasseParaXmlString(pedInutilizacao);
        }

        public static void ValidarShcema(this inutCTe inutCTe, DFeConfig config)
        {

            var xmlValidacao = inutCTe.ObterXmlString();

            switch (inutCTe.versao)
            {
                case VersaoServico.Versao200:
                    Validador.Valida(xmlValidacao, "inutCTe_v2.00.xsd", config);
                    break;
                case VersaoServico.Versao300:
                    Validador.Valida(xmlValidacao, "inutCTe_v3.00.xsd", config);
                    break;
                default:
                    throw new InvalidOperationException("Nos achamos um erro na hora de validar o schema, " +
                                                   "a versão está inválida, somente é permitido " +
                                                   "versão 2.00 é 3.00");
            }
        }

        public static void SalvarXmlEmDisco(this inutCTe inutCTe, DFeConfig config)
        {
            if (config.NaoSalvarXml()) return;

            var caminhoXml = new ResolvePasta(config, DateTime.Now).PastaInutilizacaoEnvio();

            var arquivoSalvar = Path.Combine(caminhoXml, new StringBuilder(inutCTe.infInut.Id).Append("-ped-inu.xml").ToString());

            FuncoesXml.ClasseParaArquivoXml(inutCTe, arquivoSalvar);
        }

        public static XmlDocument CriaRequestWs(this inutCTe inutCTe)
        {
            var request = new XmlDocument();
            request.LoadXml(inutCTe.ObterXmlString());

            return request;
        }
    }
}