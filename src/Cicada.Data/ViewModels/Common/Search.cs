using System;

namespace Cicada.ViewModels.Common
{
    public class Search
    {
        public string Action { get; set; }

        public string Controller { get; set; }

        public int PageSize { get; set; }

        public string AddEntityLabel { get; set; }

        public string AddEntityUrl { get; set; }
    }
}