using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
	internal class MenuBar : BindableBase
    {
		private string? _Icon;

		public string? Icon
		{
			get { return _Icon; }
			set { _Icon = value; }
		}

		private string? _Title;

		public string? Title
		{
			get { return _Title; }
			set { _Title = value; }
		}

		private string? _NameSpace;

		public string? NameSpace
		{
			get { return _NameSpace; }
			set { _NameSpace = value; }
		}

	}
}
