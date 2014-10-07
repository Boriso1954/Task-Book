using AutoMapper;

namespace TaskBook.DomainModel.Mapping
{
    /// <summary>
    /// Keep the mapping expression available, so it can be used in the fluent API
    /// The standrd ForAllMembers just returns void
    /// </summary>
    /// <remarks>See for details: http://stackoverflow.com/questions/4367591/automapper-how-to-ignore-all-destination-members-except-the-ones-that-are-mapp</remarks>
    public static class MappingExpressionExtensions
    {
        /// <summary>
        /// Allows ignoring all the unmapped members
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDest">Target type</typeparam>
        /// <param name="expression">Mapping expression</param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDest> IgnoreUnmappedMembers<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(o => o.Ignore());
            return expression;
        }
    }
}
