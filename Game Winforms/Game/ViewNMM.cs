using System.CodeDom;
using System.CodeDom.Compiler;
using System.DirectoryServices;
using System.Drawing.Drawing2D;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using ModelNineMenMorris.Model;
using ModelNineMenMorris.Persistence;
namespace NineMenMorris
{
    public partial class ViewNMM : Form
    {
        private DataAccess dataAccess;
        private Table table;
        public ViewNMM()
        {
            InitializeComponent();
            dataAccess = new NMMDataAccess();
            table = new Table(dataAccess);
            table.board.MoveTo_ += new EventHandler<NMMMoveToEventArgs>(Move_To);
            table.board.FromTo_ += new EventHandler<NMMFromToEventArgs>(From_To);
            table.board.Remove_ += new EventHandler<NMMRemoveEventArgs>(Remove);
            table.board.End_ += new EventHandler<NMMGameOverEventArgs>(Game_Over);
            table.board.ChangeStatus_ += new EventHandler<NMMChangeSatusEventArgs>(Change_Status);
        }
        #region Board_Methods
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen blkpen =new Pen(Color.FromArgb(255, 0, 0, 0), 5);
            e.Graphics.DrawRectangle(blkpen, 35, 35, 690, 690);
            e.Graphics.DrawRectangle(blkpen, 145, 145, 470, 470);
            e.Graphics.DrawRectangle(blkpen, 255, 255, 250, 250);
            e.Graphics.DrawLine(blkpen, new Point(380, 35), new Point(380, 255));
            e.Graphics.DrawLine(blkpen, new Point(380, 505), new Point(380, 725));
            e.Graphics.DrawLine(blkpen, new Point(35, 380), new Point(255, 380));
            e.Graphics.DrawLine(blkpen, new Point(505, 380), new Point(725, 380));
        }
        private void circleButton_Click(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            if(button != null)
            {
                try
                {
                    table.board.move(button.TabIndex);
                }
                catch(IncorrectIndexException)
                {
                    toolStripStatusLabelGame.Text = "Incorrect index!";
                }
            }
        }
        private void Move_To(Object? sender, NMMMoveToEventArgs e)
        {
            foreach (Control x in this.panel1.Controls)
            {
                if (x.TabIndex == e.ind)
                {
                    switch (e.color)
                    {
                        case ColorFields.BLUE: x.BackColor = Color.Blue; break;
                        case ColorFields.GREEN: x.BackColor = Color.Green; break;
                    }
                }
            }
        }
        private void From_To(Object? sender, NMMFromToEventArgs e)
        {
            foreach (Control x in this.panel1.Controls)
            {
                if (x.TabIndex == e.ind)
                {
                    switch (e.color)
                    {
                        case ColorFields.BLUE: x.BackColor = Color.Blue; break;
                        case ColorFields.GREEN: x.BackColor = Color.Green; break;
                    }
                }
                if(x.TabIndex == e.transpInd)
                {
                    x.BackColor = Color.Transparent;
                }
            }
        }
        private void Remove(Object? sender, NMMRemoveEventArgs e)
        {
            foreach (Control x in this.panel1.Controls)
            {
                if (x.TabIndex == e.transpInd)
                {
                    x.BackColor = Color.Transparent;
                }
            }
        }
        private void Game_Over(Object? sender, NMMGameOverEventArgs e)
        {
            foreach(Control x in this.panel1.Controls)
            {
                x.Enabled = false;
            }
        }
        private void Change_Status(Object? sender, NMMChangeSatusEventArgs e)
        {
            toolStripStatusLabelGame.Text = e.status;
        }
        #endregion
        #region File
        private async void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openNMMDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await table.LoadGameAsync(openNMMDialog.FileName);
                    for (int i= 0; i < table.board.FieldsColor.Length; i++)
                    {
                        foreach (Control x in this.panel1.Controls)
                        {
                            x.Enabled = true;
                            if (x.TabIndex == i)
                            {
                                switch (table.board.FieldsColor[i])
                                {
                                    case ColorFields.BLUE:
                                        x.BackColor = Color.Blue; break;
                                    case ColorFields.GREEN:
                                        x.BackColor = Color.Green; break;
                                    case ColorFields.TRANSP:
                                        x.BackColor = Color.Transparent; break;
                                }
                            }
                        }
                    }
                    table.board.MoveTo_ += new EventHandler<NMMMoveToEventArgs>(Move_To);
                    table.board.FromTo_ += new EventHandler<NMMFromToEventArgs>(From_To);
                    table.board.Remove_ += new EventHandler<NMMRemoveEventArgs>(Remove);
                    table.board.End_ += new EventHandler<NMMGameOverEventArgs>(Game_Over);
                    table.board.ChangeStatus_ += new EventHandler<NMMChangeSatusEventArgs>(Change_Status);
                }
                catch (NMMDataException)
                {
                    MessageBox.Show("Cannot load!" + Environment.NewLine + "Cannot find file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    table.board.newGame();
                    foreach (Control x in this.panel1.Controls)
                    {
                        x.BackColor = Color.Transparent;
                    }
                }
                catch (FileFormatException)
                {
                    MessageBox.Show("Cannot load, use txt files!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    table.board.newGame();
                    foreach (Control x in this.panel1.Controls)
                    {
                        x.BackColor = Color.Transparent;
                    }
                }
            }
        }
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
           table.board.newGame();
           foreach (Control x in this.panel1.Controls)
           {
                x.Enabled = true;
                x.BackColor = Color.Transparent;
           }
        }
        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_saveNMMDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await table.SaveGameAsync(_saveNMMDialog.FileName);
                }
                catch (NMMDataException)
                {
                    MessageBox.Show("Saving failed!" + Environment.NewLine + "File cannot found", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (FileFormatException)
                {
                    MessageBox.Show("Cannot save, use txt files!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Nine Men's Morris", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }
        #endregion
    }
}