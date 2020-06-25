using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Interfaces.Armazenadores;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Services;
using EmpresaApp.Domain.Services.Exclusoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApp.API.Controllers
{
    [Route("api/funcionarios")]
    [ApiController]
    public class FuncionariosController : BaseController
    {
        private readonly IArmazenadorDeFuncionario _armazenadorDeFuncionario;
        private readonly ExclusaoDeFuncionario _exclusaoDeFuncionario;
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public FuncionariosController(
            IArmazenadorDeFuncionario armazenadorDeFuncionario,
            ExclusaoDeFuncionario exclusaoDeFuncionario,
            IFuncionarioRepositorio funcionarioRepositorio,
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio) :
            base(notificacaoDeDominio)
        {
            _armazenadorDeFuncionario = armazenadorDeFuncionario;
            _exclusaoDeFuncionario = exclusaoDeFuncionario;
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        // GET: api/funcionarios
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _funcionarioRepositorio.ObterListaFuncionarioComEmpresaECargo();
                return Ok(results.Select(FuncionarioListarDto.ConvertEntityToDto));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar a lista de funcionários");
            }
        }

        // GET api/funcionarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(id);
                return Ok(FuncionarioDto.ConvertEntityToDto(funcionario));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar o funcionário informado");
            }
        }

        // POST api/funcionarios
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FuncionarioDto dto)
        {
            await _armazenadorDeFuncionario.Armazenar(dto);

            if (!OperacaoValida())
                return BadRequestResponse();

            return Ok(true);
        }

        // PUT api/funcionarios/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] FuncionarioDto dto)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(id);
            if (funcionario != null)
            {
                dto.Id = id;
                await Post(dto);
            }
            else
            {
                throw new ArgumentException("O funcionário informado não está cadastrado.");
            }
        }

        // DELETE api/funcionarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exclusaoDeFuncionario.Excluir(id);

            if (!OperacaoValida())
                return BadRequestResponse();

            return Ok(true);
        }

        // PUT api/funcionarios/5/vincularempresa/8
        [HttpPut("{id}/vincularempresa/{idEmpresa}")]
        public async Task<IActionResult> VincularEmpresa(int id, int idEmpresa)
        {
            await _armazenadorDeFuncionario.AdicionarEmpresa(id, idEmpresa);

            if (!OperacaoValida())
                return BadRequestResponse();

            return Ok(true);
        }

        // PUT api/funcionarios/5/atribuircargo/8
        [HttpPut("{id}/atribuircargo/{idCargo}")]
        public async Task<IActionResult> AtribuirCargo(int id, int idCargo)
        {
            await _armazenadorDeFuncionario.AdicionarCargo(id, idCargo);

            if (!OperacaoValida())
                return BadRequestResponse();

            return Ok(true);
        }
    }
}
