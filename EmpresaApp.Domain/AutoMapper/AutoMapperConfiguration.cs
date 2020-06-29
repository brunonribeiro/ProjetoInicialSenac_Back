using AutoMapper;
using System;
using System.Collections.Generic;

namespace EmpresaApp.Domain.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        private static IEnumerable<Type> GetAutoMapperProfiles()
        {
            var result = new List<Type>
            {
                typeof(MappingProfile),
            };
            return result;
        }

        public static void Initialize()
        {
            Mapper.Initialize((cfg) =>
            {
                cfg.AddProfiles(GetAutoMapperProfiles());
            });
        }
    }
}
