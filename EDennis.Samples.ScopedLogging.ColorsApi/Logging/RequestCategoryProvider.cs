using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Logging
{

    public interface IRequestCategoryProvider
    {
        string CreateCategory<T>();
    }

    public class RequestCategoryProvider : IRequestCategoryProvider
    {

        public RequestCategoryProvider() {
        }

        public string CreateCategory<T>() {
            return typeof(T).Name;
        }
    }
}
