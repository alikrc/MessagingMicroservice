using System;

namespace WebBffAggregator.Config
{
    public static class UrlsConfig
    {
        internal static class Messaging
        {
            internal static string GetMessages(string baseUri, Guid userId, int take, int page)
            {
                return $"{baseUri}GetMessages?userId={userId.ToString("N")}&pageSize={take}&pageIndex={page}";
            }

            internal static string BlockUser(string baseUri, Guid userIdtoBlock)
            {
                return $"{baseUri}BlockUser?userIdtoBlock={userIdtoBlock.ToString("N")}";
            }

            internal static string SendMessage(string baseUri)
            {
                return $"{baseUri}SendMessage";
            }
        }

    }
}
