using System;
using System.Collections.ObjectModel;
using NMMModel.Model;
using NMMModel.Persistence;

namespace NMM.ViewModel
{
    public class NMMModelView : ViewModelBase
    {
       
        #region Properties
        private NMMModel.Model.Table table_;
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
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitGameCommand { get; private set; }
        public DelegateCommand ResumeCommand { get; private set; }
        #endregion
        #region Events
        public event EventHandler NewGame;
        public event EventHandler ResumeGame;
        public event EventHandler ExitGame;
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;
        public event EventHandler<MessageEventArgs> ShowMessage;
        #endregion
        #region Constructor
        public NMMModelView() { table_ = new NMMModel.Model.Table(new NMMDataAccess()); }
        public NMMModelView(NMMModel.Model.Table table)
        {
            table_ = table;
            Status = "Let's start!";
            table_.board.FromTo_ += new EventHandler<NMMFromToEventArgs>(Model_FromTo);
            table_.board.Remove_ += new EventHandler<NMMRemoveEventArgs>(Model_Remove);
            table_.board.MoveTo_ += new EventHandler<NMMMoveToEventArgs>(Model_MoveTo);
            table_.board.ChangeStatus_ += new EventHandler<NMMChangeSatusEventArgs>(Model_ChangeStatus);
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());
            ResumeCommand = new DelegateCommand(param => OnResume());
            addFields();
        }
        #endregion
        #region Private_Methods
        private void Model_FromTo(object sender, NMMFromToEventArgs e)
        {
            Fields[NMMField.convertBack(e.ind)].Color = e.color;
            Fields[NMMField.convertBack(e.transpInd)].Color = ColorFields.TRANSP;
        }
        private void Model_Remove(object sender, NMMRemoveEventArgs e)
        {
            Fields[NMMField.convertBack(e.transpInd)].Color = ColorFields.TRANSP;
        }
        private void Model_MoveTo(object sender, NMMMoveToEventArgs e)
        {
            Fields[NMMField.convertBack(e.ind)].Color = e.color;
        }
        private void Model_ChangeStatus(object sender, NMMChangeSatusEventArgs e)
        {
            Status = e.status;
        }
        private void OnButtonPush(int commandparameter)
        {
            
            try
            {
                table_.board.move(NMMField.convert(commandparameter));
            }
            catch (IncorrectIndexException)
            {
                ShowMessage.Invoke(this, new MessageEventArgs("Incorrect index!"));
            }
        }

        private void OnResume()
        {
            if (ResumeGame != null)
                ResumeGame(this, EventArgs.Empty);
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
           

            for (int i = 0; i < 49; i++)
            {
                LOCKED loc = LOCKED.YES;
                if(NMMField.convert(i)!=-1) {
                    loc = LOCKED.NO;
                }
                Fields.Add(new NMMField
                {
                    Color = ColorFields.TRANSP,
                    Ind = i,
                    Locked=loc,
                    PushCircleButton = new DelegateCommand(param => OnButtonPush(Convert.ToInt32(param)))
                });
            }
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
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }
        public void Refresh()
        {
            table_.board.FromTo_ += new EventHandler<NMMFromToEventArgs>(Model_FromTo);
            table_.board.Remove_ += new EventHandler<NMMRemoveEventArgs>(Model_Remove);
            table_.board.MoveTo_ += new EventHandler<NMMMoveToEventArgs>(Model_MoveTo);
            table_.board.ChangeStatus_ += new EventHandler<NMMChangeSatusEventArgs>(Model_ChangeStatus);
            for (int i=0; i < table_.board.FieldsColor.Length; i++)
            {
                Fields[NMMField.convertBack(i)].Color = table_.board.FieldsColor[i];
            }
        }
        #endregion
    }
}
