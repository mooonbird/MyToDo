using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Services;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Paremeters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        #region 属性和命令后台绑定支持数据
        /// <summary>
        /// 右侧添加备忘录窗体是否展开
        /// </summary>
        private bool _isRightDrawerOpen;
        private ToDoDto _toDoDtoCurrent = new ToDoDto();
        private string? _searchCondition;
        private readonly IToDoService _toDoService;
        private readonly IDialogHostService _dialogHostService;

        public bool IsRightDrawerOpen
        {
            get { return _isRightDrawerOpen; }
            set { _isRightDrawerOpen = value; RaisePropertyChanged(); }
        }
        public ToDoDto ToDoDtoCurrent
        {
            get => _toDoDtoCurrent; set
            {
                _toDoDtoCurrent = value;
                RaisePropertyChanged();
            }
        }
        public string? SearchCondition
        {
            get => _searchCondition; set
            {
                _searchCondition = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ToDoDto> ToDoDtos { get; set; }

        public DelegateCommand<string> ExecuteCommand { get; set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; set; }
        public DelegateCommand<ToDoDto> DeleteCommand { get; set; }
        #endregion

        public ToDoViewModel(IToDoService toDoService, IContainerProvider containerProvider)
            : base(containerProvider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
            this._toDoService = toDoService;
            _dialogHostService = containerProvider.Resolve<IDialogHostService>();
        }

        private void Selected(ToDoDto obj)
        {
            IsRightDrawerOpen = true;
            UpdateDtoCurrent(obj);
        }

        private void Execute(string para)
        {
            switch (para)
            {
                case "添加待办": AddToDo(); break;
                case "查找待办事项": SearchToDos(); break;
                case "添加到待办": AddIntoToDos(); break;
                default:
                    break;
            }
        }

        private async void Delete(ToDoDto toDoDto)
        {
            var result = await _dialogHostService.Query("提示", "确定要删除吗？");
            if (result != null && result.Result == ButtonResult.Cancel)
                return;

            UpdateLoading(true);
            var apiResponse = await _toDoService.DeleteAsync(toDoDto.Id);
            if (apiResponse != null && apiResponse.Status)
            {
                ToDoDto? toDoDtoRemove = ToDoDtos.FirstOrDefault(e => e.Id.Equals(toDoDto.Id));
                if (toDoDtoRemove != null)
                {
                    ToDoDtos.Remove(toDoDtoRemove);
                }
            }
            UpdateLoading(false);
        }

        #region Execute的具体执行方法
        private void SearchToDos()
        {
            GetToDosAsync();
        }

        private void AddToDo()
        {
            IsRightDrawerOpen = true;
            UpdateDtoCurrent();
        }

        private async void AddIntoToDos()
        {
            if (string.IsNullOrWhiteSpace(ToDoDtoCurrent!.Title) 
                || string.IsNullOrWhiteSpace(ToDoDtoCurrent.Content))
            {
                return;
            }
            UpdateLoading(true);

            try
            {
                if (ToDoDtoCurrent.Id > 0)
                {
                    var response = await _toDoService.UpdateAsync(ToDoDtoCurrent);
                    if (response != null && response.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(e => e.Id == ToDoDtoCurrent.Id);
                        if (todo != null)
                        {
                            todo.Title = response.Result!.Title;
                            todo.Content = response.Result.Content;
                            todo.Status = response.Result.Status;
                        }
                    }

                    SendMessage("数据已更新！");
                }
                else
                {
                    var response = await _toDoService.AddAsync(ToDoDtoCurrent);
                    if (response != null && response.Status)
                    {
                        ToDoDtos.Add(response.Result!);
                    }

                    SendMessage("数据已添加！");
                }
                IsRightDrawerOpen = false;
            }
            catch (Exception)
            {
                //日志
            }
            finally
            {
                UpdateLoading(false);
            }

            
        }
        #endregion

        private void UpdateDtoCurrent(ToDoDto? toDoDto = null)
        {
            if (toDoDto != null)
            {
                ToDoDtoCurrent.Id = toDoDto.Id;
                ToDoDtoCurrent.Title = toDoDto.Title;
                ToDoDtoCurrent.Content = toDoDto.Content;
                ToDoDtoCurrent.Status = toDoDto.Status;
            }
            else
            {
                ToDoDtoCurrent.Id = 0;
                ToDoDtoCurrent.Title = string.Empty;
                ToDoDtoCurrent.Content = string.Empty;
                ToDoDtoCurrent.Status = 0;
            }
        }

        private async void GetToDosAsync()
        {
            UpdateLoading(true);

            var response = await _toDoService.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = SearchCondition,
            });

            if (response != null && response.Status)
            {
                ToDoDtos.Clear();
                foreach (var item in response.Result!.Items)
                {
                    ToDoDtos.Add(item);
                }
            }

            UpdateLoading(false);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            GetToDosAsync();
        }
    }
}
