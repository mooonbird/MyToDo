using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class MemoDto : BaseDto
    {
        /// <summary>
        /// 备忘录
        /// </summary>
        private string? title;
        private string? content;

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
    }
}
