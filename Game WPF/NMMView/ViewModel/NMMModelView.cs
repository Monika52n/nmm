using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ModelNineMenMorris.Model;

namespace NMMView.ViewModel
{
    public class NMMModelView : ViewModelBase
    {
        #region Properties
        private ModelNineMenMorris.Model.Table table_;
        public ObservableCollection<NMMField> Fields { get; set; }
        private String status;
        public String Status { get { return status; } 
            private set
            {
                status = value;
                OnPropertyChanged();
            } }
        #endregion
        #region Commands
        public MyCommand NewGameCommand { get; private set; }
        public MyCommand LoadGameCommand { get; private set; }
        public MyCommand SaveGameCommand { get; private set; }
        public MyCommand ExitGameCommand { get; private set; }
        #endregion
        #region Events
        public event EventHandler ExitGame;
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;
        public event EventHandler<MessageEventArgs> ShowMessage;
        #endregion
        #region Constructor
        public NMMModelView(ModelNineMenMorris.Model.Table table)
        {
            table_ = table;
            Status = "Let's start!";
            table_.board.FromTo_ += new EventHandler<NMMFromToEventArgs>(Model_FromTo);
            table_.board.Remove_ += new EventHandler<NMMRemoveEventArgs>(Model_Remove);
            table_.board.MoveTo_ += new EventHandler<NMMMoveToEventArgs>(Model_MoveTo);
            table_.board.ChangeStatus_ += new EventHandler<NMMChangeSatusEventArgs>(Model_ChangeStatus);
            NewGameCommand = new MyCommand(param => OnNewGame());
            LoadGameCommand = new MyCommand(param => OnLoadGame());
            SaveGameCommand = new MyCommand(param => OnSaveGame());
            ExitGameCommand = new MyCommand(param => OnExitGame());
            addFields();
        }
        #endregion
        #region Private_Methods
        private void Model_FromTo(object sender, NMMFromToEventArgs e)
        {
            Fields[e.ind].Color = e.color;
            Fields[e.transpInd].Color = ColorFields.TRANSP;
        }
        private void Model_Remove(object sender, NMMRemoveEventArgs e)
        {
            Fields[e.transpInd].Color = ColorFields.TRANSP;
        }
        private void Model_MoveTo(object sender, NMMMoveToEventArgs e)
        {
            Fields[e.ind].Color = e.color;
        }
        private void Model_ChangeStatus(object sender, NMMChangeSatusEventArgs e)
        {
            Status = e.status;
        }
        private void OnButtonPush(int commandparameter)
        {
            try
            {
                table_.board.move(commandparameter);
            }
            catch (IncorrectIndexException)
            {
                ShowMessage.Invoke(this, new MessageEventArgs("Incorrect index!"));
            }
        }
        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }
        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }
        private void addFields()
        {
            Fields = new ObservableCollection<NMMField>();
            double[] positions = { 2, 77, 154, 240.5, 240.5, 325, 402, 479 };

            for (int i = 0; i < 24; i++)
            {

                Fields.Add(new NMMField
                {
                    Color = ColorFields.TRANSP,
                    Ind = i,
                    Top = positions[i / 3],
                    Size = 49,
                    PushCircleButton = new MyCommand(param => OnButtonPush(Convert.ToInt32(param)))
                });
            }
            Fields[0].Left = positions[0];
            Fields[1].Left = positions[3];
            Fields[2].Left = positions[7];
            Fields[3].Left = positions[1];
            Fields[4].Left = positions[3];
            Fields[5].Left = positions[6];
            Fields[6].Left = positions[2];
            Fields[7].Left = positions[3];
            Fields[8].Left = positions[5];
            Fields[9].Left = positions[0];
            Fields[10].Left = positions[1];
            Fields[11].Left = positions[2];
            Fields[12].Left = positions[5];
            Fields[13].Left = positions[6];
            Fields[14].Left = positions[7];
            Fields[15].Left = positions[2];
            Fields[16].Left = positions[3];
            Fields[17].Left = positions[5];
            Fields[18].Left = positions[1];
            Fields[19].Left = positions[3];
            Fields[20].Left = positions[6];
            Fields[21].Left = positions[0];
            Fields[22].Left = positions[3];
            Fields[23].Left = positions[7];
        }
        #endregion
        #region Public_Methods
        public void OnNewGame()
        {
            table_.board.newGame();
            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].Color = ColorFields.TRANSP;
            }
        }
        public void Refresh()
        {
            table_.board.FromTo_ += new EventHandler<NMMFromToEventArgs>(Model_FromTo);
            table_.board.Remove_ += new EventHandler<NMMRemoveEventArgs>(Model_Remove);
            table_.board.MoveTo_ += new EventHandler<NMMMoveToEventArgs>(Model_MoveTo);
            for (int i=0; i < Fields.Count; i++)
            {
                Fields[i].Color = table_.board.FieldsColor[i];
            }
        }
        #endregion
    }

    public class MessageEventArgs : EventArgs
    {
        private string str;
        public string _str { get { return _str; } }
        public MessageEventArgs(string text)
        {
            str = text;
        }
    }
}
