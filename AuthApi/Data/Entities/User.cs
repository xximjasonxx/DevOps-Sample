
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApi.Data.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
    }
}