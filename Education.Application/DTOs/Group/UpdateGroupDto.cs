using System;
using System.Collections.Generic;
using System.Text;

namespace Education.Application.DTOs.Group
{
    public class UpdateGroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
