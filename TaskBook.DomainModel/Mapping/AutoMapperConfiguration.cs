using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace TaskBook.DomainModel.Mapping
{
    public sealed class AutoMapperConfiguration
    {
        public static void Initialize()
        {
            Mapper.Initialize(x => x.AddProfile<ViewModelToDomainMappingProfile>());
            Mapper.AssertConfigurationIsValid();
        }
    }
}
