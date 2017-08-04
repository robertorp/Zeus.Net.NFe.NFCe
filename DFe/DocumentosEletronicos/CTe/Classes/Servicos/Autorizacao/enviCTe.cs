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

using System.Collections.Generic;
using System.Xml.Serialization;
using DFe.DocumentosEletronicos.CTe.Classes.Flags;
using DFe.Flags;
using DFe.ManipuladorDeXml;
using CteEletronica = DFe.DocumentosEletronicos.CTe.Classes.Informacoes.CTe;

namespace DFe.DocumentosEletronicos.CTe.Classes.Servicos.Autorizacao
{
    [XmlRoot(ElementName = "enviCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class enviCTe
    {
        public enviCTe(VersaoServico versao, int idLote, List<CteEletronica> cTe)
        {
            this.versao = versao;
            this.idLote = idLote;
            CTe = cTe;
        }

        internal enviCTe() //para serialização apenas
        {
        }

        /// <summary>
        ///     AP02 - Versão do leiaute
        /// </summary>
        [XmlAttribute]
        public VersaoServico versao { get; set; }

        /// <summary>
        ///     AP03 - Identificador de controle do envio do lote.
        /// </summary>
        public int idLote { get; set; }

        /// <summary>
        ///     AP04 - Conjunto de CT-e transmitidas
        /// </summary>
        [XmlElement(ElementName = "CTe", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public List<CteEletronica> CTe { get; set; }


        public static enviCTe LoadXmlString(string xml)
        {
            return FuncoesXml.XmlStringParaClasse<enviCTe>(xml);
        }

        public static enviCTe LoadXmlArquivo(string caminhoArquivoXml)
        {
            return FuncoesXml.ArquivoXmlParaClasse<enviCTe>(caminhoArquivoXml);
        }
    }
}