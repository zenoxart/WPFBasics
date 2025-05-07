using WPFBasics.Common.Permission;

namespace WPFBasics.Common.Annotations
{

    /// <summary>
    /// Erzwingt, dass bestimmte Berechtigungen vor der Ausf�hrung einer Methode �berpr�ft werden.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class RequiresPermissionAttribute : Attribute
    {
        /// <summary>
        /// Die erforderliche Berechtigung.
        /// </summary>
        public PermissionType Permission { get; }

        /// <summary>
        /// Initialisiert eine neue Instanz des Attributs mit der angegebenen Berechtigung.
        /// </summary>
        /// <param name="permission">Die erforderliche Berechtigung.</param>
        public RequiresPermissionAttribute(PermissionType permission)
        {
            Permission = permission;
        }

        /// <summary>
        /// �berpr�ft, ob die Berechtigung vorhanden ist.
        /// </summary>
        public void Validate()
        {
            if (!PermissionManager.HasPermission(Permission))
            {
                throw new UnauthorizedAccessException($"Die Berechtigung '{Permission}' ist erforderlich, um diese Methode auszuf�hren.");
            }
        }
    }
}
