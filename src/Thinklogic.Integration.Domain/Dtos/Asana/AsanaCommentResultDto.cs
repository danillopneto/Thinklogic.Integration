namespace Thinklogic.Integration.Domain.Dtos.Asana
{
    public class AsanaCommentResultDto
    {
        public string Gid { get; set; }

        public string ResourceType { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsPinned { get; set; }

        public string Type { get; set; }

        public string Text { get; set; }
    }
}
