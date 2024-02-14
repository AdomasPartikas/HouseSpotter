namespace HouseSpotter.Models
{
    public class HouseDTO
    {
        public string? AnketosKodas { get; set; }
        public string? Link { get; set; }
        public string? BustoTipas { get; set; }
        public string? Title { get; set; }
        public double Kaina { get; set; }
        public double KainaMen { get; set; }
        public string? NamoNumeris { get; set; }
        public string? ButoNumeris { get; set; }
        public int KambariuSk { get; set; }
        public double Plotas { get; set; }
        public int Aukstas { get; set; }
        public int AukstuSk { get; set; }
        public int Metai { get; set; }
        public string? Irengimas { get; set; }
        public string? PastatoTipas { get; set; }
        public string? Sildymas { get; set; }
        public string? PastatoEnergijosSuvartojimoKlase { get; set; }
        public string? Ypatybes { get; set; }
        public string? PapildomosPatalpos { get; set; }
        public string? PapildomaIranga { get; set; }
        public string? Apsauga { get; set; }
        public bool Pirkti { get; set; }
    }
}