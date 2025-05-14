using System.Windows;
using Zenox.Wpf.Core.Common;

namespace Zenox.Wpf.Core.Common.Services
{
    /// <summary>
    /// Definiert einen Dienst zum Anzeigen von Dialogen und Nachrichten.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Zeigt einen Dialog asynchron an, der an das angegebene ViewModel gebunden ist.
        /// </summary>
        /// <typeparam name="TViewModel">Der Typ des ViewModels.</typeparam>
        /// <param name="viewModel">Das ViewModel, das an den Dialog gebunden wird.</param>
        /// <returns>Ein Task, der das Dialogergebnis (true, false oder null) zurückgibt.</returns>
        Task<bool?> ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : class;

        /// <summary>
        /// Zeigt eine Informationsnachricht asynchron an.
        /// </summary>
        /// <param name="message">Die anzuzeigende Nachricht.</param>
        /// <param name="title">Der Titel des Nachrichtenfensters. Standardwert ist "Info".</param>
        /// <returns>Ein abgeschlossener Task.</returns>
        Task ShowMessageAsync(string message, string title = "Info");
    }

    /// <summary>
    /// Implementierung des IDialogService für WPF-Anwendungen.
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <inheritdoc/>
        public async Task<bool?> ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            try
            {
                string viewTypeName = typeof(TViewModel).FullName.Replace("ViewModel", "View");
                Type viewType = Type.GetType(viewTypeName);
                if (viewType == null)
                    throw new InvalidOperationException($"View für {typeof(TViewModel).Name} nicht gefunden.");

                Window window = (Window)Activator.CreateInstance(viewType);
                window.DataContext = viewModel;
                return await Task.Run(() => window.ShowDialog());
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "DialogService.ShowDialogAsync");
                return null;
            }
        }

        /// <inheritdoc/>
        public Task ShowMessageAsync(string message, string title = "Info")
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "DialogService.ShowMessageAsync");
            }
            return Task.CompletedTask;
        }
    }
}
