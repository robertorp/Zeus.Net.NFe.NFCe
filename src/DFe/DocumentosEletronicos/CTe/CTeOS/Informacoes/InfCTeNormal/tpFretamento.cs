using System.Xml.Serialization;

namespace DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.InfCTeNormal
{
    public enum tpFretamento
    {
        [XmlEnum("1")]
        Eventual = 1,

        [XmlEnum("2")]
        Continuo = 2
    }
}