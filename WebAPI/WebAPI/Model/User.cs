﻿namespace WebAPI.API.Model
{
    public class User
    {
       public string Id { get; set; }
       public string UserName { get; set; }
       public string Name { get; set; }
       public string Email { get; set; }
       public string Password { get; set; }
       public int Age { get; set; }
        public string UserRole { get; set; }
    }
}
