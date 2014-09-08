using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel.Mapping
{
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type destinationType);
        TDest Map<TSource, TDest>(TSource source);
        object Map(object source, object destination, Type sourceType, Type destinationType);
        TDest Map<TSource, TDest>(TSource source, TDest destination);
    }
}
