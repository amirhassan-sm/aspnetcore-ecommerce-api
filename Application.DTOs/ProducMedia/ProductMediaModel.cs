using System.Text.Json.Serialization;

namespace Application.DTOs.ProducMedia
{
    public class ProductMediaModel
    {
        public int ProductMediaId { get; set; }
        public int ProductId { get; set; }
        public int DisplayOrder { get; set; }

        public int MediaTypeId { get; set; }
        public string Url { get; set; } = string.Empty;
        public long fileSize { get; set; }
        public string UploadingUserName { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
        public string ConfirmByUserName { get; set; } = string.Empty;

        [JsonIgnore]
        public Stream? FileStream { get; set; }

        public string FileName { get; set; } = string.Empty;

    }
}
