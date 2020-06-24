using System;
using System.Linq;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Services;
using EmpresaApp.Domain.Services.Exclusoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApp.API.Controllers
{
    [Route("api/empresas")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly IArmazenadorDeEmpresa _armazenadorDeEmpresa;
        private readonly ExclusaoDeEmpresa _exclusaoDeEmpresa;
        private readonly IRepository<Empresa> _empresaRepositorio;

        public EmpresasController(
            IArmazenadorDeEmpresa armazenadorDeEmpresa, 
            ExclusaoDeEmpresa exclusaoDeEmpresa, 
            IRepository<Empresa> empresaRepositorio)
        {
            _armazenadorDeEmpresa = armazenadorDeEmpresa;
            _exclusaoDeEmpresa = exclusaoDeEmpresa;
            _empresaRepositorio = empresaRepositorio;
        }

        // GET: api/empresas
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _empresaRepositorio.List().Select(EmpresaDto.ConvertEntityToDto);
                return Ok(results);
            }
            catch
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
                var result = EmpresaDto.ConvertEntityToDto(_empresaRepositorio.GetById(id));
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
                _armazenadorDeEmpresa.Armazenar(dto);
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
            var result = _empresaRepositorio.GetById(id);
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
                _exclusaoDeEmpresa.Excluir(id);
                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Problema ao excluir a empresa informada");
            }
        }
    }
}
