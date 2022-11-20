using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class UserDto : BaseDto
    {
		private string? _userName;
        private string? _account;
        private string? _password;

        public string? UserName
		{
			get { return _userName; }
			set { _userName = value; RaisePropertyChanged(); }
		}
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

	}
}
