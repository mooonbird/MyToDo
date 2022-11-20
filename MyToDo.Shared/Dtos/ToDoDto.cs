using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    /// <summary>
    /// 待办事项数据实体
    /// </summary>
    public class ToDoDto : BaseDto
    {
        private string? title;
        private string? content;
        private int status;

        public string? Title
        {
            get => title; set
            {
                title = value;
                RaisePropertyChanged();
            }
        }
        public string? Content
        {
            get => content; set
            {
                content = value;
                RaisePropertyChanged();
            }
        }
        public int Status
        {
            get => status; set
            {
                status = value;
                RaisePropertyChanged();
            }
        }
    }
}
