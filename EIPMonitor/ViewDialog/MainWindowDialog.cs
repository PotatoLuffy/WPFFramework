using EIPMonitor.IDomainService;
using EIPMonitor.ViewModel;
using EIPMonitor.ViewModel.NavigationBar;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EIPMonitor.ViewDialog
{
    public class MainWindowDialog: BaseViewDialog<MainWindow>, IModelDialog
    {
        private MainWindowViewModel mainWindowViewModel;
        public override void BindDefaultViewModel()
        {
            mainWindowViewModel = new MainWindowViewModel();
            GetWindowDialog().DataContext = mainWindowViewModel;

        }
        public override void BindViewModel<T>(T viewModel)
        {
            GetWindowDialog().DataContext = viewModel;
        }
        public override void Close()
        {
            GetWindowDialog().Close();
        }
        public override Task<bool> ShowDialog(DialogOpenedEventHandler openedEventHandler = null, DialogClosingEventHandler closingEventHandler = null)
        {
            
            GetWindowDialog().ShowDialog();
            return Task.FromResult(true);
        }
        public override void RegisterDefaultEvent()
        {
            GetWindowDialog().MouseDown += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed) GetWindowDialog().DragMove();
            };
            Messenger.Default.Register<string>(GetWindowDialog(), "InitializeTheMainWindowLayout", new Action<String>((msg) =>
            {
                mainWindowViewModel.InitializeBasicInformation();
            }));
            Messenger.Default.Register<string>(GetWindowDialog(), "MainWindowShutdown", new Action<String>((arg) => { Application.Current.Shutdown(); Environment.Exit(0); }));
            Messenger.Default.Register<string>(GetWindowDialog(), "SendMessageToMainWin", new Action<String>((msg) =>
            {
                try
                {
                    MainWindow.Snackbar.MessageQueue?.Enqueue(msg);
                }
                catch (Exception e)
                {
                    //swallow the exception
                }
            }));
            
        }
        public Window GetWindowDialog()
        {
            return GetDialog() as Window;
        }
    }
}
