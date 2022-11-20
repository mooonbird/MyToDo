using MyToDo.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class NavigationViewModel : BindableBase, INavigationAware
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            this._containerProvider = containerProvider;
            _eventAggregator = _containerProvider.Resolve<IEventAggregator>();
        }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public void UpdateLoading(bool isOpen)
        {
            _eventAggregator.UpdateLoading(new Common.Events.UpdateModel()
            {
                IsOpen = isOpen
            });
        }

        public void SendMessage(string message)
        {
            _eventAggregator.SendMessage(message);
        }
    }
}
