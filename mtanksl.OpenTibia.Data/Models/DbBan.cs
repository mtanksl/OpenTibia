using System;
using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbBan
    {
        public int Id { get; set; }

        public BanType Type { get; set; }

        public int? AccountId { get; set; }

        public int? PlayerId { get; set; }

        [StringLength(255)]
        public string IpAddress { get; set; }

        [Required]
        [StringLength(255)]
        public string Message { get; set; }

        public DateTime CreationDate { get; set; }


        public DbAccount Account { get; set; }

        public DbPlayer Player { get; set; }
    }

    public enum BanType
    {
        Account,

        Player,

        IpAddress
    }
}