using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace Ara3D.Speckle.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            dispatcherUnhandledExceptionEventArgs.Handled = true;
            MessageBox.Show(dispatcherUnhandledExceptionEventArgs.Exception.Message);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            dispatcherUnhandledExceptionEventArgs.Handled = true;
            MessageBox.Show(dispatcherUnhandledExceptionEventArgs.Exception.Message);
        }

        private void App_OnActivated(object? sender, EventArgs e)
        {
            DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }
    }

}
