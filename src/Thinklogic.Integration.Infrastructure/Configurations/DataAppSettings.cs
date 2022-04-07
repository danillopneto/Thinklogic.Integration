namespace Thinklogic.Integration.Infrastructure.Configurations
{
    public class DataAppSettings
    {
        public string AsanaPersonalAccessToken { get; set; }

        public string UrlAsana { get; set; }

        public string WorkspaceId { get; set; }

        public string ProjectNamePattern { get; set; }

        public string TaskNameReplacePattern { get; set; }
    }
}
