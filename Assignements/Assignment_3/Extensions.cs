
namespace Extensions
{
    using System;
    using dotnetKole;
    
    public static class Extension
    {
        public static Item GetHighestLevelItem(this Player player)
        {
            int highestLevel = -1;
            Item highestLevelItem = null;

            foreach (var item in player.Items)
            {
                if (item.Level > highestLevel)
                {
                    highestLevel = item.Level;
                    highestLevelItem = item;
                }
            }

            return highestLevelItem;
        }
    }
}