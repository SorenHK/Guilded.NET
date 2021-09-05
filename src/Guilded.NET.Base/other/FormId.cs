using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace Guilded.NET.Base
{
    /// <summary>
    /// The identifier for forms and media uploads.
    /// </summary>
    /// <seealso cref="Guid"/>
    /// <seealso cref="GId"/>
    [TypeConverter(typeof(FormIdConverter))]
    [JsonConverter(typeof(IdConverter))]
    public struct FormId : IEquatable<FormId>
    {
        internal readonly string _;
        private const int partLength = 7;
        private const string partChars = "0123456789";
        private static readonly Random random = new Random();
        /// <summary>
        /// Creates a random value of <see cref="FormId"/>.
        /// </summary>
        /// <value>New form ID</value>
        public static FormId Random => new FormId($"r-{random.Next(1000000, 9999999)}-{random.Next(1000000, 9999999)}");
        /// <summary>
        /// The identifier for forms and media uploads.
        /// </summary>
        /// <param name="id">The raw string in the format of Form/Media ID</param>
        /// <exception cref="FormatException">When the given ID string is in incorrect format</exception>
        public FormId(string id)
        {
            // Make sure it's in correct format
            if (!Check(id))
                throw GId.FormatError;

            _ = id;
        }

        #region Overrides
        /// <summary>
        /// Returns string representation of <see cref="FormId"/> instance.
        /// </summary>
        /// <returns><see cref="FormId"/> as string</returns>
        public override string ToString() =>
            _;
        /// <summary>
        /// Gets a hashcode of this object.
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode() =>
            HashCode.Combine(_, 1);
        /// <summary>
        /// Returns whether this and <paramref name="other"/> are equal to each other.
        /// </summary>
        /// <param name="other">Another identifier to compare</param>
        /// <returns>Are equal</returns>
        public bool Equals(FormId other) =>
            other == this;
        /// <summary>
        /// Returns whether this and <paramref name="obj"/> are equal to each other.
        /// </summary>
        /// <param name="obj">Another object to compare</param>
        /// <returns>Are equal</returns>
        public override bool Equals(object obj) =>
            obj is FormId id && Equals(id);
        #endregion

        #region Operators
        /// <summary>
        /// Checks if given <see cref="FormId"/>s are the same.
        /// </summary>
        /// <param name="id0">First ID to be compared</param>
        /// <param name="id1">Second ID to be compared</param>
        /// <returns>Are equal</returns>
        public static bool operator ==(FormId id0, FormId id1) =>
            id0._ == id1._;
        /// <summary>
        /// Checks if given <see cref="FormId"/>s are the same.
        /// </summary>
        /// <param name="id0">First ID to be compared</param>
        /// <param name="id1">Second ID to be compared</param>
        /// <returns>Aren't equal</returns>
        public static bool operator !=(FormId id0, FormId id1) =>
            !(id0 == id1);
        #endregion

        #region Static methods
        /// <summary>
        /// Checks if given string is in the correct <see cref="FormId"/> format.
        /// </summary>
        /// <param name="str">A raw string to check</param>
        /// <returns>Correct formatting</returns>
        public static bool Check(string str)
        {
            // Make sure it's in the format of r-1000000-1000000

            // (r)-1000000-1000000
            if (!str.StartsWith('r') || string.IsNullOrWhiteSpace(str))
                return false;
            // Split by - and leave out 'r'
            // r-(1000000-1000000)
            List<string> split = str.Split('-').Skip(1).ToList();
            // r-(1000000)-(1000000)
            return split.Count == 2 && !split.Any(IsFormIdPart);
        }
        /// <summary>
        /// Checks if <paramref name="part"/> is in 7 digits.
        /// </summary>
        /// <param name="part">The part of the <see cref="FormId"/> to check</param>
        /// <returns>Is <see cref="FormId"/> part</returns>
        private static bool IsFormIdPart(string part) =>
            part.Length == partLength && part.All(ch => partChars.Contains(ch));
        #endregion
    }
}