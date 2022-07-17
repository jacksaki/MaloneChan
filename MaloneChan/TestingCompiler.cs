using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Livet;
using System.Collections.ObjectModel;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Runtime;
namespace MaloneChan {
    public class TestingCompiler:NotificationObject {
        public TestingCompiler() {
        }
        public async Task<object> EvaluateAsync() {
            
            try {
                return await CSharpScript.EvaluateAsync<object>(this.Source,
                   ScriptOptions.Default.
                   WithReferences(this.ExternalAssemblies.Select(x => Assembly.LoadFrom(x)).ToArray()).
                   WithReferences(this.References.Select(x => Assembly.Load(x))).
                   WithImports(this.Imports.ToArray()));
            } catch (CompilationErrorException ex) {
                this.ErrorText = $"{ex.Message}\r\n{ex.StackTrace}";
                return null;
            } catch (Exception ex) {
                this.ErrorText = $"{ex.Message}\r\n{ex.StackTrace}";
                return null;
            } finally {
            }
        }

        private string _ErrorText;

        public string ErrorText {
            get {
                return _ErrorText;
            }
            set { 
                if (_ErrorText == value) {
                    return;
                }
                _ErrorText = value;
                RaisePropertyChanged();
            }
        }

        public List<string> Imports {
            get;
        } = new List<string>();

        public List<string> References {
            get; 
        } = new List<string>();
        public List<string> ExternalAssemblies {
            get;
        } = new List<string>();
        private string _Source;

        public string Source {
            get {
                return _Source;
            }
            set { 
                if (_Source == value) {
                    return;
                }
                _Source = value;
                RaisePropertyChanged();
            }
        }
    }
}
