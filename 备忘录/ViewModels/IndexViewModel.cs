using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Shared.Dtos;
using MyToDo.Views.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class IndexViewModel : BindableBase
    {
        private readonly IDialogHostService _dialogHostService;
        private string? _title;

        public ObservableCollection<TaskBar> TaskBars { get; set; }
        public ObservableCollection<ToDoDto> ToDoDtos { get; set; }
        public ObservableCollection<MemoDto> MemoDtos { get; set; }
        public DelegateCommand<string> ExecuteCommand { get; set; }
        public string? Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }
        public IndexViewModel(IDialogHostService dialogHostService)
        {
            Title = $"你好，{AppSession.UserName}！今天是2022-11-06 星期天";
            TaskBars = new ObservableCollection<TaskBar>();
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);

            CreateTaskBars();
            this._dialogHostService = dialogHostService;
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(); break;
                case "新增备忘录": AddMemo(); break;
                default:
                    break;
            }
        }

        private async void AddToDo()
        {
            await _dialogHostService.ShowDialog("AddToDoView", null);
        }

        private void AddMemo()
        {
            //_dialogService.ShowDialog("AddMemoView");
        }

        void CreateTaskBars()
        {
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FF0CA0FF", Target = "" });
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "9", Color = "#FF1ECA3A", Target = "" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = "100", Color = "#FF02C6DC", Target = "" });
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "89", Color = "#FFFFA000", Target = "" });
        }
    }
}
