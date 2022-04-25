using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Guilded.Base;
using Guilded.Base.Content;
using Guilded.Base.Events;

namespace Guilded.Commands;

/// <summary>
/// Represents an event that occurs once someone invokes a command.
/// </summary>
/// <seealso cref="MessageEvent" />
/// <seealso cref="Message" />
/// <seealso cref="CommandAttribute" />
public class CommandEvent
{
    #region Properties

    #region CommandEvent properties
    /// <summary>
    /// Gets the message event that invoked the command.
    /// </summary>
    /// <value>Message creation event</value>
    public MessageEvent MessageEvent { get; }
    /// <summary>
    /// Gets the prefix that has been used on the command.
    /// </summary>
    /// <value>Prefix</value>
    public string Prefix { get; }
    /// <summary>
    /// Gets the name of the command that was used.
    /// </summary>
    /// <value>Name</value>
    public string CommandName { get; }
    /// <summary>
    /// Gets the enumerable of string arguments that were given to the command.
    /// </summary>
    /// <value>Enumerable of arguments</value>
    public IEnumerable<string> Arguments { get; }
    #endregion

    #region MessageEvent properties
    /// <inheritdoc cref="MessageEvent{T}.Message" />
    public Message Message => MessageEvent.Message;
    /// <inheritdoc cref="ChannelContent{TId, TServer}.ServerId" />
    public HashId? ServerId => Message.ServerId;
    /// <inheritdoc cref="ChannelContent{TId, TServer}.ChannelId" />
    public Guid ChannelId => Message.ChannelId;
    /// <inheritdoc cref="Message.Content" />
    public string Content => Message.Content;
    /// <inheritdoc cref="ChannelContent{TId, TServer}.CreatedBy" />
    public HashId CreatedBy => Message.CreatedBy;
    /// <inheritdoc cref="Message.CreatedByWebhook" />
    public Guid? CreatedByWebhook => Message.CreatedByWebhook;
    /// <inheritdoc cref="ChannelContent{TId, TServer}.CreatedAt" />
    public DateTime CreatedAt => Message.CreatedAt;
    /// <inheritdoc cref="Message.IsReply" />
    public bool IsReply => Message.IsReply;
    /// <inheritdoc cref="Message.ReplyMessageIds" />
    public IList<Guid>? ReplyMessageIds => Message.ReplyMessageIds;
    /// <inheritdoc cref="Message.IsPrivate" />
    public bool IsPrivate => Message.IsPrivate;
    #endregion

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="CommandEvent" /> from <see cref="MessageEvent">a created message</see>.
    /// </summary>
    /// <param name="messageCreated">The message event that invoked the command</param>
    /// <param name="prefix">The prefix that has been used on the command</param>
    /// <param name="commandName">The name of the command that was used</param>
    /// <param name="arguments">The array of string arguments that were given to the command</param>
    public CommandEvent(MessageEvent messageCreated, string prefix, string commandName, IEnumerable<string> arguments) =>
        (MessageEvent, Prefix, CommandName, Arguments) = (messageCreated, prefix, commandName, arguments);
    #endregion

    #region Additional
    /// <inheritdoc cref="Message.CreateMessageAsync(string)" />
    public async Task<Message> CreateMessageAsync(string content) =>
        await Message.CreateMessageAsync(content).ConfigureAwait(false);
    /// <inheritdoc cref="Message.CreateMessageAsync(string, bool, bool)" />
    public async Task<Message> CreateMessageAsync(string content, bool isPrivate = false, bool isSilent = false) =>
        await Message.CreateMessageAsync(content, isPrivate, isSilent).ConfigureAwait(false);
    /// <inheritdoc cref="Message.CreateMessageAsync(string, Guid[])" />
    public async Task<Message> CreateMessageAsync(string content, params Guid[] replyMessageIds) =>
        await Message.CreateMessageAsync(content, replyMessageIds).ConfigureAwait(false);
    /// <inheritdoc cref="Message.CreateMessageAsync(string, bool, bool, Guid[])" />
    public async Task<Message> CreateMessageAsync(string content, bool isPrivate = false, bool isSilent = false, params Guid[] replyMessageIds) =>
        await Message.CreateMessageAsync(content, isPrivate, isSilent, replyMessageIds).ConfigureAwait(false);
    /// <inheritdoc cref="Message.ReplyAsync(string)" />
    public async Task<Message> ReplyAsync(string content) =>
        await Message.ReplyAsync(content).ConfigureAwait(false);
    /// <inheritdoc cref="Message.ReplyAsync(string, bool, bool)" />
    public async Task<Message> ReplyAsync(string content, bool isPrivate = false, bool isSilent = false) =>
        await Message.ReplyAsync(content, isPrivate, isSilent).ConfigureAwait(false);
    /// <inheritdoc cref="Message.UpdateAsync(string)" />
    public async Task<Message> UpdateAsync(string content) =>
        await Message.UpdateAsync(content).ConfigureAwait(false);
    /// <inheritdoc cref="Message.DeleteAsync()" />
    public async Task DeleteAsync() =>
        await Message.DeleteAsync().ConfigureAwait(false);
    /// <inheritdoc cref="Message.AddReactionAsync(uint)" />
    public async Task<Reaction> AddReactionAsync(uint emoteId) =>
        await Message.AddReactionAsync(emoteId).ConfigureAwait(false);
    #endregion
}