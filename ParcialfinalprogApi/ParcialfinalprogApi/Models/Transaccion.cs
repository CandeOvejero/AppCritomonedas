using System.ComponentModel.DataAnnotations;

namespace ParcialfinalprogApi.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; } // ej: "btc", "usdc"
        public string Action { get; set; } // "purchase" o "sale"
        public decimal CryptoAmount { get; set; } // Cantidad de cripto comprada o vendida
        public decimal Money { get; set; } // ARS pagado o cobrado → calculado
        public DateTime DateTime { get; set; }
        public Moneda? Moneda { get; set; }

    }

}
