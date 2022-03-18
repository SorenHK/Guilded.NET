using System;

namespace Guilded.Base.Permissions;

/// <summary>
/// Permissions related to lists.
/// </summary>
/// <remarks>
/// <para>Defines channel permissions related to lists/tasks in list channels.</para>
/// </remarks>
[Flags]
public enum ListPermissions
{
    /// <summary>
    /// No given permissions.
    /// </summary>
    None = 0,
    /// <summary>
    /// Allows you to create list items
    /// </summary>
    CreateListItem = 1,
    /// <summary>
    /// Allows you to view list items
    /// </summary>
    ViewListItems = 2,
    /// <summary>
    /// Allows you to edit list item messages by others and move list items to other channels
    /// </summary>
    ManageListItems = 4,
    /// <summary>
    /// Allows you to remove list items created by others
    /// </summary>
    RemoveListItems = 8,
    /// <summary>
    /// Allows you to complete list items created by others
    /// </summary>
    CompleteListItems = 16,
    /// <summary>
    /// Allows you to reorder list items
    /// </summary>
    ReorderListItems = 32,

    #region Additional
    /// <summary>
    /// All of the permissions combined.
    /// </summary>
    All = CreateListItem | ViewListItems | ManageListItems | RemoveListItems | CompleteListItems | ReorderListItems,
    /// <summary>
    /// All of the manage permissions combined.
    /// </summary>
    /// <remarks>
    /// <para>Sets these permissions:</para>
    /// <list type="bullet">
    ///     <item>
    ///         <description><see cref="ManageListItems"/></description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="RemoveListItems"/></description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="ReorderListItems"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    Manage = ManageListItems | RemoveListItems | ReorderListItems,
    /// <summary>
    /// A simple permission combination allowing writing permissions and reading permissions.
    /// </summary>
    /// <remarks>
    /// <para>Sets these permissions:</para>
    /// <list type="bullet">
    ///     <item>
    ///         <description><see cref="CreateListItem"/></description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="ViewListItems"/></description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="CompleteListItems"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    Basic = CreateListItem | ViewListItems | CompleteListItems
    #endregion
}