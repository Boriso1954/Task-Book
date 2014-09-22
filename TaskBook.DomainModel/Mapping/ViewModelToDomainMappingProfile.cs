using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.DomainModel.Mapping
{
    /// <summary>
    /// AutoMapper profile class for mapping from view models to domain objects
    /// </summary>
    public sealed class ViewModelToDomainMappingProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            // Creates the mapping from TaskVm view model to TbTask domain object
            Mapper.CreateMap<TaskVm, TbTask>()
                .IgnoreUnmappedMembers()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.TaskId))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.ProjectId))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
                .ForMember(d => d.DueDate, o => o.MapFrom(s => s.DueDate))
                .ForMember(d => d.CompletedDate, o => o.MapFrom(s => s.CompletedDate))
                .ForMember(d => d.Status, o => o.ResolveUsing<TaskStatusResolver>().FromMember(s => s.Status));

            // Creates the mapping from TbUserRoleVm view model to TbUser domain object
            Mapper.CreateMap<TbUserRoleVm, TbUser>()
                .IgnoreUnmappedMembers()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email));
        }
    }

    internal sealed class TaskStatusResolver: ValueResolver<string, TbTaskStatus>
    {
        protected override TbTaskStatus ResolveCore(string source)
        {
            TbTaskStatus status = TbTaskStatus.New;
            switch(source)
            {
                case "New":
                    status = TbTaskStatus.New;
                    break;
                case "In Progress":
                    status = TbTaskStatus.InProgress;
                    break;
                case "Completed":
                    status = TbTaskStatus.Completed;
                    break;
            }
            return status;
        }
    }
}
