using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NMMModel.Persistence;

namespace NMMModel.Model
{
    /// <summary>
    /// Tárolt játékok kezelő modellje.
    /// </summary>
    public class StoredGameBrowserModel
    {
        private IStore _store; // a perzisztencia

        /// <summary>
        /// Tároló megváltozásának eseménye.
        /// </summary>
        public event EventHandler? StoreChanged;

        public StoredGameBrowserModel(IStore store)
        {
            _store = store;

            StoredGames = new List<StoredGameModel>();
        }

        /// <summary>
        /// A tárolt játékok listájának lekérdezése.
        /// </summary>
        public List<StoredGameModel> StoredGames { get; private set; }

        /// <summary>
        /// Tárolt játékok frissítése.
        /// </summary>
        public async Task UpdateAsync()
        {
            if (_store == null)
                return;

            StoredGames.Clear();

            // betöltjük a mentéseket
            foreach (String name in await _store.GetFilesAsync())
            {
                if (name == "SuspendedGame") // ezt a mentést nem akarjuk betölteni
                    continue;

                StoredGames.Add(new StoredGameModel
                {
                    Name = name,
                    Modified = await _store.GetModifiedTimeAsync(name)
                });
            }

            // dátum szerint rendezzük az elemeket
            StoredGames = StoredGames.OrderByDescending(item => item.Modified).ToList();
            
            OnSavesChanged();
        }

        private void OnSavesChanged()
        {
            StoreChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
