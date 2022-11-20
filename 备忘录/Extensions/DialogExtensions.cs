using MyToDo.Common;
using MyToDo.Common.Events;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Extensions
{
    public static class DialogExtensions
    {
        public static async Task<IDialogResult?> Query(this IDialogHostService service, string title, string content)
        {
            DialogParameters para = new DialogParameters();
            para.Add("Title", title);
            para.Add("Content", content);
            return await service.ShowDialog("MessageView", para);
            
        }

        /// <summary>
        /// 发布和订阅“是与否”事件
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="model"></param>
        public static void UpdateLoading(this IEventAggregator aggregator, UpdateModel model)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);
        }

        public static void Register(this IEventAggregator aggregator, Action<UpdateModel> action)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(action);
        }

        /// <summary>
        /// 发布和订阅“字符串”消息事件
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendMessage(this IEventAggregator aggregator, string message, string filter = "Main")
        {
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel() { Message = message, Filter = filter });
        }
        public static void RegisterMessage(this IEventAggregator aggregator, Action<MessageModel> action, string filter = "Main")
        {
            aggregator.GetEvent<MessageEvent>().Subscribe(action, ThreadOption.PublisherThread, true, filter: e =>
            {
                return e.Filter == filter;
            });
        }
    }
}