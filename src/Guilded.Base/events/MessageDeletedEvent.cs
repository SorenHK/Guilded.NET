using System;
using Newtonsoft.Json;

namespace Guilded.Base.Events;

/// <summary>
/// Represents an event with the name <c>ChatMessageDeleted</c> and opcode <c>0</c> that occurs once someone creates/posts a message in the chat.
/// </summary>
/// <seealso cref="MessageDeleted"/>
/// <seealso cref="Content.Message"/>
public class MessageDeletedEvent : MessageEvent<MessageDeletedEvent.MessageDeleted>
{
    #region Properties
    /// <inheritdoc cref="MessageDeleted.Id"/>
    public Guid Id => Message.Id;
    /// <inheritdoc cref="MessageDeleted.ChannelId"/>
    public Guid ChannelId => Message.ChannelId;
    /// <inheritdoc cref="MessageDeleted.DeletedAt"/>
    public DateTime DeletedAt => Message.DeletedAt;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="MessageDeletedEvent"/> from the specified JSON properties.
    /// </summary>
    /// <param name="serverId">The identifier of the server where the message was deleted</param>
    /// <param name="message">The minimal information about the deleted message</param>
    [JsonConstructor]
    public MessageDeletedEvent(
        [JsonProperty(Required = Required.Always)]
        MessageDeleted message,

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        HashId? serverId
    ) : base(serverId, message) { }
    #endregion

    /// <summary>
    /// Represents a message that was recently deleted/removed.
    /// </summary>
    /// <seealso cref="Content.Message"/>
    /// <seealso cref="MessageDeletedEvent"/>
    public class MessageDeleted : BaseObject
    {
        #region JSON properties
        /// <summary>
        /// Gets the identifier of the message.
        /// </summary>
        /// <value>Message ID</value>
        public Guid Id { get; }
        /// <summary>
        /// Gets the identifier of the channel where the message was.
        /// </summary>
        /// <value>Channel ID</value>
        public Guid ChannelId { get; }
        /// <summary>
        /// Gets the identifier of the server where the message was.
        /// </summary>
        /// <value>Server ID?</value>
        public HashId? ServerId { get; }
        /// <summary>
        /// Gets the date when the message was deleted.
        /// </summary>
        /// <value>Date</value>
        public DateTime DeletedAt { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// The identifier of the message.
        /// </summary>
        /// <param name="id">The identifier of the message</param>
        /// <param name="channelId">The identifier of the channel where the message was</param>
        /// <param name="serverId">The identifier of the server where the message was</param>
        /// <param name="deletedAt">the date when the message was deleted</param>
        [JsonConstructor]
        public MessageDeleted(
            [JsonProperty(Required = Required.Always)]
            Guid id,

            [JsonProperty(Required = Required.Always)]
            Guid channelId,

            [JsonProperty(Required = Required.Always)]
            DateTime deletedAt,

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            HashId? serverId = null
        ) =>
            (Id, ChannelId, ServerId, DeletedAt) = (id, channelId, serverId, deletedAt);
        #endregion

        #region Overrides
        /// <summary>
        /// Creates string equivalent of the message.
        /// </summary>
        /// <returns>Message as string</returns>
        public override string ToString() =>
            $"Content {Id}";
        /// <summary>
        /// Returns whether this and <paramref name="obj"/> are equal to each other.
        /// </summary>
        /// <param name="obj">Another object to compare</param>
        /// <returns>Are equal</returns>
        public override bool Equals(object? obj) =>
            obj is MessageDeleted message && message.ChannelId == ChannelId && message.Id == Id;
        /// <summary>
        /// Gets a hashcode of this object.
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode() =>
            HashCode.Combine(ChannelId, Id);
        #endregion
    }
}