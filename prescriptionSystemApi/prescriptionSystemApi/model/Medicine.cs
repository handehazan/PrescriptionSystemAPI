namespace prescriptionSystemApi.model
{
    public class Medicine
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Unique ID for MongoDB
        public string IlacAdi { get; set; } // İlaç Adı
        public string Barkod { get; set; } // Barkod
        public string ATCCode { get; set; } // ATC Kodu
        public string ATCAdi { get; set; } // ATC Adı
        public string FirmaAdi { get; set; } // Firma Adı
        public string ReceteTuru { get; set; } // Reçete Türü
        public string Durumu { get; set; } // Durumu
        public string Aciklama { get; set; } // Açıklama
        public string TemelIlacListesiDurumu { get; set; } // Temel İlaç Listesi Durumu
        public string CocukTemelIlacListesiDurumu { get; set; } // Çocuk Temel İlaç Listesi Durumu
        public string YenidoganTemelIlacListesiDurumu { get; set; } // Yenidoğan Temel İlaç Listesi Durumu
        public DateTime? AktifUrunlerListesineAlindigiTarih { get; set; } // Aktif Ürünler Listesine Alındığı Tarih
    }
}
