using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Guilded.Base.Users;

/// <summary>
/// Global minimal information about a user.
/// </summary>
/// <remarks>
/// <para>Defines a normal user with minimal information.</para>
/// </remarks>
/// <seealso cref="User" />
/// <seealso cref="SocialLink" />
public class UserSummary : ClientObject
{
    #region JSON properties
    /// <summary>
    /// Gets the identifier of the user.
    /// </summary>
    /// <value>User ID</value>
    public HashId Id { get; }
    /// <summary>
    /// Gets the type of the user they are.
    /// </summary>
    /// <value>User type</value>
    public UserType Type { get; }
    /// <summary>
    /// Gets the global username of the user.
    /// </summary>
    /// <value>Name</value>
    public string Name { get; set; }
    #endregion

    #region Properties
    /// <summary>
    /// Gets whether the user is a <see cref="UserType.Bot">bot</see>.
    /// </summary>
    /// <value>Is a bot</value>
    public bool IsBot => Type == UserType.Bot;

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="UserSummary"/> with specified properties.
    /// </summary>
    /// <param name="id">The identifier of the user</param>
    /// <param name="type">The type of the user they are</param>
    /// <param name="name">The global username of the user</param>
    [JsonConstructor]
    public UserSummary(
        [JsonProperty(Required = Required.Always)]
        HashId id,

        [JsonProperty]
        UserType? type,

        [JsonProperty(Required = Required.Always)]
        string name
    ) =>
        (Id, Type, Name) = (id, type ?? UserType.User, name);
    #endregion

    #region Additional
    /// <inheritdoc cref="BaseGuildedClient.GetSocialLinkAsync(HashId, HashId, SocialLinkType)"/>
    public async Task<SocialLink> GetSocialLinkAsync(HashId serverId, SocialLinkType linkType) =>
        await ParentClient.GetSocialLinkAsync(serverId, Id, linkType).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.UpdateNicknameAsync(HashId, HashId, string)"/>
    public async Task<string> UpdateNicknameAsync(HashId serverId, string nickname) =>
        await ParentClient.UpdateNicknameAsync(serverId, Id, nickname).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.DeleteMessageAsync(System.Guid, System.Guid)"/>
    public async Task DeleteNicknameAsync(HashId serverId) =>
        await ParentClient.DeleteNicknameAsync(serverId, Id).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.AddRoleAsync(HashId, HashId, uint)"/>
    public async Task AddRoleAsync(HashId serverId, uint roleId) =>
        await ParentClient.AddRoleAsync(serverId, Id, roleId).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.RemoveRoleAsync(HashId, HashId, uint)"/>
    public async Task RemoveRoleAsync(HashId serverId, uint roleId) =>
        await ParentClient.RemoveRoleAsync(serverId, Id, roleId).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.AddXpAsync(HashId, HashId, long)"/>
    public async Task<long> AddXpAsync(HashId serverId, short amount) =>
        await ParentClient.AddXpAsync(serverId, Id, amount).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.KickMemberAsync(HashId, HashId)"/>
    public async Task KickAsync(HashId serverId) =>
        await ParentClient.KickMemberAsync(serverId, Id).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.GetBanAsync(HashId, HashId)"/>
    public async Task GetBanAsync(HashId serverId) =>
        await ParentClient.GetBanAsync(serverId, Id).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.BanMemberAsync(HashId, HashId, string?)"/>
    public async Task BanAsync(HashId serverId, string? reason = null) =>
        await ParentClient.BanMemberAsync(serverId, Id, reason).ConfigureAwait(false);
    /// <inheritdoc cref="BaseGuildedClient.UnbanMemberAsync(HashId, HashId)"/>
    public async Task UnbanAsync(HashId serverId) =>
        await ParentClient.UnbanMemberAsync(serverId, Id).ConfigureAwait(false);
    #endregion

    #region Overrides
    /// <summary>
    /// Returns the string representation of this <see cref="UserSummary"/> instance.
    /// </summary>
    /// <remarks>
    /// <para>The mention syntax of the user will be returned.</para>
    /// </remarks>
    /// <example>
    /// <para>An example of a code with clearly defined identifier:</para>
    /// <code lang="csharp">
    /// UserSummary user = new(new HashId("R40Mp0Wd"), UserType.User, "Example");
    /// Console.WriteLine("Here's the user: {0}", user);
    /// </code>
    /// <para>The output of the code above:</para>
    /// <code lang="bash">
    /// Here's the user: &lt;@R40Mp0Wd&gt;
    /// </code>
    /// </example>
    /// <returns>Markdown user mention</returns>
    public override string ToString() =>
        $"<@{Id}>";
    #endregion
}