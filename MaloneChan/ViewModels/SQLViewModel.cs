using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Livet;
using Livet.Commands;
using System.Data;
using MaloneChan.Database;
namespace MaloneChan.ViewModels {
    public class SQLViewModel : MenuItemViewModelBase {
        public SQLViewModel(MainWindowViewModel parent) : base(parent) {
            this.SQL.TextChanged += (sender, e) => {
                RaisePropertyChanged(nameof(SQL));
            };
        }
        public TextDocument SQL {
            get;
        } = new TextDocument();

        private DataSet _Result;

        public DataSet Result {
            get {
                return _Result;
            }
            set { 
                if (_Result == value) {
                    return;
                }
                _Result = value;
                RaisePropertyChanged();
            }
        }
        public DispatcherCollection<string> Logs {
            get;
        } = new DispatcherCollection<string>(DispatcherHelper.UIDispatcher);
        private Livet.Commands.ViewModelCommand _ExecuteCommand;

        public Livet.Commands.ViewModelCommand ExecuteCommand {
            get {
                if (_ExecuteCommand == null) {
                    _ExecuteCommand = new Livet.Commands.ViewModelCommand(Execute, CanExecute);
                }
                return _ExecuteCommand;
            }
        }

        public bool CanExecute() {
            return !string.IsNullOrEmpty(this.SQL.Text);
        }

        public void Execute() {

        }

        public DispatcherCollection<ISQLPlanItem> SQLPlans {
            get;
        } = new DispatcherCollection<ISQLPlanItem>(DispatcherHelper.UIDispatcher);

    }
}
