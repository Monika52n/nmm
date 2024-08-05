using NMMModel.Model;
using NMM.ViewModel;
using NMMModel.Persistence;
using NMM.View;
using Microsoft.Maui.Controls;
//using Android.Provider;
//using Android.OS;

namespace NMM;

public partial class AppShell : Shell
{
    #region Properties
    private readonly DataAccess _dataAccess;

    private readonly NMMModel.Model.Table _model;
    private readonly NMMModelView _viewModel;

    private ContentPage _gamePage;
    private SettingsPage _settingsPage;

    private readonly IStore _store;
    private readonly StoredGameBrowserModel _storedGameBrowserModel;
    private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;
    #endregion

    #region Constructor
    public AppShell(IStore Store,
        DataAccess NMMDataAccess,
        Table Model,
        NMMModelView ViewModel)
    {
        InitializeComponent();

        _store = Store;
        _dataAccess = NMMDataAccess;
        _model = Model;
        _viewModel = ViewModel;

        _model.board.End_ += new EventHandler<NMMGameOverEventArgs>(Model_GameOver);
        _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
        _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
        _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
        _viewModel.ResumeGame += new EventHandler(ViewModel_ResumeGame);
        _viewModel.NewGame += new EventHandler(ViewModel_ResumeGame);
        _viewModel.ShowMessage += new EventHandler<MessageEventArgs>(ViewModel_ShowMessage);

#if ANDROID
        _gamePage = new GamePageAnd();
#else
        _gamePage = new GamePageWin();
#endif
        _gamePage.BindingContext = _viewModel;

        _settingsPage = new SettingsPage();
        _settingsPage.BindingContext = _viewModel;

        _storedGameBrowserModel = new StoredGameBrowserModel(_store);
        _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
        _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
        _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
    }
    #endregion

    #region Eventhandlers
    private async void Model_GameOver(object sender, NMMGameOverEventArgs e)
    {
        await DisplayAlert("NMM", e.winner + " won!", "OK");
    }

    private async void ViewModel_ResumeGame(object sender, EventArgs e)
    {
        await Navigation.PushAsync(_gamePage);
    }

    private async void ViewModel_ExitGame(object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ViewModel_ShowMessage(object sender, MessageEventArgs e)
    {
        await DisplayAlert("NMM", e._str, "OK");
        
    }
    #endregion

    #region Load
    private async void ViewModel_LoadGame(object? sender, EventArgs e)
    {
        await _storedGameBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new LoadGamePage
        {
            BindingContext = _storedGameBrowserViewModel
        });
    }

    private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync();
        try
        {
            await _model.LoadGameAsync(e.Name);
            _viewModel.Refresh();
            _model.board.End_ += new EventHandler<NMMGameOverEventArgs>(Model_GameOver);
            await Navigation.PushAsync(_gamePage);
            await DisplayAlert("NMM", "Success!", "OK");
        }
        catch
        {
            await DisplayAlert("NMM", "Loading Failed!", "OK");
        }
    }
    #endregion

    #region Save
    private async void ViewModel_SaveGame(object? sender, EventArgs e)
    {
        await _storedGameBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new SaveGamePage
        {
            BindingContext = _storedGameBrowserViewModel
        });
    }

    private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync();

        try
        {
            await _model.SaveGameAsync(e.Name);
            await DisplayAlert("NMM", "Success!", "OK");
        }
        catch (NMMDataException ex)
        {
            await DisplayAlert("NMM", ex.Message, "OK");
        }
        catch (FileFormatException)
        {
            await DisplayAlert("NMM", "Cannot save, use txt files!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("NMM", ex.Message, "OK");
        }
    }
    #endregion
}