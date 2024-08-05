using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;
using ModelNineMenMorris.Model;
using ModelNineMenMorris.Persistence;
using NMMView.ViewModel;

namespace NMMView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ModelNineMenMorris.Model.Table table_;
        private NMMModelView viewModel_;
        private MainWindow view_;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            table_ = new ModelNineMenMorris.Model.Table(new NMMDataAccess());
            viewModel_ = new NMMModelView(table_);
            table_.board.End_ += new EventHandler<NMMGameOverEventArgs>(Model_GameOver);
            viewModel_.ExitGame += new EventHandler(ViewModel_ExitGame);
            viewModel_.LoadGame += new EventHandler(ViewModel_LoadGame);
            viewModel_.SaveGame += new EventHandler(ViewModel_SaveGame);
            viewModel_.ShowMessage += new EventHandler<MessageEventArgs>(ViewModel_ShowMessage);
            view_ = new MainWindow();
            view_.DataContext = viewModel_;
            view_.Closing += new System.ComponentModel.CancelEventHandler(View_Closing);
            view_.Show();
        }

        private void Model_GameOver(object sender, NMMGameOverEventArgs e)
        {
            MessageBox.Show("End");
        }

        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            view_.Close();
        }

        private void ViewModel_ShowMessage(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e._str);
        }
        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Saving";
                saveFileDialog.Filter = "NMM|*.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await table_.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (NMMDataException)
                    {
                        MessageBox.Show("Saving failed!" + Environment.NewLine + "File cannot found", "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (FileFormatException)
                    {
                        MessageBox.Show("Cannot save, use txt files!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Cannot save!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }  
        }

        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Loading";
                openFileDialog.Filter = "NMM|*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    await table_.LoadGameAsync(openFileDialog.FileName);
                    viewModel_.Refresh();
                }
                table_.board.End_ += new EventHandler<NMMGameOverEventArgs>(Model_GameOver);
            }
            catch (FileFormatException)
            {
                MessageBox.Show("Cannot load, use txt files!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                viewModel_.OnNewGame();
            }
            catch (NMMDataException)
            {
                MessageBox.Show("Cannot load!" + Environment.NewLine + "Cannot find file!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                viewModel_.OnNewGame();
            }
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "NMM", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
