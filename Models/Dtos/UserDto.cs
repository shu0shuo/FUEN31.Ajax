﻿namespace WebApplication2.Models.Dtos
{
    public class UserDto
    {
        //public int Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }

        public IFormFile? Avatar { get; set; }

    }
}
