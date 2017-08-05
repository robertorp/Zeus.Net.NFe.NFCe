﻿/********************************************************************************/
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
using System.Xml.Serialization;
using DFe.DocumentosEletronicos.Entidades;
using DFe.DocumentosEletronicos.Flags;
using DFe.DocumentosEletronicos.ManipuladorDeXml;
using DFe.DocumentosEletronicos.MDFe.Classes.Servicos.StatusServico;

namespace DFe.DocumentosEletronicos.MDFe.Classes.Retorno.StatusServico
{
    [Serializable]
    [XmlRoot(Namespace = "http://www.portalfiscal.inf.br/mdfe",
        ElementName = "retConsStatServMDFe")]
    public class retConsStatServMDFe : RetornoBase
    {
        [XmlAttribute(AttributeName = "versao")]
        public string versao { get; set; }

        [XmlElement(ElementName = "tpAmb")]
        public TipoAmbiente tpAmb { get; set; }

        [XmlElement(ElementName = "verAplic")]
        public string verAplic { get; set; }

        [XmlElement(ElementName = "cStat")]
        public short cStat { get; set; }

        [XmlElement(ElementName = "xMotivo")]
        public string xMotivo { get; set; }

        [XmlIgnore]
        public Estado cUF { get; set; }

        [XmlElement(ElementName = "cUF")]
        public string cUFProxy
        {
            get
            {
                return cUF.GetCodigoIbgeEmString();
            }
            set { cUF = cUF.CodigoIbgeParaEstado(value); }
        }

        [XmlElement(ElementName = "dhRecbto")]
        public DateTime dhRecbto { get; set; }

        [XmlElement(ElementName = "tMed")]
        public int? tMed { get; set; }

        [XmlElement(ElementName = "dhRetorno")]
        public DateTime? dhRetorno { get; set; }

        [XmlElement(ElementName = "xObs")]
        public string xObs { get; set; }

        public bool tMedSpecified { get { return tMed.HasValue; } }
        public bool dhRetornoSpecified { get { return dhRetorno.HasValue; } }

        public static retConsStatServMDFe LoadXml(string xml)
        {
            var retorno = FuncoesXml.XmlStringParaClasse<retConsStatServMDFe>(xml);
            retorno.RetornoXmlString = xml;

            return retorno;
        }

        public static retConsStatServMDFe LoadXml(string xml, consStatServMDFe consStatServMdFe)
        {
            var retorno = LoadXml(xml);
            retorno.EnvioXmlString = FuncoesXml.ClasseParaXmlString(consStatServMdFe);

            return retorno;
        }
    }
}