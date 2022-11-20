using DryIoc;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    internal class MainViewModel : BindableBase, IConfigureService
    {
        public MainViewModel(IRegionManager regionManager, IContainerProvider container)
        {
            this._RegionManager = regionManager;
            this._container = container;
            MenuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            LogoutCommand = new DelegateCommand(Logout);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (_Journal != null && _Journal.CanGoBack)
                {
                    _Journal.GoBack();
                }
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (_Journal != null && _Journal.CanGoForward)
                {
                    _Journal.GoForward();
                }
            });
        }

        private void Logout()
        {
            App.Logout(_container);
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
            {
                return;
            }
            _RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, result =>
            {
                _Journal = result.Context.NavigationService.Journal;
            });
        }

        #region 属性和命令
        /// <summary>
        /// 路由命令
        /// </summary>
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand GoForwardCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        private ObservableCollection<MenuBar>? _MenuBars;
        private string? _userName;
        public ObservableCollection<MenuBar>? MenuBars
        {
            get { return _MenuBars; }
            set { _MenuBars = value; }
        }

        public string? UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        /// <summary>
        /// 服务
        /// </summary>
        private readonly IRegionManager _RegionManager;
        private readonly IContainerProvider _container;
        private IRegionNavigationJournal? _Journal;

        private void CreateMenuBar()
        {
            MenuBars!.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "代办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
        }

        public void Configure()
        {
            this.UserName = AppSession.UserName;
            CreateMenuBar();
            _RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
