﻿using Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly bibliotecaContext _biblioContexto;

        public AutoresController(bibliotecaContext biblioContexto)
        {
            _biblioContexto = biblioContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Autor> listadoAutor = (from e in _biblioContexto.autor
                                           select e).ToList();
            if (listadoAutor.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoAutor);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var autor = (from a in _biblioContexto.autor
                            join l in _biblioContexto.libro
                                 on a.Id equals l.AutorId  
                                 where l.AutorId == id
                            select new
                            {
                                Id_autor = a.Id,
                                a.Nombre,
                                a.Nacionalidad,
                                Id_Libro = l.Id,
                                l.Titulo,
                            }).ToList();

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarAutor([FromBody] Autor autor)
        {
            try
            {
                _biblioContexto.autor.Add(autor);
                _biblioContexto.SaveChanges();
                return Ok(autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarAutor(int id, [FromBody] Autor autorModificar)
        {
            Autor? autorActual = (from e in _biblioContexto.autor
                                     where e.Id == id
                                     select e).FirstOrDefault();



            if (autorActual == null)
            {
                return NotFound();
            }

            autorActual.Nombre = autorModificar.Nombre;
            autorActual.Nacionalidad = autorModificar.Nacionalidad;
            

            _biblioContexto.Entry(autorActual).State = EntityState.Modified;
            _biblioContexto.SaveChanges();


            return Ok(autorModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarAutor(int id)
        {
            Autor? autor = (from e in _biblioContexto.autor
                               where e.Id == id
                               select e).FirstOrDefault();



            if (autor == null)
            {
                return NotFound();
            }



            _biblioContexto.autor.Attach(autor);
            _biblioContexto.autor.Remove(autor);
            _biblioContexto.SaveChanges();



            return Ok(autor);
        }

        [HttpGet]
        [Route("ContarLibrosXAutor/{id}")]
        public IActionResult ContarLibros(int id)
        {
            var autor = (from a in _biblioContexto.autor
                         join l in _biblioContexto.libro
                         on a.Id equals l.AutorId
                         where l.AutorId == id
                         group l by a.Id into grupo
                         select new
                         {
                             AutorId = grupo.Key,
                             CantidadLibros = grupo.Count()
                         }).FirstOrDefault();

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }
    }
}
