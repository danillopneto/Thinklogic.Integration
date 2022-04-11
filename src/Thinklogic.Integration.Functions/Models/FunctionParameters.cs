using System.ComponentModel;

namespace Thinklogic.Integration.Functions.Models
{
    public class FunctionParameters
    {
        [Description("asana-comment-path")]
        public string AsanaCommentPath { get; set; }

        [Description("asana-custom-field-key")]
        public string AsanaCustomFieldKey { get; set; }

        [Description("asana-custom-field-value")]
        public string AsanaCustomFieldValue { get; set; }

        [Description("asana-title-path")]
        public string AsanaTitlePath { get; set; }

        [Description("filter-path")]
        public string FilterPath { get; set; }

        [Description("filter-value")]
        public string FilterValue { get; set; }
    }
}