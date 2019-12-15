using System;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }
    }
}