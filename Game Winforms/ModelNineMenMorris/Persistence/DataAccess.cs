using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelNineMenMorris.Model;
namespace ModelNineMenMorris.Persistence
{
    public interface DataAccess
    {
        Task<Board> LoadAsync(String path);
        Task SaveAsync(String path, Board table);
    }
}
