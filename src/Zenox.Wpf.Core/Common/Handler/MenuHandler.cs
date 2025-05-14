using System.ComponentModel.Design;
using Zenox.Wpf.Core.Common.Command;

namespace Zenox.Wpf.Core.Common.Handler
{
    /// <summary>
    /// Stellt einen Menü-Handler für das Verwalten & Laden 
    /// von dynamischen generierten Menü-Punkten
    /// </summary>
    public class RelayMenu : RelayCommandPool, IMenuCommandService
    {
        public DesignerVerbCollection Verbs { get; } = new DesignerVerbCollection();

        public void AddCommand(MenuCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            Add(command.CommandID.Guid.ToString(),
                _ => command.Invoke(),
                _ => command.Enabled);
        }

        public void AddVerb(DesignerVerb verb)
        {
            if (verb == null)
                throw new ArgumentNullException(nameof(verb));

            Verbs.Add(verb);
        }

        public MenuCommand? FindCommand(CommandID commandID)
        {
            if (commandID == null)
            {
                throw new ArgumentNullException(nameof(commandID));
            }

            if (TryGetCommand(commandID.Guid.ToString(), out var relayCommand))
            {
                return new MenuCommand((sender, e) => relayCommand.Execute(null), commandID)
                {
                    Enabled = relayCommand.CanExecute(null)
                };
            }

            return null;
        }

        public bool GlobalInvoke(CommandID commandID)
        {
            if (commandID == null)
                throw new ArgumentNullException(nameof(commandID));

            if (TryGetCommand(commandID.Guid.ToString(), out var relayCommand) && relayCommand.CanExecute(null))
            {
                relayCommand.Execute(null);
                return true;
            }

            return false;
        }

        public bool GlobalInvoke(CommandID commandID, object parameter)
        {
            if (commandID == null)
                throw new ArgumentNullException(nameof(commandID));

            if (TryGetCommand(commandID.Guid.ToString(), out var relayCommand) && relayCommand.CanExecute(parameter))
            {
                relayCommand.Execute(parameter);
                return true;
            }

            return false;
        }

        public void RemoveCommand(MenuCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            Remove(command.CommandID.Guid.ToString());
        }

        public void RemoveVerb(DesignerVerb verb)
        {
            if (verb == null)
                throw new ArgumentNullException(nameof(verb));

            Verbs.Remove(verb);
        }

        public void ShowContextMenu(CommandID menuID, int x, int y)
        {
            if (menuID == null)
                throw new ArgumentNullException(nameof(menuID));

            // Implementation for showing a context menu can be added here.
            // This is typically dependent on the UI framework being used.
            throw new NotImplementedException("Context menu display logic is not implemented.");
        }
    }
}