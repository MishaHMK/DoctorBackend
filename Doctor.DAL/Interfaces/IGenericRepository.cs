using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.DataAcsess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task SaveAsync();
    }
}
