using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Guilded.NET.Base;
using Guilded.NET.Base.Embeds;
using Guilded.NET.Base.Content;
using Guilded.NET.Base.Permissions;
using RestSharp;

namespace Guilded.NET
{
    public abstract partial class AbstractGuildedClient
    {
        private const int messageLimit = 4000;

        #region Webhook
        /// <inheritdoc/>
        private async Task CreateHookMessageAsync(Guid webhookId, string token, MessageContent message) =>
            await ExecuteRequestAsync(new RestRequest(new Uri(GuildedUrl.Media, $"webhooks/{webhookId}/{token}"), Method.POST).AddJsonBody(message)).ConfigureAwait(false);
        /// <inheritdoc/>
        public override async Task CreateHookMessageAsync(Guid webhookId, string token, string content) =>
            await CreateHookMessageAsync(webhookId, token, new MessageContent { Content = content }).ConfigureAwait(false);
        /// <inheritdoc/>
        public override async Task CreateHookMessageAsync(Guid webhookId, string token, string content, IList<Embed> embeds) =>
            await CreateHookMessageAsync(webhookId, token, new MessageContent { Content = content, Embeds = embeds }).ConfigureAwait(false);
        /// <inheritdoc/>
        public override async Task CreateHookMessageAsync(Guid webhookId, string token, IList<Embed> embeds) =>
            await CreateHookMessageAsync(webhookId, token, new MessageContent { Embeds = embeds }).ConfigureAwait(false);
        #endregion

        #region Chat channel
        /// <inheritdoc/>
        public override async Task<IList<Message>> GetMessagesAsync(Guid channelId, bool includePrivate = false) =>
            await GetObject<IList<Message>>(new RestRequest($"channels/{channelId}/messages?includePrivate={includePrivate}", Method.GET), "messages").ConfigureAwait(false);
        /// <inheritdoc/>
        public override async Task<Message> GetMessageAsync(Guid channelId, Guid messageId) =>
            await GetObject<Message>(new RestRequest($"channels/{channelId}/messages/{messageId}", Method.GET), "message").ConfigureAwait(false);
        /// <summary>
        /// Creates a message in chat.
        /// </summary>
        /// <remarks>
        /// <para>Creates a new chat messsage in the specified channel.</para>
        /// </remarks>
        /// <param name="channelId">The identifier of the parent channel</param>
        /// <param name="message">The message to send</param>
        /// <exception cref="GuildedException"/>
        /// <exception cref="GuildedPermissionException"/>
        /// <exception cref="GuildedResourceException"/>
        /// <exception cref="GuildedAuthorizationException"/>
        /// <permission cref="ChatPermissions.ReadMessages">Required for reading all channel and thread messages</permission>
        /// <permission cref="ChatPermissions.SendMessages">Required for sending a message in a channel</permission>
        /// <permission cref="ChatPermissions.SendThreadMessages">Required for sending a message in a thread</permission>
        /// <returns>Message created</returns>
        private async Task<Message> CreateMessageAsync(Guid channelId, MessageContent message) =>
            await GetObject<Message>(new RestRequest($"channels/{channelId}/messages", Method.POST).AddJsonBody(message), "message").ConfigureAwait(false);
        /// <inheritdoc/>
        public override async Task<Message> CreateMessageAsync(Guid channelId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));
            else if (content.Length > messageLimit)
                throw new ArgumentOutOfRangeException(nameof(content), content, $"{nameof(content)} exceeds the 4000 character message limit");
            else
                return await CreateMessageAsync(channelId, new MessageContent { Content = content }).ConfigureAwait(false);
        }
        /// <inheritdoc/>
        public override async Task<Message> CreateMessageAsync(Guid channelId, string content, params Guid[] replyMessageIds)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }
            else if (content.Length > messageLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(content), content, $"{nameof(content)} exceeds the 4000 character message limit");
            }
            else
            {
                return await CreateMessageAsync(channelId, new MessageContent
                {
                    Content = content,
                    ReplyMessageIds = replyMessageIds
                }).ConfigureAwait(false);
            }
        }
        /// <inheritdoc/>
        public override async Task<Message> CreateMessageAsync(Guid channelId, string content, bool isPrivate, params Guid[] replyMessageIds)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }
            else if (content.Length > messageLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(content), content, $"{nameof(content)} exceeds the 4000 character message limit");
            }
            else
            {
                return await CreateMessageAsync(channelId, new MessageContent
                {
                    Content = content,
                    IsPrivate = isPrivate,
                    ReplyMessageIds = replyMessageIds
                }).ConfigureAwait(false);
            }
        }
        /// <inheritdoc/>
        public override async Task<Message> UpdateMessageAsync(Guid channelId, Guid messageId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }
            else if (content.Length > messageLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(content), content, $"{nameof(content)} exceeds the 4000 character message limit");
            }
            else
            {
                return await CreateMessageAsync(channelId, new MessageContent
                {
                    Content = content
                }).ConfigureAwait(false);
            }
        }
        /// <inheritdoc/>
        public override async Task DeleteMessageAsync(Guid channelId, Guid messageId) =>
            await ExecuteRequestAsync(new RestRequest($"channels/{channelId}/messages/{messageId}", Method.DELETE)).ConfigureAwait(false);
        /// <inheritdoc/>
        public override async Task<Reaction> AddReactionAsync(Guid channelId, Guid messageId, uint emoteId) =>
            await GetObject<Reaction>(new RestRequest($"channels/{channelId}/content/{messageId}/emotes/{emoteId}", Method.PUT), "emote").ConfigureAwait(false);
        /// <inheritdoc/>
        public override async Task RemoveReactionAsync(Guid channelId, Guid messageId, uint emoteId) =>
            await ExecuteRequestAsync(new RestRequest($"channels/{channelId}/content/{messageId}/emotes/{emoteId}", Method.DELETE)).ConfigureAwait(false);
        #endregion

        #region Forum channels
        /// <inheritdoc/>
        public override async Task<ForumThread> CreateForumThreadAsync(Guid channelId, string title, string content)
        {
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));
            else if(string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));

            return await GetObject<ForumThread>(new RestRequest($"channels/{channelId}/forum", Method.POST)
                .AddJsonBody(new
                {
                    title,
                    content
                })
            , "forumThread").ConfigureAwait(false);
        }
        #endregion

        #region List channels
        /// <inheritdoc/>
        public override async Task<ListItem> CreateListItemAsync(Guid channelId, string message, string note = null)
        {
            if(string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            return await GetObject<ListItem>(new RestRequest($"channels/{channelId}/list", Method.POST)
                .AddJsonBody(new
                {
                    message,
                    note
                })
            , "listItem").ConfigureAwait(false);
        }
        #endregion

        #region Content
        /// <inheritdoc/>
        public override async Task<Reaction> AddReactionAsync(Guid channelId, uint contentId, uint emoteId) =>
            await GetObject<Reaction>(new RestRequest($"channels/{channelId}/content/{contentId}/emotes/{emoteId}", Method.PUT), "emote").ConfigureAwait(false);
        /// <inheritdoc/>
        public override async Task RemoveReactionAsync(Guid channelId, uint contentId, uint emoteId) =>
            await ExecuteRequestAsync(new RestRequest($"channels/{channelId}/content/{contentId}/emotes/{emoteId}", Method.DELETE)).ConfigureAwait(false);
        #endregion
    }
}