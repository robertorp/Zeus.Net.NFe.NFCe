using System;

namespace DFe.DocumentosEletronicos.CTe.CTeOS.Informacoes.InfCTeNormal
{
    public class infFretamentoOs
    {
        public tpFretamento tpFretamento { get; set; }

        public DateTime? dhViagem { get; set; }

        public bool dhViagemSpecified { get { return dhViagem.HasValue; } }
    }
}