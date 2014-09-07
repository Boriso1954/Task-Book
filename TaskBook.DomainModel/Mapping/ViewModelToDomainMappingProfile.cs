using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.DomainModel.Mapping
{
    public sealed class ViewModelToDomainMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<TaskVm, TbTask>();
        }
    }
}
