using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApp.API.Controllers
{
    [Route("api/cargos")]
    [ApiController]
    public class CargosController : ControllerBase
    {
        private readonly CargoService _cargoService;
        public CargosController(CargoService cargoService)
        {
            _cargoService = cargoService;
        }

        // GET: api/cargos
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _cargoService.List();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar a lista de cargos");
            }
        }

        // GET api/cargos/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _cargoService.GetById(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar o cargo informado");
            }
        }

        // POST api/cargos
        [HttpPost]
        public void Post([FromBody] CargoDto dto)
        {
            if (ModelState.IsValid)
            {
                _cargoService.Save(dto);
            }
            else
            {
                throw new ArgumentException("O objeto informado está inválido.");
            }
        }

        // PUT api/cargos/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CargoDto dto)
        {
            var result = _cargoService.GetById(id);
            if (result != null)
            {
                dto.Id = id;
                Post(dto);
            }
            else
            {
                throw new ArgumentException("O cargo informado não está cadastrado.");
            }
        }

        // DELETE api/cargos/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_cargoService.Remove(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao excluir o cargo informado");
            }
        }
    }
}
