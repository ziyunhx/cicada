using System;

namespace Cicada.ViewModels.Common
{
	public class Pager
	{
		public int TotalCount { get; set; }

		public int PageSize { get; set; }

		public string Action { get; set; }

        public string Search { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Status { get; set; }

	    public bool EnableSearch { get; set; } = false;

        public bool EnableTimeFilter { get; set; } = false;
	}
}
