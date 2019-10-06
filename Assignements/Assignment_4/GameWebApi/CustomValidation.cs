using System;
using System.ComponentModel.DataAnnotations;

namespace dotnetKole
{
    public class CustomValidation
    {
        public sealed class PastDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var currentDateTime = new DateTime();

                if ((DateTime)value > currentDateTime)
                {
                    return new ValidationResult($"Date cannot be newer than the current date {value}.");
                }

                return ValidationResult.Success;
            }
        }

        public class SwordMinLevelManager : ValidationAttribute
        {
            private Player _player { get; set; }
            private Item _item { get; set; }


            public SwordMinLevelManager(Player player, Item item) 
            {
                _player = player;
                _item = item;
            }

            public SwordMinLevelManager IfSwordCheckPlayerLevel() 
            {
                if (_player.Level < 3 && _item.ItemType == ItemType.Sword) 
                {
                    throw new ArgumentException(string.Format("Player: {0} level {1} is too low. Sword level {2}", _player.Id, _player.Level, _item.Level));
                }

                return this;
            }
        }
    }
}
