using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels.Dialogs
{
    public class MessageViewModel : BindableBase, IDialogHostAware
    {
        private string? title;
        private string? content;

        public string? DialogHostName { get; set; }
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
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public MessageViewModel()
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
                DialogParameters parameters = new();
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameters));
            }

        }



        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(nameof(Title)))
            {
                this.Title=parameters.GetValue<string>(nameof(Title));  
            }

            if (parameters.ContainsKey(nameof(Content)))
            {
                this.Content = parameters.GetValue<string>(nameof(Content));
            }
        }
    }
}
