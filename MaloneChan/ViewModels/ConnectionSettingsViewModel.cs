using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using MaloneChan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
namespace MaloneChan.ViewModels {
    public class ConnectionSettingsViewModel : ViewModel {
        public ConnectionSettingsViewModel():base() {
            this.DialogCoordinator = MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance;
            this.Connections.Clear();
            foreach (var conn in AppConfig.GetInstance().ConnectionParameters) {
                this.Connections.Add(conn);
            }
        }
        public MahApps.Metro.Controls.Dialogs.IDialogCoordinator DialogCoordinator {
            get;
            set;
        }


        private ViewModelCommand _TestCommand;

        public ViewModelCommand TestCommand {
            get {
                if (_TestCommand == null) {
                    _TestCommand = new ViewModelCommand(Test, CanTest);
                }
                return _TestCommand;
            }
        }

        public bool CanTest() {
            if(this.SelectedConnection == null) {
                return false;
            }
            try {
                this.SelectedConnection.Validate();
                return true;
            } catch  {
                return false;
            }
        }

        public void Test() {
            try {
                this.SelectedConnection.Connect();
                DialogCoordinator.ShowMessageAsync(this, "OK", "接続成功！", MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
            } catch (Exception ex) {
                DialogCoordinator.ShowMessageAsync(this, "エラー", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
            }
        }


        private bool? _DialogResult;

        public bool? DialogResult {
            get {
                return _DialogResult;
            }
            set { 
                if (_DialogResult == value) {
                    return;
                }
                _DialogResult = value;
                RaisePropertyChanged();
            }
        }

        private ViewModelCommand _RemoveSelectedCommand;

        public ViewModelCommand RemoveSelectedCommand {
            get {
                if (_RemoveSelectedCommand == null) {
                    _RemoveSelectedCommand = new ViewModelCommand(RemoveSelected, CanRemoveSelected);
                }
                return _RemoveSelectedCommand;
            }
        }

        public bool CanRemoveSelected() {
            return this.SelectedConnection != null;
        }

        public void RemoveSelected() {
            this.Connections.Remove(this.SelectedConnection);
        }

        private ViewModelCommand _SaveSelectedCommand;

        public ViewModelCommand SaveSelectedCommand {
            get {
                if (_SaveSelectedCommand == null) {
                    _SaveSelectedCommand = new ViewModelCommand(SaveSelected, CanSaveSelected);
                }
                return _SaveSelectedCommand;
            }
        }

        public bool CanSaveSelected() {
            return this.SelectedConnection != null;
        }

        public void SaveSelected() {
            if (this.Connections.Where(x => x.OrgName.Equals(this.SelectedConnection.OrgName)).Any()) {
                AppConfig.GetInstance().ConnectionParameters.RemoveAll(x => x.OrgName.Equals(this.SelectedConnection.OrgName));
            }
            foreach(var conn in this.Connections) {
                AppConfig.GetInstance().ConnectionParameters.Add(conn);
            }
            AppConfig.GetInstance().Save();
        }

        private ViewModelCommand _OKCommand;

        public ViewModelCommand OKCommand {
            get {
                if (_OKCommand == null) {
                    _OKCommand = new ViewModelCommand(OK, CanOK);
                }
                return _OKCommand;
            }
        }

        public bool CanOK() {
            return this.SelectedConnection != null;
        }

        public void OK() {
            try {
                this.SelectedConnection.Validate();
                this.SelectedConnection.Connect();
                AppConfig.GetInstance().CurrentConnectionParameter = this.SelectedConnection;
                SaveSelectedCommand.Execute();
                this.DialogResult = true;
            } catch (Exception ex) {
                DialogCoordinator.ShowMessageAsync(this, "エラー", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
            }
        }

        private ViewModelCommand _CancelCommand;

        public ViewModelCommand CancelCommand {
            get {
                if (_CancelCommand == null) {
                    _CancelCommand = new ViewModelCommand(Cancel);
                }
                return _CancelCommand;
            }
        }

        public void Cancel() {
            this.DialogResult = false;
        }

        private ViewModelCommand _AddCommand;

        public ViewModelCommand AddCommand {
            get {
                if (_AddCommand == null) {
                    _AddCommand = new ViewModelCommand(Add);
                }
                return _AddCommand;
            }
        }

        public void Add() {
            var newConnection = new DbConnectionParameter();
            this.Connections.Add(newConnection);
            this.SelectedConnection = newConnection;
        }

        public DispatcherCollection<IDbConnectionParameter> Connections {
            get;
        } = new DispatcherCollection<IDbConnectionParameter>(DispatcherHelper.UIDispatcher);

        private IDbConnectionParameter _SelectedConnection;

        public IDbConnectionParameter SelectedConnection {
            get {
                return _SelectedConnection;
            }
            set { 
                if (_SelectedConnection == value) {
                    return;
                }
                _SelectedConnection = value;
                OKCommand.RaiseCanExecuteChanged();
                TestCommand.RaiseCanExecuteChanged();
                RemoveSelectedCommand.RaiseCanExecuteChanged();
                SaveSelectedCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }
   }
}
