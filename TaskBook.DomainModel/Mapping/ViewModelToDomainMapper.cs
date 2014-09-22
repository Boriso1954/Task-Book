using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace TaskBook.DomainModel.Mapping
{
    /// <summary>
    /// Provides the mapping
    /// </summary>
    public sealed class ViewModelToDomainMapper: IMapper
    {
        /// <summary>
        /// Makes mapping from the source object to the new target object based on the explicit type objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="sourceType">Source type</param>
        /// <param name="destinationType">Target type</param>
        /// <returns>Mapped target object</returns>
        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }

        /// <summary>
        /// Makes mapping from the source object to the new target object based on the generic type parameters
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDest">Target type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>Mapped target object</returns>
        public TDest Map<TSource, TDest>(TSource source)
        {
            return Mapper.Map<TSource, TDest>(source);
        }

        /// <summary>
        /// Makes mapping from the source object to the existing target object based on the explicit type objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Existing target object</param>
        /// <param name="sourceType">Source type</param>
        /// <param name="destinationType">Target type</param>
        /// <returns>Mapped target object</returns>
        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, destination, sourceType, destinationType);
        }

        /// <summary>
        /// Makes mapping from the source object to the existing target object based on the generic type parameters
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDest">Target type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Existing target object</param>
        /// <returns>Mapped target object</returns>
        public TDest Map<TSource, TDest>(TSource source, TDest destination)
        {
            return Mapper.Map<TSource, TDest>(source, destination);
        }


    }
}
