using System;

namespace TaskBook.DomainModel.Mapping
{
    /// <summary>
    /// Defines mapping operations
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Makes mapping from the source object to the new target object based on the explicit type objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="sourceType">Source type</param>
        /// <param name="destinationType">Target type</param>
        /// <returns>Mapped target object</returns>
        object Map(object source, Type sourceType, Type destinationType);

        /// <summary>
        /// Makes mapping from the source object to the new target object based on the generic type parameters
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDest">Target type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>Mapped target object</returns>
        TDest Map<TSource, TDest>(TSource source);

        /// <summary>
        /// Makes mapping from the source object to the existing target object based on the explicit type objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Existing target object</param>
        /// <param name="sourceType">Source type</param>
        /// <param name="destinationType">Target type</param>
        /// <returns>Mapped target object</returns>
        object Map(object source, object destination, Type sourceType, Type destinationType);

        /// <summary>
        /// Makes mapping from the source object to the existing target object based on the generic type parameters
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDest">Target type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Existing target object</param>
        /// <returns>Mapped target object</returns>
        TDest Map<TSource, TDest>(TSource source, TDest destination);
    }
}
