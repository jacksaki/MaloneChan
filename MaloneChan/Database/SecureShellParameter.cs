﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.Xml.Linq;
using Livet;
namespace MaloneChan {
    public class SecureShellParameter :NotificationObject ,ISecureshellParameter {
        public void Open() {
            if (System.IO.File.Exists(this.PrivateKeyFile)) {
                _client = new SshClient(this.HostName, this.Port.Value, this.UserName, new PrivateKeyFile[] { new PrivateKeyFile(this.PrivateKeyFile) });
            } else {
                _client = new SshClient(this.HostName, this.Port.Value, this.UserName, this.Password);
            }
            _port = new ForwardedPortLocal("127.0.0.1", this.LocalPort.Value, this.DbHostName, this.DbPort.Value);
            _port.Start();
        }
        public void Close() {
            _port?.Stop();
            _client?.Disconnect();
        }
        public XElement ToXElement() {
            return new XElement("SshParameter",
                new XAttribute("SshHostName", this.HostName),
                new XAttribute("SshPort", this.Port),
                new XAttribute("UserName", this.UserName),
                new XAttribute("Password", this.Password.Encrypt()),
                new XAttribute("LocalPort", this.LocalPort),
                new XAttribute("DbHostName", this.DbHostName),
                new XAttribute("DbPort", this.DbPort),
                new XAttribute("PrivateKeyFile", this.PrivateKeyFile)
                );
        }
        public static SecureShellParameter CreateFromXElement(XElement element) {
            return new SecureShellParameter(){
                HostName = element.Attribute("SshHostName").Value,
                Port = element.Attribute("SshPort").Value.ToInt32(22),
                UserName = element.Attribute("UserName").Value,
                LocalPort = (uint)element.Attribute("BoundPort")?.Value.ToInt32(5432),
                DbHostName = element.Attribute("DbHostName")?.Value ?? "",
                DbPort = (uint)element.Attribute("DbPort")?.Value.ToInt32(5432),
                Password = (element.Attribute("Password")?.Value ?? "").Decrypt(),
                PrivateKeyFile = element.Attribute("PrivateKeyFile")?.Value
            };
        }
        private SshClient _client;
        private ForwardedPortLocal _port;
        private bool disposedValue;


        private string _HostName = "";

        public string HostName {
            get {
                return _HostName;
            }
            set { 
                if (_HostName == value) {
                    return;
                }
                _HostName = value;
                RaisePropertyChanged();
            }
        }


        private int? _Port;

        public int? Port {
            get {
                return _Port;
            }
            set { 
                if (_Port == value) {
                    return;
                }
                _Port = value;
                RaisePropertyChanged();
            }
        }


        private string _UserName = "";

        public string UserName {
            get {
                return _UserName;
            }
            set { 
                if (_UserName == value) {
                    return;
                }
                _UserName = value;
                RaisePropertyChanged();
            }
        }


        private string _Password = "";

        public string Password {
            get {
                return _Password;
            }
            set { 
                if (_Password == value) {
                    return;
                }
                _Password = value;
                RaisePropertyChanged();
            }
        }


        private uint? _LocalPort;

        public uint? LocalPort {
            get {
                return _LocalPort;
            }
            set { 
                if (_LocalPort == value) {
                    return;
                }
                _LocalPort = value;
                RaisePropertyChanged();
            }
        }


        private string _DbHostName = "";

        public string DbHostName {
            get {
                return _DbHostName;
            }
            set { 
                if (_DbHostName == value) {
                    return;
                }
                _DbHostName = value;
                RaisePropertyChanged();
            }
        }


        private uint? _DbPort;

        public uint? DbPort {
            get {
                return _DbPort;
            }
            set { 
                if (_DbPort == value) {
                    return;
                }
                _DbPort = value;
                RaisePropertyChanged();
            }
        }


        private string _PrivateKeyFile = "";

        public string PrivateKeyFile {
            get {
                return _PrivateKeyFile;
            }
            set { 
                if (_PrivateKeyFile == value) {
                    return;
                }
                _PrivateKeyFile = value;
                RaisePropertyChanged();
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }
                this.Close();
                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~SecureShellParameter()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose() {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Validate() {
            if (string.IsNullOrEmpty(this.HostName)) {
                throw new SecureShellParameterException("SSH HostName を入力してください");
            }
            if (!this.Port.HasValue) {
                throw new SecureShellParameterException("SSH Port を入力してください");
            }
            if(this.Port.Value<1 || this.Port > 65535) {
                throw new SecureShellParameterException("SSH Port は1～65535で入力してください");
            }
            if (string.IsNullOrEmpty(this.UserName)) {
                throw new SecureShellParameterException("SSH UserName を入力してください");
            }
            if (string.IsNullOrEmpty(this.Password) && string.IsNullOrEmpty( this.PrivateKeyFile)) {
                throw new SecureShellParameterException("SSH Password,PrivateKeyFile どちらかを入力してください");
            }
            if (!string.IsNullOrEmpty(this.PrivateKeyFile) && !System.IO.File.Exists(this.PrivateKeyFile)) {
                throw new SecureShellParameterException("SSH PrivateKeyFile が存在しません");
            }
            if (!this.LocalPort.HasValue) {
                throw new SecureShellParameterException("SSH LocalPort を入力してください");
            }
            if (this.LocalPort.Value < 1 || this.LocalPort > 65535) {
                throw new SecureShellParameterException("SSH LocalPort は1～65535で入力してください");
            }
            if (string.IsNullOrEmpty(this.DbHostName)) {
                throw new SecureShellParameterException("SSH DbHostName を入力してください");
            }
            if (this.DbPort.Value < 1 || this.DbPort > 65535) {
                throw new SecureShellParameterException("SSH LocalPort は1～65535で入力してください");
            }
        }
    }
}
