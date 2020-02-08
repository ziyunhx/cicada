using System.Collections.Generic;

namespace Cicada.ViewModels.Identity
{
	public class UsersDto
	{
		public UsersDto()
		{
			Users = new List<UserDto>();
		}

	    public int PageSize { get; set; }

	    public int TotalCount { get; set; }

        public List<UserDto> Users { get; set; }
	}
}