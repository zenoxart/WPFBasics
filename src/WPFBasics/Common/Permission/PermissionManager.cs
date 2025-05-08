using System.Windows;
using System.Windows.Controls;

namespace WPFBasics.Common.Permission
{
    /// <summary>
    /// Definiert die verschiedenen Berechtigungen.
    /// </summary>
    public enum PermissionType
    {
        None,
        User,
        Developer,
        Maintainer,
        Admin
    }

    /// <summary>
    /// Verwaltet Berechtigungen und bietet Methoden zur �berpr�fung von Berechtigungen.
    /// </summary>
    public static class PermissionManager
    {
        private static readonly HashSet<PermissionType> _grantedPermissions = new();

        /// <summary>
        /// F�gt eine Berechtigung hinzu.
        /// </summary>
        /// <param name="permission">Die hinzuzuf�gende Berechtigung.</param>
        public static void GrantPermission(PermissionType permission)
        {
            _grantedPermissions.Add(permission);
        }

        /// <summary>
        /// Entfernt eine Berechtigung.
        /// </summary>
        /// <param name="permission">Die zu entfernende Berechtigung.</param>
        public static void RevokePermission(PermissionType permission)
        {
            _grantedPermissions.Remove(permission);
        }

        /// <summary>
        /// �berpr�ft, ob eine Berechtigung vorhanden ist.
        /// </summary>
        /// <param name="permission">Die zu �berpr�fende Berechtigung.</param>
        /// <returns>True, wenn die Berechtigung vorhanden ist, sonst false.</returns>
        public static bool HasPermission(PermissionType permission)
        {
            if (_grantedPermissions.Contains(permission))
                return true;

            // Admin hat alle Berechtigungen
            if (permission != PermissionType.Admin && _grantedPermissions.Contains(PermissionType.Admin))
                return true;

            // Entwickler hat Maintainer- und Benutzerberechtigungen
            if ((permission == PermissionType.Maintainer || permission == PermissionType.User) &&
                _grantedPermissions.Contains(PermissionType.Developer))
                return true;

            // Maintainer hat Benutzerberechtigungen
            if (permission == PermissionType.User && _grantedPermissions.Contains(PermissionType.Maintainer))
                return true;

            return false;
        }
    }

    /// <summary>
    /// Stellt ein Verhalten bereit, um Berechtigungen an UI-Elemente zu binden.
    /// </summary>
    public static class PermissionBehavior
    {
        /// <summary>
        /// Abh�ngigkeitseigenschaft f�r die erforderliche Berechtigung.
        /// </summary>
        public static readonly DependencyProperty RequiredPermissionProperty =
            DependencyProperty.RegisterAttached(
                "RequiredPermission",
                typeof(PermissionType),
                typeof(PermissionBehavior),
                new PropertyMetadata(PermissionType.None, OnRequiredPermissionChanged));

        /// <summary>
        /// Ruft die erforderliche Berechtigung f�r ein UI-Element ab.
        /// </summary>
        /// <param name="element">Das UI-Element.</param>
        /// <returns>Die erforderliche Berechtigung.</returns>
        public static PermissionType GetRequiredPermission(UIElement element)
        {
            return (PermissionType)element.GetValue(RequiredPermissionProperty);
        }

        /// <summary>
        /// Legt die erforderliche Berechtigung f�r ein UI-Element fest.
        /// </summary>
        /// <param name="element">Das UI-Element.</param>
        /// <param name="value">Die festzulegende Berechtigung.</param>
        public static void SetRequiredPermission(UIElement element, PermissionType value)
        {
            element.SetValue(RequiredPermissionProperty, value);
        }

        /// <summary>
        /// Wird aufgerufen, wenn sich die erforderliche Berechtigung �ndert.
        /// </summary>
        /// <param name="d">Das Abh�ngigkeitsobjekt.</param>
        /// <param name="e">Die Ereignisdaten.</param>
        private static void OnRequiredPermissionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control control)
            {
                var permission = (PermissionType)e.NewValue;
                control.IsEnabled = permission == PermissionType.None || PermissionManager.HasPermission(permission);
            }
        }
    }
}