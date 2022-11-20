using MyToDo.Common.Models;
using MyToDo.Services;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Paremeters;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class MemoViewModel : BindableBase
    {
        /// <summary>
        /// 右侧添加代办窗体是否展开
        /// </summary>
        private bool _IsRightDrawerOpen;
        private readonly IMemoService _memoService;

        public bool IsRightDrawerOpen
        {
            get { return _IsRightDrawerOpen; }
            set { _IsRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public DelegateCommand AddCommand { get; set; }
        public ObservableCollection<MemoDto> MemoDtos { get; set; }

        public MemoViewModel(IMemoService memoService)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            AddCommand = new DelegateCommand(Add);
            this._memoService = memoService;
            GetDataAsync();
        }
        /// <summary>
        /// 添加代办执行方法
        /// </summary>
        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        async void GetDataAsync()
        {
            var response = await _memoService.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
            });


            if (response != null && response.Status)
            {
                foreach (var item in response.Result!.Items)
                {
                    MemoDtos.Add(item);
                }
            }
            
        }
    }
}