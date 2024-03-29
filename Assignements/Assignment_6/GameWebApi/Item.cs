using System;
using System.ComponentModel.DataAnnotations;

namespace dotnetKole
{
    public class Item
    {
        public Guid Id { get; set; }
        
        [Range(1, 99)]
        public int Level { get; set; }
        public int Price { get; set; }

        [Required]
        public ItemType ItemType { get; set; }

        [CustomValidation.PastDate]
        public DateTime CreationDate{ get; set; }
    }
}
