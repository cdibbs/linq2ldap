using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Linq2Ldap.Core.Proxies;
using Linq2Ldap.Core.Types;

namespace Linq2Ldap.Core.ExtensionMethods {
    /// <summary>
    /// These methods could not have been implemented as instance methods, since
    /// they need to be able to deal with the case when an instance is null
    /// (in LDAP terms, this is called an existence check, e.g., attr=*).
    ///
    /// To put it another way, null.Matches() is a no-go in an Expression. :-)
    /// </summary>
    public static class PropertyExtensions {

        /// <summary>
        /// Checks whether an LDAP filter pattern matches the source string.
        /// </summary>
        /// <param name="source">The string to match against.</param>
        /// <param name="pattern">The pattern to match (ex: some*thing).</param>
        /// <returns>True, if it matches.</returns>
        public static bool Matches(this string source, string pattern) {
            if (source == null) {
                return false;
            }

            // existence check for single-valued. Multi-valued handled by LdapStringList overload
            if (pattern == "*") {
                return true;
            }

            var pieces = Regex.Split(pattern, @"(?<!\\)\*"); // non-escaped asterisk
            int len = pieces.Length;
            if (len == 0) {
                return source == pattern;
            }

            if (pieces[0] != "" && ! source.StartsWith(pieces[0])) {
                return false;
            }

            int i = 1, pos = pieces[0] == "" ? 0 : pieces[0].Length;
            while(i < len && pos != -1) {
                pos = source.IndexOf(pieces[i], pos);
                pos = pos != -1
                    ? pos + pieces[i].Length
                    : -1;
                i++;
            }

            return (i == len && pieces[len - 1] == "")
                    || pos == source.Length;
        }

        /// <summary>
        /// Checks whether an LDAP filter pattern matches any member of the multi-valued source.
        /// </summary>
        /// <param name="source">The multi-valued source.</param>
        /// <param name="pattern">The LDAP filter pattern (ex: some*thing).</param>
        /// <returns>True, if it matches.</returns>
        public static bool Matches<T, U>(this BaseLdapManyType<T, U> source, string pattern)
            where T: IComparable
            where U: class, IConverter<List<T>>
        {
            if (source == null) {
                return false;
            }

            if (pattern == "*") {
                return true; // existence check operator
            }

            return source.Any(e => Matches(e.ToString(), pattern));
        }

        /// <summary>
        /// Checks whether an LDAP filter pattern matches any member of the multi-valued source.
        /// </summary>
        /// <param name="source">The multi-valued source.</param>
        /// <param name="pattern">The LDAP filter pattern (ex: some*thing).</param>
        /// <returns>True, if it matches.</returns>
        public static bool Matches(this PropertyValueCollection source, string pattern)
        {
            if (source == null) {
                return false;
            }

            if (pattern == "*") {
                return true; // existence check operator
            }


            return source.Any(e => Matches(e.ToString(), pattern));
        }

        /// <summary>
        /// Checks whether an LDAP filter pattern matches any member of the multi-valued source.
        /// </summary>
        /// <param name="source">The multi-valued source.</param>
        /// <param name="pattern">The LDAP filter pattern (ex: some*thing).</param>
        /// <returns>True, if it matches.</returns>
        public static bool Matches<T>(this BaseLdapType<T> source, string pattern)
            where T: IComparable
        {
            if (source == null) {
                return false;
            }

            if (pattern == "*") {
                return true; // existence check operator
            }

            return Matches(source.ToString(), pattern);
        }

        /// <summary>
        /// Checks whether the pattern approximately matches (~=) the source string.
        /// Warning: locally, this does a lower-invariant .Match(). This may not line
        /// up with LDAP implementations. Take local, unit test results with a grain of salt.
        /// </summary>
        /// <param name="source">The string to match against.</param>
        /// <param name="pattern">The pattern to match (ex: some*thing).</param>
        /// <returns>True, if it matches.</returns>
        public static bool Approx(this string source, string pattern) {
            return Matches(source.ToLowerInvariant(), pattern.ToLowerInvariant());
        }

        /// <summary>
        /// Checks whether the pattern approximately matches (~=) any member of the
        /// multi-valued source.
        /// Warning: locally, this does a lower-invariant .Match(). This may not line
        /// up with LDAP implementations. Take local, unit test results with a grain of salt.
        /// </summary>
        /// <param name="source">The multi-valued source to match against.</param>
        /// <param name="pattern">The pattern to match (ex: some*thing).</param>
        /// <returns>True, if it matches.</returns>
        public static bool Approx<T, U>(this BaseLdapManyType<T, U> source, string pattern)
            where T: IComparable
            where U: class, IConverter<List<T>>
        {
            if (source == null) {
                return false;
            }

            if (pattern == "*") {
                return true; // existence check operator
            }

            return source.Any(e => Approx(e.ToString(), pattern));
        }

        /// <summary>
        /// Checks whether the pattern approximately matches (~=) any member of the
        /// multi-valued source.
        /// Warning: locally, this does a lower-invariant .Match(). This may not line
        /// up with LDAP implementations. Take local, unit test results with a grain of salt.
        /// </summary>
        /// <param name="source">The multi-valued source to match against.</param>
        /// <param name="pattern">The pattern to match (ex: some*thing).</param>
        /// <returns>True, if it matches.</returns>
        public static bool Approx(this PropertyValueCollection source, string pattern)
        {
            if (source == null) {
                return false;
            }

            if (pattern == "*") {
                return true; // existence check operator
            }

            return source.Any(e => Approx(e.ToString(), pattern));
        }

        public static bool Approx<T>(this BaseLdapType<T> source, string pattern)
            where T: IComparable
        {
            if (source == null) {
                return false;
            }

            if (pattern == "*") {
                return true; // existence check operator
            }

            return Matches(source, pattern);
        }
    }
}