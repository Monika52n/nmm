using System;

namespace NMMModel.Model
{
    /// <summary>
    /// Tárolt játék modellje.
    /// </summary>
    public class StoredGameModel
    {
        /// <summary>
        /// Név lekérdezése, vagy beállítása.
        /// </summary>
        public String Name { get; set; } = String.Empty;

        /// <summary>
        /// Módosítás idejének lekérdezése, vagy beállítása.
        /// </summary>
        public DateTime Modified { get; set; }
    }
}
