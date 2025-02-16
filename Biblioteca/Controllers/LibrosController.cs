using Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly bibliotecaContext _biblioContexto;

        public LibrosController(bibliotecaContext biblioContexto)
        {
            _biblioContexto = biblioContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Libro> listadoLibros = (from e in _biblioContexto.libro
                                         select e).ToList();
            if (listadoLibros.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoLibros);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var autor = (from ll in _biblioContexto.libro
                         join aa in _biblioContexto.autor
                              on ll.AutorId equals aa.Id
                              where ll.Id == id
                         select new
                         {
                             Id_libro = ll.Id,
                             ll.Titulo,
                             ll.AnioPublicacion,
                             Id_Autor = aa.Id,
                             aa.Nombre,
                             ll.CategoriaId,
                             ll.Resumen
                         }).ToList();

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarLibro([FromBody] Libro libro)
        {
            try
            {
                _biblioContexto.libro.Add(libro);
                _biblioContexto.SaveChanges();
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult Actualizarlibro(int id, [FromBody] Libro libroModificar)
        {
            Libro? libroActual = (from e in _biblioContexto.libro
                                  where e.Id == id
                                  select e).FirstOrDefault();



            if (libroActual == null)
            {
                return NotFound();
            }

            libroActual.Titulo = libroModificar.Titulo;
            libroActual.AnioPublicacion = libroModificar.AnioPublicacion;
            libroActual.AutorId = libroModificar.AutorId;
            libroActual.CategoriaId = libroModificar.CategoriaId;
            libroActual.Resumen = libroModificar.Resumen;


            _biblioContexto.Entry(libroActual).State = EntityState.Modified;
            _biblioContexto.SaveChanges();


            return Ok(libroModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarLibro(int id)
        {
            Libro? libro = (from e in _biblioContexto.libro
                            where e.Id == id
                            select e).FirstOrDefault();



            if (libro == null)
            {
                return NotFound();
            }



            _biblioContexto.libro.Attach(libro);
            _biblioContexto.libro.Remove(libro);
            _biblioContexto.SaveChanges();



            return Ok(libro);
        }

        [HttpGet]
        [Route("GetLibros")]
        public IActionResult libros2000()
        {
            var listadoEquipo = (from ll in _biblioContexto.libro
                                 join aa in _biblioContexto.autor
                                      on ll.AutorId equals aa.Id
                                 where ll.AnioPublicacion > 2000
                                 select new
                                 {
                                     Id_libro = ll.Id,
                                     ll.Titulo,
                                     ll.AnioPublicacion,
                                     Id_Autor = aa.Id,
                                     aa.Nombre,
                                     ll.CategoriaId,
                                     ll.Resumen
                                 }).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        [HttpGet]
        [Route("GetByTitulo/{titulo}")]
        public IActionResult GetLibroxTitulo(string titulo)
        {
            var autor = (from ll in _biblioContexto.libro
                         join aa in _biblioContexto.autor
                              on ll.AutorId equals aa.Id
                         where ll.Titulo == titulo
                         select new
                         {
                             Id_libro = ll.Id,
                             ll.Titulo,
                             ll.AnioPublicacion,
                             Id_Autor = aa.Id,
                             aa.Nombre,
                             ll.CategoriaId,
                             ll.Resumen
                         }).ToList();

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        [HttpGet]
        [Route("GetLibrosPaginado")]
        public IActionResult librosPaginados()
        {
            var listadoEquipo = (from ll in _biblioContexto.libro
                                 join aa in _biblioContexto.autor
                                      on ll.AutorId equals aa.Id
                                 select new
                                 {
                                     Id_libro = ll.Id,
                                     ll.Titulo,
                                     ll.AnioPublicacion,
                                     Id_Autor = aa.Id,
                                     aa.Nombre,
                                     ll.CategoriaId,
                                     ll.Resumen
                                 }).Skip(10).Take(10).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }


    }
}
