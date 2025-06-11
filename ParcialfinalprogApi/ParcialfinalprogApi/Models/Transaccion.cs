using System.ComponentModel.DataAnnotations;

namespace ParcialfinalprogApi.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; } 
        public string Action { get; set; } 
        public decimal CryptoAmount { get; set; } 
        public decimal Money { get; set; } 
        public DateTime DateTime { get; set; }
        public Moneda? Moneda { get; set; }

    }

}
