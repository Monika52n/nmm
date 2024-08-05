using ModelNineMenMorris.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ModelNineMenMorris.Persistence
{
    public class NMMDataAccess : DataAccess
    {
        #region Load
        public async Task<Board> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    int turns = Convert.ToInt32(numbers[0]);
                    int blue1 = Convert.ToInt32(numbers[1]);
                    int green0 = Convert.ToInt32(numbers[2]);
                    Turns player_;
                    if (numbers[3]=="BLUE")
                    {
                        player_= Turns.BLUE;
                    } 
                    else
                    {
                        player_= Turns.GREEN;
                    }
                    bool remove = Convert.ToBoolean(numbers[4]);
                    Move moves;
                    if (numbers[5] == "CHOOSE")
                    {
                        moves = Move.CHOOSE;
                    }
                    else
                    {
                        moves = Move.PLACE;
                    }
                    int moveFrom = Convert.ToInt32(numbers[6]);
                    ColorFields[] colors = new ColorFields[24];
                    String line2 = await reader.ReadLineAsync() ?? String.Empty;
                    String[] allColors = line2.Split(' ');
                    for (int i=0; i<24; i++)
                    {
                        colors[i] = new ColorFields();
                        switch(Convert.ToInt32(allColors[i]))
                        {
                            case 0:
                                colors[i]=ColorFields.GREEN; break;
                            case 1:
                                colors[i] = ColorFields.BLUE; break;
                            case 2:
                                colors[i] = ColorFields.TRANSP; break;
                        }
                    }
                    Board table = new Board(colors, green0, blue1, turns, remove, player_, moves, moveFrom);
                    return table;
                }
            }
            catch
            {
                throw new NMMDataException();
            }
        }
        #endregion
        #region Save
        public async Task SaveAsync(String path, Board table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.Write(table.turnsValue + " " + table.blue1Value + " " + table.green0Value + " " + table.player_Value.ToString() + " " + table.removeValue + " " + table.movesValue.ToString() + " " + table.moveFromValue); // kiírjuk a méreteket
                    await writer.WriteLineAsync();
                    for (int i = 0; i < table.FieldsColor.Length; i++)
                    {
                        int out_ = (int)(table.FieldsColor[i]);
                        await writer.WriteAsync(out_ + " "); // kiírjuk az értékeket
                    }
                    await writer.WriteLineAsync();
                }
            }
            catch
            {
                throw new NMMDataException();
            }
        }
        #endregion
    }
    public class NMMDataException : Exception
    {
        public NMMDataException() { }
        public NMMDataException(string str) :base(str){ }
    }
}
