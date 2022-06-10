using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Guilded.Base;

namespace Guilded.Commands;

/// <summary>
/// Represents the information about one-value command argument in <see name="CommandInfo">a command method</see>.
/// </summary>
public class CommandArgumentInfo : AbstractCommandArgument
{
    #region Static
    // FIXME: Cast int.Parse, bool.Parse, etc. to Func<string, object> somehow or something like that
    // I hate this
    internal static readonly Dictionary<Type, Func<string, object>> _converters =
        new()
        {
            { typeof(string),   x => x },
            { typeof(int),      x => int.Parse(x) },
            { typeof(bool),     x => bool.Parse(x) },
            { typeof(Guid),     x => Guid.Parse(x) },
            { typeof(HashId),   x => new HashId(x) },
            { typeof(uint),     x => uint.Parse(x) },
            { typeof(long),     x => long.Parse(x) },
            { typeof(ulong),    x => ulong.Parse(x) },
            { typeof(float),    x => float.Parse(x) },
            { typeof(short),    x => short.Parse(x) },
            { typeof(ushort),   x => ushort.Parse(x) },
            { typeof(byte),     x => byte.Parse(x) },
            { typeof(sbyte),    x => sbyte.Parse(x) },
            { typeof(double),   x => double.Parse(x) },
            { typeof(decimal),  x => decimal.Parse(x) },
            { typeof(char),     x => char.Parse(x) },
            { typeof(DateTime), x => DateTime.Parse(x) },
            { typeof(TimeSpan), x => TimeSpan.Parse(x) }
        };
    #endregion

    #region Properties
    /// <summary>
    /// Gets the converter to convert string values to the <see cref="AbstractCommandArgument.ArgumentType">argument's type</see>.
    /// </summary>
    /// <value>String to Object Converter</value>
    internal Func<string, object> Converter { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="CommandArgumentInfo" /> from a <paramref name="parameter">method parameter</paramref>.
    /// </summary>
    /// <param name="parameter">The parameter that was declared as a command parameter</param>
    public CommandArgumentInfo(ParameterInfo parameter) : base(parameter) =>
        Converter = GetParametersParser(parameter.ParameterType);
    #endregion

    #region Methods
    /// <inheritdoc />
    public override object GetValueFrom(IEnumerable<string> arguments, int index) =>
        Converter(arguments.ElementAt(index));
    #endregion

    #region Static methods
    internal static Func<string, object> GetParametersParser(Type parameterType)
    {
        if (!_converters.ContainsKey(parameterType))
            throw new FormatException($"Cannot have type {parameterType} as a command argument's type");

        return _converters[parameterType];
    }
    #endregion
}

/// <summary>
/// Represents the information about one-value command argument in <see name="CommandInfo">a command method</see>.
/// </summary>
public class CommandOptionalArgumentInfo : AbstractCommandArgument
{
    #region Properties
    /// <summary>
    /// Gets the converter to convert string values to the <see cref="AbstractCommandArgument.ArgumentType">argument's type</see>.
    /// </summary>
    /// <value>String to Object Converter</value>
    internal Func<string, object> Converter { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="CommandArgumentInfo" /> from a <paramref name="parameter">method parameter</paramref>.
    /// </summary>
    /// <param name="parameter">The parameter that was declared as a command parameter</param>
    /// <param name="type">The type of the argument</param>
    public CommandOptionalArgumentInfo(ParameterInfo parameter, Type type) : base(parameter) =>
        Converter = CommandArgumentInfo.GetParametersParser(type);
    #endregion

    #region Methods
    /// <inheritdoc />
    public override object? GetValueFrom(IEnumerable<string> arguments, int index) =>
        // Convert properly
        arguments.Count() > index
        ? Converter(arguments.ElementAt(index))
        // = xyz or `null`
        // This could be minimized to just .Value, but at this point maybe RAM would suffer and it's
        // obsession over micro-optimizations
        // need to check RAM usage of the commands first
        : Parameter.HasDefaultValue
        ? Parameter.DefaultValue
        : null;
    #endregion
}

/// <summary>
/// Represents the information about array command argument in <see name="CommandInfo">a command method</see>.
/// </summary>
public class CommandRestInfo : AbstractCommandArgument
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="CommandRestInfo" /> from a <paramref name="parameter">method parameter</paramref>.
    /// </summary>
    /// <param name="parameter">The parameter that was declared as a command parameter</param>
    public CommandRestInfo(ParameterInfo parameter) : base(parameter) { }
    #endregion

    #region Method
    /// <inheritdoc />
    public override object GetValueFrom(IEnumerable<string> arguments, int index) =>
        arguments.Skip(index).ToArray();
    #endregion
}