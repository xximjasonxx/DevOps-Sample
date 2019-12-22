using System;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Data.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}