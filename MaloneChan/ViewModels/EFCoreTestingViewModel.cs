using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Livet;
using Livet.Commands;
using System.Collections.ObjectModel;
namespace MaloneChan.ViewModels {
    public class EFCoreTestingViewModel : MenuItemViewModelBase {
        public EFCoreTestingViewModel(MainWindowViewModel parent) : base(parent) {
            this.Source.TextChanged += (sender, e) => {
                ExecuteCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(Source));
            };
        }


        public EFCoreTestingParameter Parameter {
            get {
                return AppConfig.GetInstance().EFCoreTestingParameter;
            }
        }


        private string _ExternalAssemblies = "";

        public string ExternalAssemblies {
            get {
                return _ExternalAssemblies;
            }
            set { 
                if (_ExternalAssemblies == value) {
                    return;
                }
                _ExternalAssemblies = value;
                RaisePropertyChanged();
            }
        }

        private string _References = "";

        public string References {
            get {
                return _References;
            }
            set { 
                if (_References == value) {
                    return;
                }
                _References = value;
                RaisePropertyChanged();
            }
        }


        private string _Imports = "System";

        public string Imports {
            get {
                return _Imports;
            }
            set { 
                if (_Imports == value) {
                    return;
                }
                _Imports = value;
                RaisePropertyChanged();
            }
        }

        public TextDocument Source {
            get;
        } = new TextDocument();

        private Livet.Commands.ViewModelCommand _ExecuteCommand;

        public Livet.Commands.ViewModelCommand ExecuteCommand {
            get {
                if (_ExecuteCommand == null) {
                    _ExecuteCommand = new Livet.Commands.ViewModelCommand(ExecuteAsync, CanExecute);
                }
                return _ExecuteCommand;
            }
        }

        public bool CanExecute() {
            return !string.IsNullOrEmpty(this.Source.Text);
        }
        
        public async void ExecuteAsync() {
            var compiler = new TestingCompiler();
            compiler.Source = this.Source.Text;
            compiler.Imports.AddRange(
                this.Imports.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x)));
            compiler.References.AddRange(
                this.References.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x)));
            compiler.ExternalAssemblies.AddRange(
                this.ExternalAssemblies.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.None).Where(x => System.IO.File.Exists(x)));
            await compiler.EvaluateAsync().ContinueWith(t=> {
                this.Result = t.Result?.ToString();
                this.Errors = compiler.ErrorText;
            },TaskScheduler.FromCurrentSynchronizationContext());
        }
        private string _Result;
        public string Result {
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


        private string _Errors;

        public string Errors {
            get {
                return _Errors;
            }
            set { 
                if (_Errors == value) {
                    return;
                }
                _Errors = value;
                RaisePropertyChanged();
            }
        }

    }
}
