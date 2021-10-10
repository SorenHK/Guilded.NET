using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Guilded.NET.Base.Events;
using Guilded.NET.Base.Permissions;

namespace Guilded.NET.Base.Content
{
    /// <summary>
    /// A message posted in the chat.
    /// </summary>
    /// <remarks>
    /// <para>An existing/a cached message that can be found in a chat. This can be found in chat channels, voice channels, stream channels and their equivalent threads.</para>
    /// <para>This currently includes both messages of types <see cref="MessageType.Default"/> and <see cref="MessageType.System"/>, but it could be changed in the future.</para>
    /// </remarks>
    /// <seealso cref="ListItem"/>
    /// <seealso cref="ForumThread"/>
    /// <seealso cref="MessageCreatedEvent"/>
    /// <seealso cref="MessageUpdatedEvent"/>
    public class Message : ChannelContent<Guid>
    {
        #region JSON properties

        #region Content
        /// <summary>
        /// The contents of the message.
        /// </summary>
        /// <remarks>
        /// <para>The contents of the message in Markdown format.</para>
        /// <para>This includes images and videos, which are in the format of <c>![](source_url)</c>.</para>
        /// </remarks>
        /// <value>Content in Markdown</value>
        [JsonProperty(Required = Required.Always)]
        public string Content
        {
            get; set;
        }
        /// <summary>
        /// Which messages it is replying to.
        /// </summary>
        /// <remarks>
        /// <para>Specifies which messages were replied to in this message. The max reply limit is 5.</para>
        /// </remarks>
        /// <value>List of message IDs?</value>
        public IList<Guid> ReplyMessageIds
        {
            get; set;
        }
        /// <summary>
        /// Whether the reply is private.
        /// </summary>
        /// <remarks>
        /// <para>Specifies whether the reply is private or not.</para>
        /// <para>This can only be <see langword="true"/> if <see cref="ReplyMessageIds"/> has a value.</para>
        /// </remarks>
        /// <value>Reply is private</value>
        public bool IsPrivate
        {
            get; set;
        }
        /// <summary>
        /// Whether the specified message is a reply
        /// </summary>
        /// <remarks>
        /// <para>Checks whether the message is a reply.</para>
        /// </remarks>
        /// <value>Message is a reply</value>
        [JsonIgnore]
        public bool IsReply => ReplyMessageIds?.Count > 0;
        #endregion

        /// <summary>
        /// The date of when the message was updated.
        /// </summary>
        /// <remarks>
        /// <para>The <see cref="DateTime"/> of when the message was edited.</para>
        /// </remarks>
        /// <value>Updated at?</value>
        public DateTime? UpdatedAt
        {
            get; set;
        }
        /// <summary>
        /// The type of the message.
        /// </summary>
        /// <remarks>
        /// <para>Allows message to be determined as a <see cref="MessageType.Default"/> or <see cref="MessageType.System"/>.</para>
        /// </remarks>
        /// <value>Message type</value>
        [JsonProperty(Required = Required.Always)]
        public MessageType Type
        {
            get; set;
        }
        #endregion

        #region Additional
        /// <summary>
        /// Creates a message in a chat.
        /// </summary>
        /// <remarks>
        /// <para>Creates a new message in the channel of identifier <see cref="ChannelContent{T}.ChannelId"/> where the message is.</para>
        /// <para>This does not automatically include the message in the reply list.</para>
        /// </remarks>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <exception cref="GuildedException"/>
        /// <exception cref="GuildedPermissionException"/>
        /// <exception cref="GuildedResourceException"/>
        /// <exception cref="GuildedRequestException"/>
        /// <exception cref="GuildedAuthorizationException"/>
        /// <exception cref="ArgumentNullException">When the <paramref name="content"/> only consists of whitespace or is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When the <paramref name="content"/> is above the message limit of 4000 characters</exception>
        /// <permission cref="ChatPermissions.ReadMessages">Required for reading all channel and thread messages</permission>
        /// <permission cref="ChatPermissions.SendMessages">Required for sending a message in a channel</permission>
        /// <permission cref="ChatPermissions.SendThreadMessages">Required for sending a message in a thread</permission>
        /// <returns>Message created</returns>
        public async Task<Message> CreateMessageAsync(string content) =>
            await ParentClient.CreateMessageAsync(ChannelId, content).ConfigureAwait(false);
        /// <inheritdoc cref="CreateMessageAsync(string)"/>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <param name="replyMessageIds">The array of all messages it is replying to(5 max)</param>
        public async Task<Message> CreateMessageAsync(string content, params Guid[] replyMessageIds) =>
            await ParentClient.CreateMessageAsync(ChannelId, content, replyMessageIds).ConfigureAwait(false);
        /// <inheritdoc cref="CreateMessageAsync(string)"/>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <param name="isPrivate">Whether the reply is private</param>
        /// <param name="replyMessageIds">The array of all messages it is replying to(5 max)</param>
        public async Task<Message> CreateMessageAsync(string content, bool isPrivate, params Guid[] replyMessageIds) =>
            await ParentClient.CreateMessageAsync(ChannelId, content, isPrivate, replyMessageIds).ConfigureAwait(false);
        /// <summary>
        /// Replies to the message in the chat.
        /// </summary>
        /// <remarks>
        /// <para>Creates a new message in the channel of identifier <see cref="ChannelContent{T}.ChannelId"/> where the message is.</para>
        /// <para>Includes the message in the reply list.</para>
        /// </remarks>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <exception cref="GuildedException"/>
        /// <exception cref="GuildedPermissionException"/>
        /// <exception cref="GuildedResourceException"/>
        /// <exception cref="GuildedRequestException"/>
        /// <exception cref="GuildedAuthorizationException"/>
        /// <exception cref="ArgumentNullException">When the <paramref name="content"/> only consists of whitespace or is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When the <paramref name="content"/> is above the message limit of 4000 characters</exception>
        /// <permission cref="ChatPermissions.ReadMessages">Required for reading all channel and thread messages</permission>
        /// <permission cref="ChatPermissions.SendMessages">Required for sending a message in a channel</permission>
        /// <permission cref="ChatPermissions.SendThreadMessages">Required for sending a message in a thread</permission>
        /// <returns>Message created</returns>
        public async Task<Message> ReplyAsync(string content) =>
            await CreateMessageAsync(content, Id).ConfigureAwait(false);
        /// <inheritdoc cref="ReplyAsync(string)"/>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        /// <param name="isPrivate">Whether the reply is private</param>
        public async Task<Message> ReplyAsync(string content, bool isPrivate) =>
            await CreateMessageAsync(content, isPrivate, Id).ConfigureAwait(false);
        /// <inheritdoc cref="BaseGuildedClient.UpdateMessageAsync(Guid, Guid, string)"/>
        /// <param name="content">The contents of the message in Markdown plain text</param>
        public async Task<Message> UpdateMessageAsync(string content) =>
            await ParentClient.UpdateMessageAsync(ChannelId, Id, content).ConfigureAwait(false);
        /// <inheritdoc cref="BaseGuildedClient.DeleteMessageAsync(Guid, Guid)"/>
        public async Task DeleteMessageAsync() =>
            await ParentClient.DeleteMessageAsync(ChannelId, Id).ConfigureAwait(false);
        /// <inheritdoc cref="BaseGuildedClient.AddReactionAsync(Guid, Guid, uint)"/>
        /// <param name="emoteId">The identifier of the emote to add</param>
        public async Task<Reaction> AddReactionAsync(uint emoteId) =>
            await ParentClient.AddReactionAsync(ChannelId, Id, emoteId).ConfigureAwait(false);
        /// <inheritdoc cref="BaseGuildedClient.RemoveReactionAsync(Guid, Guid, uint)"/>
        /// <param name="emoteId">The identifier of the emote to remove</param>
        public async Task RemoveReactionAsync(uint emoteId) =>
            await ParentClient.RemoveReactionAsync(ChannelId, Id, emoteId).ConfigureAwait(false);
        #endregion
    }
}
