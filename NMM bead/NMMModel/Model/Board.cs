using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml.Serialization;

namespace NMMModel.Model
{
    public class Board
    {
        #region Properties
        private Field[] fields;
        private int green0;
        private int blue1;
        private int turns;
        private bool remove;
        private Turns player_;
        private Move moves;
        private int moveFrom;
        private bool end_;
        #endregion
        #region Getters
        public int green0Value { get { return green0; } }
        public int blue1Value { get { return blue1; } }
        public int turnsValue { get { return turns; } }
        public bool removeValue { get { return remove; } }
        public Turns player_Value { get { return player_;  } }
        public Move movesValue { get { return moves; } }
        public int moveFromValue { get { return moveFrom; } }
        public ColorFields[] FieldsColor { get
            {
                ColorFields[] colors = new ColorFields[fields.Length];
                for(int i=0; i<fields.Length; i++)
                {
                    colors[i] = new ColorFields();
                    colors[i] = fields[i].player;
                }
                return colors;
            } }
        public bool endValue { get { return end_; } }
        #endregion
        #region Public_Methods
        public void move(int ind)
        {
            if (ind < 0 || ind > (fields.Length - 1)) throw new IncorrectIndexException();
            ColorFields color = tocolorFields(player_);
            int transpInd = -1;
            string status = "";
            if (!end_)
            {
                if (remove)
                {
                    if (removeOne(ind, player_))
                    {
                        remove = false;
                        player_ = switchPlayer(player_);
                        if(!canPlayerMove()) player_ = switchPlayer(player_);
                        status = toString(player_) + "'s turn";
                        Remove(ind);
                    }
                    else
                    {
                        status = "Could not remove!";
                    }
                }
                else if (turns > 0)
                {
                    if (!moveTo(ind, player_))
                    {
                        status = "Cannot place there!";
                    }
                    else if (threeNextToEachOther(ind))
                    {
                        remove = true;
                        status = "Removing one";
                        MoveTo(color, ind);
                    }
                    else
                    {
                        player_ = switchPlayer(player_);
                        status = toString(player_) + "'s turn";
                        MoveTo(color, ind);
                    }
                    if (turns == 0 && !canPlayerMove()) player_ = switchPlayer(player_); 
                }
                else //if (turns <= 0)
                {
                    if (moves == Move.CHOOSE && equals(fields[ind].player, player_) && canMove(ind))
                    {
                        moves = Move.PLACE;
                        moveFrom = ind;
                        status = toString(player_) + "'s turn to move";
                    }
                    else if (moves == Move.PLACE && fields[ind].player == ColorFields.TRANSP && moveFromTo(moveFrom, ind, player_))
                    {
                        moves = Move.CHOOSE;
                        if (threeNextToEachOther(ind))
                        {
                            remove = true;
                            status = "Removing one";
                            FromTo(color,  moveFrom, ind);
                        }
                        else
                        {
                            player_ = switchPlayer(player_);
                            if(!canPlayerMove()) player_ = switchPlayer(player_);
                            status = toString(player_) + "'s turn to choose a piece";
                            FromTo(color, moveFrom, ind);
                        }
                        transpInd = moveFrom;
                    }
                    else
                    {
                        status = "Cannot place or move here!";
                    }
                }
                if (blue1 < 3)
                {
                    status = "Green won!";
                    end_ = true;
                    GameOver("Green");

                }
                if (green0 < 3)
                {
                    status = "Blue won!";
                    end_ = true;
                    GameOver("Blue");
                }
            }
            else
            {
                status = "End";
            }
            ChangeStatus(status); 
        }
        public void newGame()
        {
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].player = ColorFields.TRANSP;
                player_ = Turns.BLUE;
                green0 = 9;
                blue1 = 9;
                turns = 18;
                remove = false;
                moves = Move.CHOOSE;
                end_ = false;
            }
        }
        #endregion
        #region Private_Methods
        private string toString(Turns p)
        {
            string str = p.ToString();
            str = str[0] + str.Substring(1).ToLower();
            return str;
        }
        private Turns switchPlayer(Turns p)
        {
            if (p == Turns.GREEN) return Turns.BLUE;
            else return Turns.GREEN;
        }
        private bool equals(ColorFields a, Turns b)
        {
            if (a == ColorFields.GREEN && b == Turns.GREEN || a == ColorFields.BLUE && b == Turns.BLUE)
            {
                return true;
            }
            return false;
        }
        private ColorFields tocolorFields(Turns a)
        {
            switch (a)
            {
                case Turns.BLUE: return ColorFields.BLUE;
                case Turns.GREEN: return ColorFields.GREEN;
            }
            return ColorFields.TRANSP;
        }
        private bool moveTo(int ind, Turns player)
        {
            if (fields[ind].player != ColorFields.TRANSP)
            {
                return false;
            }
            fields[ind].player = tocolorFields(player);
            turns--;
            return true;
        }
        private bool moveFromTo(int indFrom, int indTo, Turns player)
        {
            if (fields[indFrom].Right != fields[indTo] && fields[indFrom].Left != fields[indTo] && fields[indFrom].Up != fields[indTo] && fields[indFrom].Down != fields[indTo])
            {
                if (player == Turns.GREEN && green0 != 3 || player == Turns.BLUE && blue1 != 3) return false;
            }
            fields[indFrom].player = ColorFields.TRANSP;
            fields[indTo].player = tocolorFields(player);
            return true;
        }
        private bool canMove(int ind)
        {
            if (player_ == Turns.BLUE && blue1 <= 3 || player_ == Turns.GREEN && green0 <= 3) return true;
            if (fields[ind].Right != null && fields[ind].Right.player == ColorFields.TRANSP ||
                fields[ind].Left != null && fields[ind].Left.player == ColorFields.TRANSP ||
                fields[ind].Up != null && fields[ind].Up.player == ColorFields.TRANSP ||
                fields[ind].Down != null && fields[ind].Down.player == ColorFields.TRANSP)
            {
                return true;
            }
            return false;
        }
        private bool canPlayerMove()
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (equals(fields[i].player, player_) && canMove(i))
                    return true;
            }
            return false;
        }
        private bool removeOne(int ind, Turns player)
        {
            if (fields[ind].player == tocolorFields(player) || fields[ind].player == ColorFields.TRANSP || !allNextToEachOther(switchPlayer(player_)) && threeNextToEachOther(ind)) return false;
            if (fields[ind].player == ColorFields.GREEN) green0--;
            if (fields[ind].player == ColorFields.BLUE) blue1--;
            fields[ind].player = ColorFields.TRANSP;
            return true;
        }
        private bool threeNextToEachOther(int ind)
        {
            if (fields[ind].player == ColorFields.TRANSP) return false;
            if (fields[ind].Right != null)
            {
                if (fields[ind].Left != null && fields[ind].Right.player == fields[ind].player && fields[ind].player == fields[ind].Left.player)
                {
                    return true;
                }
                else if (fields[ind].Right.Right != null && fields[ind].player == fields[ind].Right.player && fields[ind].player == fields[ind].Right.Right.player)
                {
                    return true;
                }
            }
            if (fields[ind].Up != null)
            {
                if (fields[ind].Down != null && fields[ind].player == fields[ind].Up.player && fields[ind].player == fields[ind].Down.player)
                {
                    return true;
                }
                else if (fields[ind].Up.Up != null && fields[ind].player == fields[ind].Up.player && fields[ind].player == fields[ind].Up.Up.player)
                {
                    return true;
                }
            }
            if (fields[ind].Left != null && fields[ind].Left.Left != null)
            {
                if (fields[ind].player == fields[ind].Left.player && fields[ind].player == fields[ind].Left.Left.player)
                {
                    return true;
                }
            }
            if (fields[ind].Down != null && fields[ind].Down.Down != null)
            {
                if (fields[ind].player == fields[ind].Down.player && fields[ind].player == fields[ind].Down.Down.player)
                {
                    return true;
                }
            }
            return false;
        }
        private bool allNextToEachOther(Turns player)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (equals(fields[i].player, player) && !threeNextToEachOther(i))
                {
                    return false;
                }
            }
            return true;
        }
  
        #endregion
        #region Constructors
        public Board()
        {
            green0 = 9;
            blue1 = 9;
            turns = 18;
            remove = false;
            player_ = Turns.BLUE;
            moves = Move.CHOOSE;
            end_ = false;
            fields = new Field[24];
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = new Field();
            }
            for (int i = 0; i < fields.Length; i++)
            {
                if (i % 3 != 2)
                {
                    fields[i].Right = fields[i + 1];
                }
                if (i % 3 != 0)
                {
                    fields[i].Left = fields[i - 1];
                }
            }
            fields[4].Up = fields[1];
            fields[4].Down = fields[7];
            fields[19].Up = fields[16];
            fields[19].Down = fields[22];
            fields[9].Up = fields[0];
            fields[9].Down = fields[21];
            fields[10].Up = fields[3];
            fields[10].Down = fields[18];
            fields[11].Up = fields[6];
            fields[11].Down = fields[15];
            fields[12].Up = fields[8];
            fields[12].Down = fields[17];
            fields[13].Up = fields[5];
            fields[13].Down = fields[20];
            fields[14].Up = fields[2];
            fields[14].Down = fields[23];
        }
        public Board(ColorFields[] colors, int green0, int blue1, int turns, bool remove, Turns player_, Move moves, int moveFrom)
        {
            fields = new Field[24];
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = new Field();
            }
            for (int i = 0; i < fields.Length; i++)
            {
                if (i % 3 != 2)
                {
                    fields[i].Right = fields[i + 1];
                }
                if (i % 3 != 0)
                {
                    fields[i].Left = fields[i - 1];
                }
            }
            fields[4].Up = fields[1];
            fields[4].Down = fields[7];
            fields[19].Up = fields[16];
            fields[19].Down = fields[22];
            fields[9].Up = fields[0];
            fields[9].Down = fields[21];
            fields[10].Up = fields[3];
            fields[10].Down = fields[18];
            fields[11].Up = fields[6];
            fields[11].Down = fields[15];
            fields[12].Up = fields[8];
            fields[12].Down = fields[17];
            fields[13].Up = fields[5];
            fields[13].Down = fields[20];
            fields[14].Up = fields[2];
            fields[14].Down = fields[23];
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].player = colors[i];
            }
            this.green0 = green0;
            this.blue1 = blue1;
            this.turns = turns;
            this.remove = remove;
            this.player_ = player_;
            this.moves = moves;
            this.moveFrom = moveFrom;
            this.end_ = false;
        }
        #endregion
        #region Events
        public event EventHandler<NMMMoveToEventArgs>? MoveTo_;
        public event EventHandler<NMMRemoveEventArgs>? Remove_;
        public event EventHandler<NMMFromToEventArgs>? FromTo_;
        public event EventHandler<NMMGameOverEventArgs>? End_;
        public event EventHandler<NMMChangeSatusEventArgs>? ChangeStatus_;
        private void ChangeStatus(string status)
        {
            ChangeStatus_?.Invoke(this, new NMMChangeSatusEventArgs(status));
        }
        private void MoveTo(ColorFields color, int ind)
        {
            MoveTo_?.Invoke(this, new NMMMoveToEventArgs(color, ind));
        }
        private void FromTo(ColorFields color, int from, int to)
        {
            FromTo_?.Invoke(this, new NMMFromToEventArgs(color, to, from));
        }
        private void Remove(int ind)
        {
            Remove_?.Invoke(this, new NMMRemoveEventArgs(ind));
        }
        private void GameOver(string winner)
        {
            End_?.Invoke(this, new NMMGameOverEventArgs(winner));
        }

        #endregion
    }
}
public class IncorrectIndexException : Exception
{
    public IncorrectIndexException() { }
    public IncorrectIndexException(string str) : base(str) { }
}