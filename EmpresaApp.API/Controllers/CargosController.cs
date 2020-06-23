using System;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Services.Exclusoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApp.API.Controllers
{
    [Route("api/cargos")]
    [ApiController]
    public class CargosController : ControllerBase
    {
        private readonly IArmazenadorDeCargo _armazenadorDeCargo;
        private readonly ExclusaoDeCargo _exclusaoDeCargo;
        private readonly IRepository<Cargo> _cargoRepositorio;

        public CargosController (
            IArmazenadorDeCargo armazenadorDeCargo, 
            ExclusaoDeCargo exclusaoDeCargo, 
            IRepository<Cargo> cargoRepositorio)
        {
            _armazenadorDeCargo = armazenadorDeCargo;
            _exclusaoDeCargo = exclusaoDeCargo;
            _cargoRepositorio = cargoRepositorio;
        }

        // GET: api/cargos
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _cargoRepositorio.List();
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
                var result = _cargoRepositorio.GetById(id);
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
                _armazenadorDeCargo.Armazenar(dto);
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
            var result = _cargoRepositorio.GetById(id);
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
                _exclusaoDeCargo.Excluir(id);
                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Problema ao excluir o cargo informado");
            }
        }
    }
}
