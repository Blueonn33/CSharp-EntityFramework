using Microsoft.EntityFrameworkCore;
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

                //bool isDuplicate = existingMessages
                //    .Any(m => m.ConversationId == messageDto.ConversationId &&
                //              m.SenderId == messageDto.SenderId &&
                //              m.SentAt == sentAt);

                //isDuplicate |= messagesToPersist
                //    .Any(m => m.ConversationId == messageDto.ConversationId &&
                //              m.SenderId == messageDto.SenderId &&
                //              m.SentAt == sentAt);

                //if (isDuplicate)
                //{
                //    sb.AppendLine(DuplicatedDataMessage);
                //    continue;
                //}

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
            throw new NotImplementedException();
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
