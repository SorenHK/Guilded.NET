using System;

namespace Guilded.NET.Base.Permissions
{
    /// <summary>
    /// Permissions related to XP.
    /// </summary>
    /// <remarks>
    /// <para>Defines permissions related to XP &amp; levels.</para>
    /// </remarks>
    [Flags]
    public enum XPPermissions
    {
        /// <summary>
        /// No given permissions.
        /// </summary>
        None = 0,
        /// <summary>
        /// Allows you to manage XP on server members
        /// </summary>
        ManageServerXP = 1,

        #region Additional
        /// <summary>
        /// All of the permissions combined.
        /// </summary>
        All = ManageServerXP,
        /// <summary>
        /// All of the manage permissions combined.
        /// </summary>
        Manage = ManageServerXP
        #endregion
    }
}