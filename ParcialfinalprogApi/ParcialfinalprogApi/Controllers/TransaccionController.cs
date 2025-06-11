using Microsoft.AspNetCore.Mvc;
using ParcialfinalprogApi.Data;
using ParcialfinalprogApi.Models;
using System.Text.Json;

namespace ParcialfinalprogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransaccionController()
        {
            _context = new AppDbContext();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Transaccion>> GetAll()
        {
            var transacciones = _context.Transacciones
                .OrderByDescending(t => t.DateTime)
                .ToList();

            return Ok(transacciones);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Transaccion transaccion)
        {
            if (transaccion.CryptoAmount <= 0)
                return BadRequest("Intente con una cantidad valida");

            if (transaccion.Action != "purchase" && transaccion.Action != "sale")
                return BadRequest("Intente nuevamente, la accion tiene que ser 'purchase' o 'sale'");

            if (transaccion.DateTime == DateTime.MinValue)
                return BadRequest("Intente con una fecha valida");

            // Validar que no se venda más de lo que se tiene
            if (transaccion.Action == "sale")
            {
                decimal compras = _context.Transacciones
                    .Where(t => t.CryptoCode == transaccion.CryptoCode && t.Action == "purchase")
                    .Sum(t => t.CryptoAmount);

                decimal ventas = _context.Transacciones
                    .Where(t => t.CryptoCode == transaccion.CryptoCode && t.Action == "sale")
                    .Sum(t => t.CryptoAmount);

                decimal disponible = compras - ventas;

                if (transaccion.CryptoAmount > disponible)
                    return BadRequest($"No se pudo realizar la venta {transaccion.CryptoAmount}. Monto insuficiente {disponible}.");
            }

            // Consultar precio actual en CriptoYa
            decimal precioActual = await ObtenerPrecioActual(transaccion.CryptoCode);

            // Calcular el money
            transaccion.Money = transaccion.CryptoAmount * precioActual;

            // Guardar en la base
            _context.Transacciones.Add(transaccion);
            _context.SaveChanges();

            return Ok();
        }

        private async Task<decimal> ObtenerPrecioActual(string cryptoCode)
        {
            using (var client = new HttpClient())
            {
                string url = $"https://criptoya.com/api/satoshitango/{cryptoCode}/ars";

                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Intente nuevamente");

                var json = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;

                    decimal precio = root.GetProperty("totalBid").GetDecimal(); // usamos totalBid

                    return precio;
                }
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Transaccion> GetById(int id)
        {
            var transaccion = _context.Transacciones.Find(id);

            if (transaccion == null)
                return NotFound();

            return Ok(transaccion);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var transaccion = _context.Transacciones.Find(id);

            if (transaccion == null)
                return NotFound();

            _context.Transacciones.Remove(transaccion);
            _context.SaveChanges();

            return Ok();
        }
    }
}
