using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmpresaApp.Domain.Services
{
    public class ServiceDefault<TDto, TEntity> where TEntity : Entity<TDto> where TDto : DtoBase
    {
        public static Func<TEntity, TDto> ConvertEntityToDto { get; set; }
        public static Func<int, TEntity> ById { get; set; }

        public ServiceDefault(Func<TEntity, TDto> convertEntityToDto, Func<int, TEntity> getById)
        {
            ConvertEntityToDto = convertEntityToDto;
            ById = getById;

        }
        public TDto GetById(int? id)
        {
            if (id.HasValue)
            {
                return ConvertEntityToDto(ById(id.Value));
            }

            return null;
        }

        public List<TDto> List(Func<List<TEntity>> list)
        {
            return list().Select(ConvertEntityToDto).ToList();
        }

        public bool Remove(int? id, Action<TEntity> remove)
        {
            var entity = ById(id.Value);
            if (entity != null)
            {
                remove(entity);
                return true;
            }
            return false;
        }

        public void Save(TDto dto, Func<TDto, TEntity> create, Action<TEntity> save)
        {
            var entity = ById(dto.Id);
            if (entity == null)
            {
                entity = create(dto);
                save(entity);
                dto.Id = entity.Id;
            }
            else
            {
                entity.Update(dto);
            }
        }
    }
}
