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
    [Route("api/funcionarios")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly FuncionarioService _funcionarioService;
        private readonly EmpresaService _empresaService;
        private readonly CargoService _cargoService;

        public FuncionariosController(FuncionarioService funcionarioService, EmpresaService empresaService, CargoService cargoService)
        {
            _funcionarioService = funcionarioService;
            _empresaService = empresaService;
            _cargoService = cargoService;
        }

        // GET: api/funcionarios
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _funcionarioService.List();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar a lista de funcionários");
            }
        }

        // GET api/funcionarios/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _funcionarioService.GetById(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar o funcionário informado");
            }
        }

        // POST api/funcionarios
        [HttpPost]
        public void Post([FromBody] FuncionarioDto dto)
        {
            if (ModelState.IsValid)
            {
                _funcionarioService.Save(dto);
            }
            else
            {
                throw new ArgumentException("O objeto informado está inválido.");
            }
        }

        // PUT api/funcionarios/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] FuncionarioDto dto)
        {
            var result = _funcionarioService.GetById(id);
            if (result != null)
            {
                dto.Id = id;
                Post(dto);
            }
            else
            {
                throw new ArgumentException("O funcionário informado não está cadastrado.");
            }
        }

        // DELETE api/funcionarios/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_funcionarioService.Remove(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao excluir o funcionário informado");
            }
        }

        // PUT api/funcionarios/5/vincularempresa/8
        [HttpPut("{id}/vincularempresa/{idEmpresa}")]
        public void VincularEmpresa(int id, int idEmpresa)
        {
            var funcionario = _funcionarioService.GetById(id);
            if(funcionario == null)
                throw new ArgumentException("O funcionário informado não está cadastrado.");

            var empresa = _empresaService.GetById(idEmpresa);
            if (empresa == null)
                throw new ArgumentException("A empresa informada não está cadastrada.");

            funcionario.EmpresaId = idEmpresa;
            Post(funcionario);
        }

        // PUT api/funcionarios/5/atribuircargo/8
        [HttpPut("{id}/atribuircargo/{idCargo}")]
        public void AtribuirCargo(int id, int idCargo)
        {
            var funcionario = _funcionarioService.GetById(id);
            if (funcionario == null)
                throw new ArgumentException("O funcionário informado não está cadastrado.");

            var cargo = _cargoService.GetById(idCargo);
            if (cargo == null)
                throw new ArgumentException("O cargo informado não está cadastrado.");

            funcionario.CargoId = idCargo;
            _funcionarioService.AddCargo(funcionario);
        }
    }
}
