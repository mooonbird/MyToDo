using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyToDo.Extensions
{
    public class PasswordBindable
    {
        public static string GetPasswordBindable(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordBindableProperty);
        }

        public static void SetPasswordBindable(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordBindableProperty, value);
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordBindableProperty =
            DependencyProperty.RegisterAttached(nameof(PasswordBindable), typeof(string), typeof(PasswordBindable), new PropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            string? password = e.NewValue as string;

            if (passwordBox != null && passwordBox.Password != password)
            {
                passwordBox.Password = password;
            }
        }
    }

    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox? senderPasswordBox = sender as PasswordBox;

            if (senderPasswordBox == null) 
                return;

            string? password = PasswordBindable.GetPasswordBindable(senderPasswordBox);

            if (senderPasswordBox.Password != password)
            {
                PasswordBindable.SetPasswordBindable(senderPasswordBox, senderPasswordBox.Password);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }
    }
}
