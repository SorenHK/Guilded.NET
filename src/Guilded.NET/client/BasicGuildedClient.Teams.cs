using System;
using System.Threading.Tasks;

using RestSharp;

namespace Guilded.NET
{
    using Base;

    /// <summary>
    /// Logged-in user in Guilded.
    /// </summary>
    public abstract partial class BasicGuildedClient
    {
        #region Members
        /// <summary>
        /// Adds a role to the given user.
        /// </summary>
        /// <example>
        /// <code>
        /// await client.AddRoleAsync(message.CreatedBy, 100000000);
        /// </code>
        /// </example>
        /// <param name="memberId">The identifier of the receiving user</param>
        /// <param name="roleId">The identifier of the role to add</param>
        /// <exception cref="GuildedException">When the client receives an error from Guilded API</exception>
        public override async Task AddRoleAsync(GId memberId, uint roleId) =>
            await ExecuteRequest($"members/{memberId}/roles/{roleId}", Method.PUT);
        /// <summary>
        /// Removes a role from the given user.
        /// </summary>
        /// <example>
        /// <code>
        /// await client.RemoveRoleAsync(message.CreatedBy, 100000000);
        /// </code>
        /// </example>
        /// <param name="memberId">The identifier of the losing user</param>
        /// <param name="roleId">The identifier of the role to remove</param>
        /// <exception cref="GuildedException">When the client receives an error from Guilded API</exception>
        public override async Task RemoveRoleAsync(GId memberId, uint roleId) =>
            await ExecuteRequest($"members/{memberId}/roles/{roleId}", Method.DELETE);
        /// <summary>
        /// Adds XP to the given user.
        /// </summary>
        /// <example>
        /// <code>
        /// await client.AddXpAsync(message.CreatedBy, 10);
        /// </code>
        /// </example>
        /// <param name="userId">The identifier of the receiving user</param>
        /// <param name="xpAmount">The amount of XP received from -1000 to 1000</param>
        /// <exception cref="ArgumentOutOfRangeException">When the amount of XP given exceeds the limit</exception>
        /// <exception cref="GuildedException">When the client receives an error from Guilded API</exception>
        /// <returns>Total XP</returns>
        public override async Task<long> AddXpAsync(GId userId, short xpAmount)
        {
            // Checks if it's not too much or too little
            if (xpAmount > 1000 || xpAmount < -1000)
                throw new ArgumentOutOfRangeException($"Expected {nameof(xpAmount)} to be between 1000 and -1000, but got {xpAmount} instead");
            // Gives XP to the user
            return await GetObject<long>($"members/{userId}/xp", Method.POST, "total", new
            {
                amount = xpAmount
            });
        }
        #endregion

        #region Roles
        /// <summary>
        /// Attaches amount of XP required to a role.
        /// </summary>
        /// <example>
        /// <code>
        /// await client.AttachRoleLevelAsync(1000000000, 2048);
        /// </code>
        /// </example>
        /// <param name="roleId">The identifier of the editing role</param>
        /// <param name="amount">The amount XP added</param>
        /// <exception cref="GuildedException">When the client receives an error from Guilded API</exception>
        public override async Task AttachRoleLevelAsync(uint roleId, long amount) =>
            await ExecuteRequest($"roles/{roleId}/xp", Method.POST, new
            {
                amount
            });
        #endregion

        #region Groups
        /// <summary>
        /// Adds a member to the group.
        /// </summary>
        /// <example>
        /// <code>
        /// await client.AddMembershipAsync(group.Id, message.CreatedBy);
        /// </code>
        /// </example>
        /// <param name="groupId">The identifier of the parent group</param>
        /// <param name="memberId">The identifier of the member to add</param>
        /// <exception cref="GuildedException">When the client receives an error from Guilded API</exception>
        public override async Task AddMembershipAsync(GId groupId, GId memberId) =>
            await ExecuteRequest($"groups/{groupId}/members/{memberId}", Method.PUT);
        /// <summary>
        /// Removes a member from the group.
        /// </summary>
        /// <example>
        /// <code>
        /// await client.RemoveMembershipAsync(group.Id, message.CreatedBy);
        /// </code>
        /// </example>
        /// <param name="groupId">The identifier of the parent group</param>
        /// <param name="memberId">The identifier of the member to remove</param>
        /// <exception cref="GuildedException">When the client receives an error from Guilded API</exception>
        public override async Task RemoveMembershipAsync(GId groupId, GId memberId) =>
            await ExecuteRequest($"groups/{groupId}/members/{memberId}", Method.DELETE);
        #endregion
    }
}