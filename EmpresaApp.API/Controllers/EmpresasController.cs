using System;
using System.Linq;
using System.Threading.Tasks;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Interfaces.Armazenadores;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Services.Exclusoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApp.API.Controllers
{
    [Route("api/empresas")]
    [ApiController]
    public class EmpresasController : BaseController
    {
        private readonly IArmazenadorDeEmpresa _armazenadorDeEmpresa;
        private readonly ExclusaoDeEmpresa _exclusaoDeEmpresa;
        private readonly IEmpresaRepositorio _empresaRepositorio;

        public EmpresasController(
            IArmazenadorDeEmpresa armazenadorDeEmpresa,
            ExclusaoDeEmpresa exclusaoDeEmpresa,
            IEmpresaRepositorio empresaRepositorio,
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio) :
            base(notificacaoDeDominio)
        {
            _armazenadorDeEmpresa = armazenadorDeEmpresa;
            _exclusaoDeEmpresa = exclusaoDeEmpresa;
            _empresaRepositorio = empresaRepositorio;
        }

        // GET: api/empresas
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _empresaRepositorio.ListarAsync();
                return Ok(results.Select(EmpresaDto.ConvertEntityToDto));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar a lista de empresas");
            }
        }

        // GET api/empresas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _empresaRepositorio.ObterPorIdAsync(id);
                return Ok(EmpresaDto.ConvertEntityToDto(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problema ao recuperar a empresa informada");
            }
        }

        // POST api/empresas
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmpresaDto dto)
        {
            await _armazenadorDeEmpresa.Armazenar(dto);

            if (!OperacaoValida())
                return BadRequestResponse();

            return Ok(true);
        }

        // PUT api/empresas/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] EmpresaDto dto)
        {
            var result = await _empresaRepositorio.ObterPorIdAsync(id);
            if (result != null)
            {
                dto.Id = id;
                await Post(dto);
            }
            else
            {
                throw new ArgumentException("A empresa informado não está cadastrada.");
            }
        }

        // DELETE api/empresas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exclusaoDeEmpresa.Excluir(id);

            if (!OperacaoValida())
                return BadRequestResponse();

            return Ok(true);
        }
    }
}
