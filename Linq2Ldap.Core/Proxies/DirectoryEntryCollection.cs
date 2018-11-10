using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Linq2Ldap.Core.Proxies
{
    public class DirectoryEntryPropertyCollection: Dictionary<string, PropertyValueCollection>
    {
        public DirectoryEntryPropertyCollection(): base() {}
        public DirectoryEntryPropertyCollection(IDictionary results)
        {
            foreach (var key in results.Keys) {
                if (results[key] is ReadOnlyCollectionBase c) {
                    this.Add(key as string, (PropertyValueCollection)c);
                } else if (results[key] is PropertyValueCollection p) {
                    this.Add(key as string, p);
                } else {
                    throw new ArgumentException(
                        $"Dictionary value must be ReadOnlyCollectionBase or {nameof(PropertyValueCollection)}. Was: {results[key]?.GetType()}.");
                }
            }
        }

        public static implicit operator DirectoryEntryPropertyCollection(DictionaryBase col)
            => new DirectoryEntryPropertyCollection(col);
    }
}
