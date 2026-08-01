using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SocialNetwork.Data;
using SocialNetwork.DataProcessor.ExportDTOs;
using SocialNetwork.Utilities;
using System.Globalization;

namespace SocialNetwork.DataProcessor
{
    public class Serializer
    {
        public static string ExportUsersWithFriendShipsCountAndTheirPosts(SocialNetworkDbContext dbContext)
        {
            var users = dbContext.Users
                .AsNoTracking()
                .Include(u => u.Posts)
                .ToList();

            var friendships = dbContext.Friendships
                .AsNoTracking()
                .ToList();

            var usersDto = users
                .OrderBy(u => u.Username)
                .Select(u => new ExportUsersWithFriendShipsCountAndTheirPosts
                {
                    Username = u.Username,
                    Friendships = friendships
                        .Count(f => f.UserOneId == u.Id || f.UserTwoId == u.Id),

                    Posts = u.Posts
                        .OrderBy(p => p.Id) // 🔥 Точно това иска задачата
                        .Select(p => new ExportPost
                        {
                            Content = p.Content,
                            CreatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss")
                        })
                        .ToArray()
                })
                .ToArray();

            return XmlSerializerWrapper.Serialize(usersDto, "Users");
        }

        public static string ExportConversationsWithMessagesChronologically(SocialNetworkDbContext dbContext)
        {
            var conversations = dbContext.Conversations
                .AsNoTracking()
                .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
                .OrderBy(c => c.StartedAt)
                .ToList();

            var result = conversations
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    StartedAt = c.StartedAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),

                    Messages = c.Messages
                        .OrderBy(m => m.SentAt)
                        .Select(m => new
                        {
                            Content = m.Content,
                            SentAt = m.SentAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                            Status = m.Status,
                            SenderUsername = m.Sender.Username
                        })
                        .GroupBy(x => new { x.Content, x.SentAt, x.SenderUsername })
                        .Select(g => g.First())
                        .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }


    }
}
