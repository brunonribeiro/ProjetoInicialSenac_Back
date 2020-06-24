using System;
using System.Linq;
using System.Threading.Tasks;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Interfaces.Armazenadores;
using EmpresaApp.Domain.Interfaces.Repositorios;
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
        private readonly ICargoRepositorio _cargoRepositorio;

        public CargosController(
            IArmazenadorDeCargo armazenadorDeCargo,
            ExclusaoDeCargo exclusaoDeCargo,
            ICargoRepositorio cargoRepositorio)
        {
            _armazenadorDeCargo = armazenadorDeCargo;
            _exclusaoDeCargo = exclusaoDeCargo;
            _cargoRepositorio = cargoRepositorio;
        }

        // GET: api/cargos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _cargoRepositorio.ListarAsync();
                return Ok(results.Select(CargoDto.ConvertEntityToDto));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar a lista de cargos");
            }
        }

        // GET api/cargos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var cargo = await _cargoRepositorio.ObterPorIdAsync(id);
                return Ok(CargoDto.ConvertEntityToDto(cargo));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar o cargo informado");
            }
        }

        // POST api/cargos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CargoDto dto)
        {
            await _armazenadorDeCargo.Armazenar(dto);
            return Ok(true);
        }

        // PUT api/cargos/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] CargoDto dto)
        {
            var cargo = await _cargoRepositorio.ObterPorIdAsync(id);
            if (cargo != null)
            {
                dto.Id = id;
                await Post(dto);
            }
            else
            {
                throw new ArgumentException("O cargo informado não está cadastrado.");
            }
        }

        // DELETE api/cargos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _exclusaoDeCargo.Excluir(id);
                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Problema ao excluir o cargo informado");
            }
        }
    }
}
