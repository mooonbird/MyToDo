using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels.Dialogs
{
    internal class AddToDoViewModel : IDialogHostAware
    {
        public string? DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public AddToDoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel));
        }

        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters parameters = new DialogParameters();
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameters));
            }

        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
