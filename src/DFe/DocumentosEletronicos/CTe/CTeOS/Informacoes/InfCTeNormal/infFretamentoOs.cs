using System;
using System.Xml.Serialization;
using DFe.Ext;

namespace DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.InfCTeNormal
{
    public class infFretamentoOs
    {
        public tpFretamento tpFretamento { get; set; }

        [XmlIgnore]
        public DateTime? dhViagem { get; set; }

        [XmlElement(ElementName = "dhViagem")]
        public string ProxydhViagem
        {
            get
            {
                if (dhViagem == null) return null;

                return dhViagem.Value.ParaDataHoraStringUtc();
            }
            set
            {
                dhViagem = Convert.ToDateTime(value);
            }
        }
    }
}