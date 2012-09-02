using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingSiteCheck.Resource
{
    public interface IResourceCollector
    {
        void Add(string path);
        ResourceType ResourceType { get; set; }
        bool Debug { get; set; }
        string Group { get; set; }
        string BaseFolder { get; set; }
        string BuildHtmlTag();
    }
}
