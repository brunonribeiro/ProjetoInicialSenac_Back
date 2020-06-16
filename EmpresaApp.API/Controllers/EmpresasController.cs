using System;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApp.API.Controllers
{
    [Route("api/empresas")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly EmpresaService _empresaService;
        public EmpresasController(EmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        // GET: api/empresas
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _empresaService.List();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar a lista de empresas");
            }
        }

        // GET api/empresas/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _empresaService.GetById(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar a empresa informada");
            }
        }

        // POST api/empresas
        [HttpPost]
        public void Post([FromBody] EmpresaDto dto)
        {
            if (ModelState.IsValid)
            {
                _empresaService.Save(dto);
            }
            else
            {
                throw new ArgumentException("O objeto informado está inválido.");
            }
        }

        // PUT api/empresas/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] EmpresaDto dto)
        {
            var result = _empresaService.GetById(id);
            if (result != null)
            {
                dto.Id = id;
                Post(dto);
            }
            else
            {
                throw new ArgumentException("A empresa informado não está cadastrada.");
            }
        }

        // DELETE api/empresas/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_empresaService.Remove(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao excluir a empresa informada");
            }
        }
    }
}
