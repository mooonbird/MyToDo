using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Services;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo.ViewModels.Dialogs
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        #region 属性和命令
        private string? _account;
        private string? _password;
        private int _selectedIndex;
        private UserDto _userRegister = new();
        private string? _passwordConfirm;


        private readonly LoginService _loginService;
        private readonly IEventAggregator _eventAggregator;

        public string Title { get; set; } = "ToDo";

        public string? Account
        {
            get { return _account; }
            set { _account = value; RaisePropertyChanged(); }
        }
        public string? Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(); }
        }
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; RaisePropertyChanged(); }
        }
        public UserDto UserRegister
        {
            get { return _userRegister; }
            set
            {
                _userRegister = value;
                RaisePropertyChanged();
            }
        }
        public string? PasswordConfirm
        {
            get { return _passwordConfirm; }
            set
            {
                _passwordConfirm = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<string> ExecuteCommand { get; set; }

        #endregion

        public LoginViewModel(LoginService loginService, IEventAggregator eventAggregator)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this._loginService = loginService;
            this._eventAggregator = eventAggregator;
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "登录": Login(); break;
                case "退出": Exit(); break;
                case "跳转注册": SelectedIndex = 1; break;
                case "确认注册账号": Register(); break;
                case "返回登录": SelectedIndex = 0; break;
                default:
                    break;
            }
        }

        private async void Register()
        {
            if (string.IsNullOrWhiteSpace(UserRegister.UserName) ||
                string.IsNullOrWhiteSpace(UserRegister.Account) ||
                string.IsNullOrWhiteSpace(UserRegister.Password))
            {
                _eventAggregator.SendMessage("用户名、账号和密码不能为空！", "Login");
                return;
            }

            if (UserRegister.Password != PasswordConfirm)
            {
                _eventAggregator.SendMessage("确认密码应与密码保持一致！", "Login");
                return;
            }

            var response = await _loginService.RegiterAsync(new UserDto()
            {
                UserName = UserRegister.UserName,
                Account = UserRegister.Account,
                Password = UserRegister.Password,
            });

            if (response != null && response.Status)
            {
                _eventAggregator.SendMessage("注册成功！", "Login");
                SelectedIndex = 0;
                return;
            }

            //注册失败提示...
        }

        private void Exit()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(Account) ||
                string.IsNullOrEmpty(Password))
                return;

            var response = await _loginService.LoginAsync(new UserDto() { Account = Account, Password = Password });
            if (response != null && response.Status)
            {
                AppSession.UserName = response.Result!.UserName;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                return;
            }

            _eventAggregator.SendMessage("用户名或密码错误，登录失败！", "Login");
        }

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            Exit();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
