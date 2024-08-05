namespace NMM;
using NMMModel.Model;
using NMMModel.Persistence;
using NMM.ViewModel;
using NMM.Persistence;

public partial class App : Application
{
    private const string SuspendedGameSavePath = "SuspendedGame";

    private readonly AppShell _appShell;
    private readonly DataAccess _dataAccess;
    private readonly Table _model;
    private readonly NMMStore _NMMStore;
    private readonly NMMModelView _viewModel;
    public App()
    {
        InitializeComponent();
        _NMMStore= new NMMStore();
        _dataAccess = new NMMDataAccess(FileSystem.AppDataDirectory);
        _model = new NMMModel.Model.Table(_dataAccess);
        _viewModel = new NMMModelView(_model);
        _appShell = new AppShell(_NMMStore, _dataAccess, _model, _viewModel)
        {
            BindingContext = _viewModel
        };
        MainPage = _appShell;
	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Created += (s, e) =>
        {
            // új játékot indítunk
            _model.board.newGame();
        };

        window.Activated += (s, e) =>
        {
            if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                return;

            Task.Run(async () =>
            {
                try
                {
                    await _model.LoadGameAsync(SuspendedGameSavePath);
                }
                catch
                {
                }
            });
        };

        window.Stopped += (s, e) =>
        {
            Task.Run(async () =>
            {
                try
                {
                    await _model.SaveGameAsync(SuspendedGameSavePath);
                }
                catch
                {
                }
            });
        };

        return window;
    }
}
