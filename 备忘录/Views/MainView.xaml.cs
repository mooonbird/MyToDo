using MaterialDesignThemes.Wpf;
using MyToDo.Extensions;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToDo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IEventAggregator aggregator)
        {
            InitializeComponent();

            aggregator.RegisterMessage(e =>
            {
                snackbar.MessageQueue!.Enqueue(e);
            });
            //
            aggregator.Register(e =>
            {
                dialogHost.IsOpen = e.IsOpen;

                if (dialogHost.IsOpen)
                {
                    dialogHost.DialogContent = new ProgressView();
                }
            });

            btMax.Click += (sender, args) =>
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
            };
            btClose.Click += (sender, args) => { this.Close(); };
            colorZone.MouseMove += (sender, args) =>
            {
                if (args.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            //colorZone.MouseDoubleClick += (sender, args) =>
            //{
            //    if (this.WindowState == WindowState.Normal)
            //    {
            //        this.WindowState = WindowState.Maximized;
            //    }
            //    else
            //    {
            //        this.WindowState = WindowState.Normal;
            //    }
            //};

            listBoxMenuBar.SelectionChanged += (sender, args) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
