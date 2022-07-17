using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using MaloneChan.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MaloneChan.ViewModels {
    public class MainWindowViewModel : ViewModel {
        public MainWindowViewModel() : base() {
            this.TabPages = new ObservableCollection<MenuItemViewModelBase>();
            this.DialogCoordinator = MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance;

            var fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.AppTitle = $"{fv.ProductName} Ver {fv.ProductVersion}";
            AddDatabaseObjectViewCommand.Execute();
        }
        public MahApps.Metro.Controls.Dialogs.IDialogCoordinator DialogCoordinator {
            get;
            set;
        }
        public string AppTitle {
            get;
        }

        public void Initialize() {
            // AddDatabaseObjectViewCommand.Execute();
        }

        private MenuItemViewModelBase _SelectedTabPage;

        public MenuItemViewModelBase SelectedTabPage {
            get {
                return _SelectedTabPage;
            }
            set { 
                if (_SelectedTabPage == value) {
                    return;
                }
                _SelectedTabPage = value;
                RemoveSelectedViewCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MenuItemViewModelBase> TabPages {
            get;
        }

        private ViewModelCommand _AddSQLViewCommand;

        public ViewModelCommand AddSQLViewCommand {
            get {
                if (_AddSQLViewCommand == null) {
                    _AddSQLViewCommand = new ViewModelCommand(AddSQLView);
                }
                return _AddSQLViewCommand;
            }
        }

        public void AddSQLView() {
            var sql = new SQLViewModel(this) {
                Icon = new PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.Home },
                Label = "SQL",
                IsVisible = true,
                ToolTip = "SQL",
            };
            sql.Message += Menu_Message;
            sql.ErrorOccurred += Menu_ErrorOccurred;
            this.TabPages.Add(sql);
            this.SelectedTabPage = sql;
        }

        private ViewModelCommand _AddDatabaseObjectViewCommand;

        public ViewModelCommand AddDatabaseObjectViewCommand {
            get {
                if (_AddDatabaseObjectViewCommand == null) {
                    _AddDatabaseObjectViewCommand = new ViewModelCommand(AddDatabaseObjectView);
                }
                return _AddDatabaseObjectViewCommand;
            }
        }

        public void AddDatabaseObjectView() {
            var db = new DatabaseObjectViewModel(this) {
                Icon = new PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.Home },
                Label = "Database",
                IsVisible = true,
                ToolTip = "Database",
            };
            db.Message += Menu_Message;
            db.ErrorOccurred += Menu_ErrorOccurred;
            this.TabPages.Add(db);
            this.SelectedTabPage = db;
        }


        private ViewModelCommand _AddEFCoreTestingViewCommand;

        public ViewModelCommand AddEFCoreTestingViewCommand {
            get {
                if (_AddEFCoreTestingViewCommand == null) {
                    _AddEFCoreTestingViewCommand = new ViewModelCommand(AddEFCoreTestingView);
                }
                return _AddEFCoreTestingViewCommand;
            }
        }

        public void AddEFCoreTestingView() {
            var efcore = new EFCoreTestingViewModel(this) {
                Icon = new PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.Home },
                Label = "EFCoreTest",
                IsVisible = true,
                ToolTip = "EFCoreTest",
            };
            efcore.Message += Menu_Message;
            efcore.ErrorOccurred += Menu_ErrorOccurred;
            this.TabPages.Add(efcore);
            this.SelectedTabPage = efcore;
        }

        private ViewModelCommand _RemoveSelectedViewCommand;

        public ViewModelCommand RemoveSelectedViewCommand {
            get {
                if (_RemoveSelectedViewCommand == null) {
                    _RemoveSelectedViewCommand = new ViewModelCommand(RemoveSelectedView, CanRemoveSelectedView);
                }
                return _RemoveSelectedViewCommand;
            }
        }

        public bool CanRemoveSelectedView() {
            return this.SelectedTabPage != null;
        }

        public void RemoveSelectedView() {
            this.TabPages.Remove(this.SelectedTabPage);
        }



        private void Menu_Message(object sender, MessageEventArgs e) {
            DialogCoordinator.ShowMessageAsync(this, e.Title, e.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }

        private void Menu_ErrorOccurred(object sender, ErrorOccurredEventArgs e) {
            DialogCoordinator.ShowMessageAsync(this, "エラー", e.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }
    }
}