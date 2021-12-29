using EIPMonitor.IDomainService;
using EIPMonitor.ViewModel;
using EIPMonitor.ViewModel.SecurityModels;
using EIPMonitor.Views.UserWindowViews;
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
    public class LoginViewDialog: BaseViewDialog<Login>,IModelDialog
    {

        public override void BindDefaultViewModel()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            GetWindowDialog().DataContext = loginViewModel;
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
            Messenger.Default.Register<string>(GetWindowDialog(), "ApplicationHiding", new Action<String>((msg) => { GetWindowDialog().Hide(); }));
            Messenger.Default.Register<string>(GetWindowDialog(), "ApplicationShutdown", new Action<String>((arg) => { Application.Current.Shutdown(); }));
            Messenger.Default.Register<string>(GetWindowDialog(), "SendMessageToLoginWin", new Action<String>((msg) =>
            { 
                (GetWindowDialog() as Login).MainSnackbar.MessageQueue?.Enqueue(msg); 
            }));
        }
        public Window GetWindowDialog()
        {
            return GetDialog() as Window;
        }
    }
}
