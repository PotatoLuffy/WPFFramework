﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EIPMonitor.ViewModel.NavigationBar
{
    public class MainWindowViewModel: ViewModelBase
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            NavigationItems = new ObservableCollection<NavigationItem>(new[]
            {
                new NavigationItem(
                    "Welcome",
                    typeof(Welcome)
                )
            });
            var items = GenerateNavigationItems(snackbarMessageQueue)?.OrderBy(i => i.Name)??null;
            if (items != null)
                foreach (var item in items)
                {
                    NavigationItems.Add(item);
                }
        }

        private NavigationItem? _selectedItem;
        private int _selectedIndex;
        private bool _controlsEnabled = true;

        public ObservableCollection<NavigationItem> NavigationItems { get; }

        public NavigationItem? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set => SetProperty(ref _controlsEnabled, value);
        }

        private static IEnumerable<NavigationItem> GenerateNavigationItems(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue is null)
                throw new ArgumentNullException(nameof(snackbarMessageQueue));
            return default;
        }
    }
}