using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Services;
using EmpresaApp.Domain.Services.Exclusoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApp.API.Controllers
{
    [Route("api/funcionarios")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly IArmazenadorDeFuncionario _armazenadorDeFuncionario;
        private readonly ExclusaoDeFuncionario _exclusaoDeFuncionario;
        private readonly IRepository<Funcionario> _funcionarioRepositorio;

        public FuncionariosController(
            IArmazenadorDeFuncionario armazenadorDeFuncionario, 
            ExclusaoDeFuncionario exclusaoDeFuncionario, 
            IRepository<Funcionario> funcionarioRepositorio)
        {
            _armazenadorDeFuncionario = armazenadorDeFuncionario;
            _exclusaoDeFuncionario = exclusaoDeFuncionario;
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        // GET: api/funcionarios
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _funcionarioRepositorio.List();
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
                var result = _funcionarioRepositorio.GetById(id);
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
                _armazenadorDeFuncionario.Armazenar(dto);
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
            var result = _funcionarioRepositorio.GetById(id);
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
                _exclusaoDeFuncionario.Excluir(id);
                return Ok(true);
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
            _armazenadorDeFuncionario.AdicionarEmpresa(id, idEmpresa);
        }

        // PUT api/funcionarios/5/atribuircargo/8
        [HttpPut("{id}/atribuircargo/{idCargo}")]
        public void AtribuirCargo(int id, int idCargo)
        {
            _armazenadorDeFuncionario.AdicionarCargo(id, idCargo); ;
        }
    }
}
