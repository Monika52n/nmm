using NMMModel.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NMMModel.Model
{
    public class Table
    {
        private DataAccess IdataAccess;
        public Board board;
        public Table(DataAccess idataAccess)
        {
            IdataAccess = idataAccess;
            board = new Board();
        }
        #region SaveLoad
        public async Task SaveGameAsync(string path)
        {
            if (IdataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            if (Path.GetExtension(path) != ".txt")
                throw new FileFormatException();
            await IdataAccess.SaveAsync(path, board);
        }
        public async Task LoadGameAsync(string path)
        {
            if (IdataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            if (Path.GetExtension(path) != ".txt")
                throw new FileFormatException();
            board = await IdataAccess.LoadAsync(path);
        }
        #endregion
    }
}
public class FileFormatException : Exception
{
    public FileFormatException() { }
    public FileFormatException(string message) : base(message) { }
}
