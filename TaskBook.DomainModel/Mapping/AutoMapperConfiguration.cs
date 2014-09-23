using AutoMapper;

namespace TaskBook.DomainModel.Mapping
{
    /// <summary>
    /// AutoMapper configuration class
    /// </summary>
    public sealed class AutoMapperConfiguration
    {
        /// <summary>
        /// Registers profiles
        /// </summary>
        public static void Initialize()
        {
            Mapper.Initialize(x => x.AddProfile<ViewModelToDomainMappingProfile>());
            Mapper.AssertConfigurationIsValid();
        }
    }
}
