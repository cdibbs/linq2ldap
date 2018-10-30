using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using AutoMapper;

namespace Linq2Ldap.Protocols {
    public class LinqSearchResponse<T> {
        internal SearchResponse Response { get; private set; }

        public LinqSearchResponse(SearchResponse response) {
            Response = response;
        }

        public IEnumerable<T> Entries { get; set; }

        public static implicit operator LinqSearchResponse<T>(SearchResponse response)
            => new LinqSearchResponse<T>(response);

        public static implicit operator SearchResponse(LinqSearchResponse<T> response)
            => response.Response;
    }
}