using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo.Common
{
    public class DialogHostService :  IDialogHostService
    {
        private readonly IContainerExtension _containerExtension;

        public DialogHostService(IContainerExtension containerExtension)
        {
            this._containerExtension = containerExtension;
        }

        public async Task<IDialogResult?> ShowDialog(string name, IDialogParameters? parameters, string dialogHostName)
        {
            if (parameters == null)
            {
                parameters = new DialogParameters();
            }
            var content = _containerExtension.Resolve<object>(name);

            if (content is not FrameworkElement dialogContent)
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (dialogContent.DataContext is not IDialogHostAware viewmodel)
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            viewmodel.DialogHostName = dialogHostName;

            DialogOpenedEventHandler eventHandler = (s, arg) =>
            {
                if (viewmodel is IDialogHostAware aware)
                {
                    aware.OnDialogOpened(parameters);
                }
                arg.Session.UpdateContent(content);
            };

            return await DialogHost.Show(dialogContent, viewmodel.DialogHostName, eventHandler) as IDialogResult;

        }
    }
}
