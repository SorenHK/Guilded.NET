using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Guilded.Commands;

/// <summary>
/// Represents the interface for all commands.
/// </summary>
/// <typeparam name="T">The type of the reflection member</typeparam>
public interface ICommandInfo<out T> where T : MemberInfo
{
    #region Abstract members
    /// <summary>
    /// Gets the <see cref="CommandAttribute.Name">name</see> of the command.
    /// </summary>
    /// <value>Name</value>
    string Name { get; }
    /// <summary>
    /// Gets the array of <see cref="CommandAttribute.Aliases">alternative names</see> of the command.
    /// </summary>
    /// <value>Array of names</value>
    string[]? Aliases { get; }
    /// <summary>
    /// Gets the member that was declared as a command.
    /// </summary>
    /// <value>Reflection member</value>
    T Member { get; }
    /// <summary>
    /// Gets the <see cref="CommandAttribute">command attribute</see> that was given to the <see cref="Member">member</see>.
    /// </summary>
    /// <value>Command attribute</value>
    CommandAttribute Attribute { get; }
    /// <summary>
    /// Invokes the command.
    /// </summary>
    /// <param name="commandEvent">The command event that invoked the command</param>
    /// <param name="arguments">The arguments that have been used to invoke the command</param>
    /// <returns>Whether the command was properly invoked</returns>
    Task<bool> InvokeAsync(CommandEvent commandEvent, IEnumerable<string> arguments);
    #endregion

    #region Additional
    /// <summary>
    /// Gets whether the <paramref name="name">given name</paramref> matches command's <see cref="Name">name</see> or its <see cref="Aliases">aliases</see>.
    /// </summary>
    /// <param name="name">The name to check whether the command contains</param>
    /// <returns>Command has <paramref name="name">given name</paramref></returns>
    public bool HasName(string name) =>
        Name == name || (Aliases?.Contains(name) ?? false);
    #endregion
}
/// <summary>
/// Represents the base for information about any type of Guilded.NET command.
/// </summary>
/// <typeparam name="T">The type of the member it uses for commands</typeparam>
/// <seealso cref="CommandAttribute" />
/// <seealso cref="CommandParamAttribute" />
/// <seealso cref="ICommandInfo{T}" />
/// <seealso cref="CommandInfo" />
/// <seealso cref="CommandContainerInfo" />
public abstract class AbstractCommandInfo<T> : ICommandInfo<T> where T : MemberInfo
{
    #region Properties
    /// <inheritdoc />
    public string Name { get; }
    /// <inheritdoc />
    public string[]? Aliases => Attribute.Aliases;
    /// <inheritdoc />
    public T Member { get; }
    /// <inheritdoc />
    public CommandAttribute Attribute { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="AbstractCommandInfo{T}" />.
    /// </summary>
    /// <param name="attribute">The command attribute that was given to the member</param>
    /// <param name="member">The member that was declared as a command</param>
    protected AbstractCommandInfo(CommandAttribute attribute, T member) =>
        (Name, Member, Attribute) = (attribute.Name ?? member.Name.ToLowerInvariant(), member, attribute);
    #endregion

    #region Instance methods
    /// <inheritdoc />
    public abstract Task<bool> InvokeAsync(CommandEvent commandEvent, IEnumerable<string> arguments);
    #endregion
}