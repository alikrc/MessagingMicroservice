using System;

namespace WebBffAggregator.Config
{
    public static class UrlsConfig
    {
        internal static class Messaging
        {
            internal static string GetMyMessages(string baseUri, int take, int page)
            {
                return $"{baseUri}getMyMessages?pageSize={take}&pageIndex={page}";
            }

            internal static string BlockUser(string baseUri, Guid userIdtoBlock)
            {
                return $"{baseUri}BlockUser?userIdtoBlock={userIdtoBlock}";
            }
        }

    }
}
