using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;

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

        [Description("asana-project-path")]
        public string AsanaProjectPath { get; set; }

        [Description("asana-task-path")]
        public string AsanaTaskPath { get; set; }

        [Description("filter-path")]
        public string FilterPath { get; set; }

        [Description("filter-value")]
        public string FilterValue { get; set; }

        public FunctionParameters(HttpRequestMessage req)
        {
            foreach (var parameter in this.GetType().GetProperties())
            {
                DescriptionAttribute key = (DescriptionAttribute)Attribute.GetCustomAttribute(parameter, typeof(DescriptionAttribute));
                if (req.Headers.Contains(key.Description))
                {
                    var value = req.Headers.GetValues(key.Description).First();
                    parameter.SetValue(this, value);
                }
            }
        }
    }
}