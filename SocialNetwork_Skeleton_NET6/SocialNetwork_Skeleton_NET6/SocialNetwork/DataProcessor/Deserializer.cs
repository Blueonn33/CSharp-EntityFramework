using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SocialNetwork.Data;
using SocialNetwork.Data.Models;
using SocialNetwork.Data.Models.Enums;
using SocialNetwork.DataProcessor.ImportDTOs;
using SocialNetwork.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace SocialNetwork.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format.";
        private const string DuplicatedDataMessage = "Duplicated data.";
        private const string SuccessfullyImportedMessageEntity = "Successfully imported message (Sent at: {0}, Status: {1})";
        private const string SuccessfullyImportedPostEntity = "Successfully imported post (Creator {0}, Created at: {1})";

        public static string ImportMessages(SocialNetworkDbContext dbContext, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportMessageDto>? messageDtos = XmlSerializerWrapper
                .Deserialize<ImportMessageDto[]>(xmlString, "Messages");

            if (messageDtos == null)
            {
                return sb.ToString();
            }

            IEnumerable<Message> existingMessages = dbContext.Messages
                .AsNoTracking()
                .ToArray();

            ICollection<Message> messagesToPersist = new List<Message>();

            foreach (var messageDto in messageDtos)
            {
                bool isValidStatus = Enum.TryParse<MessageStatus>(messageDto.Status, out var parsedStatus);

                bool isValidDate = DateTime.TryParseExact(
                    messageDto.SentAt,
                    "yyyy-MM-ddTHH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime sentAt);

                //bool isDuplicate = existingMessages
                //    .Any(m =>
                //        m.SentAt == sentAt &&
                //        m.Content == messageDto.Content &&
                //        m.Status == parsedStatus &&
                //        m.ConversationId == messageDto.ConversationId &&
                //        m.SenderId == messageDto.SenderId);

                //isDuplicate |= messagesToPersist
                //    .Any(m =>
                //        m.SentAt == sentAt &&
                //        m.Content == messageDto.Content &&
                //        m.Status == parsedStatus &&
                //        m.ConversationId == messageDto.ConversationId &&
                //        m.SenderId == messageDto.SenderId);

                //if (isDuplicate)
                //{
                //    sb.AppendLine(DuplicatedDataMessage);
                //    continue;
                //}


                if (!IsValid(messageDto) || !isValidStatus || !isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool senderExists = dbContext.Users.Any(u => u.Id == messageDto.SenderId);
                bool conversationExists = dbContext.Conversations.Any(c => c.Id == messageDto.ConversationId);

                if (!senderExists || !conversationExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Message message = new Message()
                {
                    Content = messageDto.Content,
                    Status = parsedStatus,
                    ConversationId = messageDto.ConversationId,
                    SenderId = messageDto.SenderId,
                    SentAt = sentAt
                };

                messagesToPersist.Add(message);

                sb.AppendLine(string.Format(
                    SuccessfullyImportedMessageEntity,
                    message.SentAt.ToString("yyyy-MM-ddTHH:mm:ss"),
                    message.Status));
            }

            dbContext.Messages.AddRange(messagesToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportPosts(SocialNetworkDbContext dbContext, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportPostDto>? postDtos = JsonConvert
                .DeserializeObject<ImportPostDto[]>(jsonString);

            if (postDtos == null)
            {
                return sb.ToString();
            }

            IEnumerable<int> validCreatorIds = dbContext.Users
                .AsNoTracking()
                .Select(u => u.Id)
                .ToArray();

            ICollection<Post> postsToPersist = new List<Post>();

            foreach (var postDto in postDtos)
            {
                if (postDto == null ||
                    string.IsNullOrWhiteSpace(postDto.Content) ||
                    string.IsNullOrWhiteSpace(postDto.CreatedAt) ||
                    !DateTime.TryParseExact(postDto.CreatedAt, "yyyy-MM-ddTHH:mm:ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime postDate) ||
                    !validCreatorIds.Contains(postDto.CreatorId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (postDto.Content.Length > 300)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var creator = dbContext.Users.FirstOrDefault(u => u.Id == postDto.CreatorId);

                if (creator == null)
                {
                    continue;
                }

                Post post = new Post()
                {
                    Content = postDto.Content,
                    CreatedAt = postDate,
                    CreatorId = postDto.CreatorId,
                    Creator = creator
                };

                postsToPersist.Add(post);
                sb.AppendLine(string.Format(SuccessfullyImportedPostEntity, creator.Username, post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss")));
            }

            dbContext.Posts.AddRange(postsToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static bool IsValid(object dto)
        {
            ValidationContext validationContext = new ValidationContext(dto);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach (ValidationResult validationResult in validationResults)
            {
                if (validationResult.ErrorMessage != null)
                {
                    string currentMessage = validationResult.ErrorMessage;
                }
            }

            return isValid;
        }
    }
}
