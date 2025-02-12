using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Guilded.Commands.Items;

/// <summary>
/// Represents the interface for all commands.
/// </summary>
/// <typeparam name="TMember">The type of the reflection member</typeparam>
public interface ICommand<out TMember> where TMember : MemberInfo
{
    #region Abstract members
    /// <summary>
    /// Gets the <see cref="CommandAttribute.Name">name</see> of the command.
    /// </summary>
    /// <value>The <see cref="CommandAttribute.Name">name</see> of the command</value>
    string Name { get; }

    /// <summary>
    /// Gets the member who was declared as a <see cref="CommandAttribute">command</see>.
    /// </summary>
    /// <value>The member who was declared as a <see cref="CommandAttribute">command</see></value>
    TMember Member { get; }

    /// <summary>
    /// Gets the <see cref="CommandAttribute">command attribute</see> that was given to the <see cref="Member">member</see>.
    /// </summary>
    /// <value>The <see cref="CommandAttribute">command attribute</see> that was given to the <see cref="Member">member</see></value>
    CommandAttribute Attribute { get; }

    /// <inheritdoc cref="CommandAttribute.Aliases" />
    string[]? Aliases { get; }

    /// <inheritdoc cref="DescriptionAttribute.Text" />
    string? Description { get; }

    /// <inheritdoc cref="ExampleAttribute.Content" />
    IEnumerable<ExampleAttribute>? Examples { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Gets whether the given <paramref name="name" /> matches command's <see cref="Name">name</see> or its <see cref="Aliases">aliases</see>.
    /// </summary>
    /// <param name="name">The name to check whether the command contains</param>
    /// <returns>Command has given <paramref name="name" /></returns>
    public bool HasName(string name);
    #endregion
}

/// <summary>
/// Represents the base for information about any type of Guilded.NET command.
/// </summary>
/// <typeparam name="TMember">The type of the member it uses for commands</typeparam>
/// <seealso cref="CommandAttribute" />
/// <seealso cref="CommandParamAttribute" />
/// <seealso cref="ICommand{TMember}" />
/// <seealso cref="Command" />
/// <seealso cref="CommandContainer" />
public abstract class AbstractCommand<TMember> : ICommand<TMember> where TMember : MemberInfo
{
    private readonly string _name;

    #region Properties
    /// <inheritdoc />
    public virtual string Name => _name;

    /// <inheritdoc />
    public TMember Member { get; }

    /// <inheritdoc />
    public virtual CommandAttribute Attribute { get; }

    /// <inheritdoc cref="CommandAttribute.Aliases" />
    public virtual string[]? Aliases => Attribute.Aliases;

    /// <inheritdoc cref="DescriptionAttribute.Text" />
    public virtual string? Description => Member.GetCustomAttribute<DescriptionAttribute>()?.Text;

    /// <inheritdoc cref="ExampleAttribute.Content" />
    public virtual IEnumerable<ExampleAttribute> Examples => Member.GetCustomAttributes<ExampleAttribute>();
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="AbstractCommand{TMember}" />.
    /// </summary>
    /// <param name="attribute">The command attribute that was given to the member</param>
    /// <param name="member">The member who was declared as a command</param>
    protected AbstractCommand(CommandAttribute attribute, TMember member) =>
        (_name, Member, Attribute) = (attribute.Name ?? TransformMethodName(member.Name), member, attribute);
    #endregion

    #region Methods

    public bool HasName(string name) =>
        Name == name || (Aliases?.Contains(name) ?? false);

    internal static string TransformMethodName(string name)
    {
        // Trim XCommandAsync(), XCommand(), XAsync()
        string unsuffixedName = TrimSuffix(TrimSuffix(name, "Async"), "Command");

        return unsuffixedName.ToLowerInvariant();
    }

    private static string TrimSuffix(string str, string substring)
    {
        int suffixIndex = str.LastIndexOf(substring);

        return suffixIndex > -1 ? str.Substring(0, suffixIndex) : str;
    }
    #endregion
}
