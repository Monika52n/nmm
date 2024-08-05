using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMMModel.Model;
namespace NMMModel.Persistence
{
    public interface DataAccess
    {
        Task<Board> LoadAsync(String path);
        Task SaveAsync(String path, Board table);
    }
}
