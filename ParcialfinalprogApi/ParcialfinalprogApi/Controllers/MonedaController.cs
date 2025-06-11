using Microsoft.AspNetCore.Mvc;
using ParcialfinalprogApi.Data;
using ParcialfinalprogApi.Models;

namespace ParcialfinalprogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonedaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MonedaController()
        {
            _context = new AppDbContext();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Moneda>> GetAll()
        {
            return Ok(_context.Monedas.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Moneda> GetById(int id)
        {
            var moneda = _context.Monedas.Find(id);
            if (moneda == null)
                return NotFound();

            return Ok(moneda);
        }

        [HttpPost]
        public ActionResult Create(Moneda moneda)
        {
            _context.Monedas.Add(moneda);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Moneda updatedMoneda)
        {
            var moneda = _context.Monedas.Find(id);
            if (moneda == null)
                return NotFound();

            moneda.Abreviatura = updatedMoneda.Abreviatura;
            moneda.Nombre = updatedMoneda.Nombre;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var moneda = _context.Monedas.Find(id);
            if (moneda == null)
                return NotFound();

            _context.Monedas.Remove(moneda);
            _context.SaveChanges();

            return Ok();
        }
    }
}
