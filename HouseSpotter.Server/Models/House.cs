namespace HouseSpotter.Server.Models
{
    /// <summary>
    /// Represents a housing object.
    /// </summary>
    public class Housing
    {
        /// <summary>
        /// Gets or sets the ID of the housing.
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// Gets or sets the Scrape of the housing.
        /// </summary>
        public ScrapedSite ScrapedSite { get; set; }

        /// <summary>
        /// Gets or sets the AnketosKodas of the housing.
        /// </summary>
        public string? AnketosKodas { get; set; }

        /// <summary>
        /// Gets or sets the Nuotrauka of the housing.
        /// </summary>
        public string? Nuotrauka { get; set; }

        /// <summary>
        /// Gets or sets the Link of the housing.
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// Gets or sets the BustoTipas of the housing.
        /// </summary>
        public string? BustoTipas { get; set; }

        /// <summary>
        /// Gets or sets the Title of the housing.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the Kaina of the housing.
        /// </summary>
        public double? Kaina { get; set; }

        /// <summary>
        /// Gets or sets the Gyvenviete of the housing.
        /// </summary>
        public string? Gyvenviete { get; set; }

        /// <summary>
        /// Gets or sets the Gatve of the housing.
        /// </summary>
        public string? Gatve { get; set; }

        /// <summary>
        /// Gets or sets the KainaMen of the housing.
        /// </summary>
        public double? KainaMen { get; set; }

        /// <summary>
        /// Gets or sets the NamoNumeris of the housing.
        /// </summary>
        public string? NamoNumeris { get; set; }

        /// <summary>
        /// Gets or sets the ButoNumeris of the housing.
        /// </summary>
        public string? ButoNumeris { get; set; }

        /// <summary>
        /// Gets or sets the KambariuSk of the housing.
        /// </summary>
        public int? KambariuSk { get; set; }

        /// <summary>
        /// Gets or sets the Plotas of the housing.
        /// </summary>
        public double? Plotas { get; set; }

        /// <summary>
        /// Gets or sets the SklypoPlotas of the housing.
        /// </summary>
        public string? SklypoPlotas { get; set; }

        /// <summary>
        /// Gets or sets the Aukstas of the housing.
        /// </summary>
        public int? Aukstas { get; set; }

        /// <summary>
        /// Gets or sets the AukstuSk of the housing.
        /// </summary>
        public int? AukstuSk { get; set; }

        /// <summary>
        /// Gets or sets the Metai of the housing.
        /// </summary>
        public int? Metai { get; set; }

        /// <summary>
        /// Gets or sets the Irengimas of the housing.
        /// </summary>
        public string? Irengimas { get; set; }

        /// <summary>
        /// Gets or sets the NamoTipas of the housing.
        /// </summary>
        public string? NamoTipas { get; set; }

        /// <summary>
        /// Gets or sets the PastatoTipas of the housing.
        /// </summary>
        public string? PastatoTipas { get; set; }

        /// <summary>
        /// Gets or sets the Sildymas of the housing.
        /// </summary>
        public string? Sildymas { get; set; }

        /// <summary>
        /// Gets or sets the PastatoEnergijosSuvartojimoKlase of the housing.
        /// </summary>
        public string? PastatoEnergijosSuvartojimoKlase { get; set; }

        /// <summary>
        /// Gets or sets the Ypatybes of the housing.
        /// </summary>
        public string? Ypatybes { get; set; }

        /// <summary>
        /// Gets or sets the PapildomosPatalpos of the housing.
        /// </summary>
        public string? PapildomosPatalpos { get; set; }

        /// <summary>
        /// Gets or sets the PapildomaIranga of the housing.
        /// </summary>
        public string? PapildomaIranga { get; set; }

        /// <summary>
        /// Gets or sets the Apsauga of the housing.
        /// </summary>
        public string? Apsauga { get; set; }

        /// <summary>
        /// Gets or sets the Vanduo of the housing.
        /// </summary>
        public string? Vanduo { get; set; }

        /// <summary>
        /// Gets or sets the IkiTelkinio of the housing.
        /// </summary>
        public int? IkiTelkinio { get; set; }

        /// <summary>
        /// Gets or sets the ArtimiausiasTelkinys of the housing.
        /// </summary>
        public string? ArtimiausiasTelkinys { get; set; }

        /// <summary>
        /// Gets or sets the RCNumeris of the housing.
        /// </summary>
        public string? RCNumeris { get; set; }

        /// <summary>
        /// Gets or sets the Aprasymas of the housing.
        /// </summary>
        public string? Aprasymas { get; set; }
    }
}