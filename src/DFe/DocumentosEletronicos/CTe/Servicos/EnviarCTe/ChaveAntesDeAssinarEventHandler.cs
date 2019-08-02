namespace DFe.DocumentosEletronicos.CTe.Servicos.EnviarCTe
{
    public class ChaveAntesDeAssinarEventHandler
    {
        public CTeOS.CTeOS CteOs { get; }
        public string Chave { get; }

        public ChaveAntesDeAssinarEventHandler(CTeOS.CTeOS cteOs, string chave)
        {
            CteOs = cteOs;
            Chave = chave;
        }
    }
}