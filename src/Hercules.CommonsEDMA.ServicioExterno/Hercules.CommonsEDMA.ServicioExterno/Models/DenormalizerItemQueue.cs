using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models
{
    public class DenormalizerItemQueue
    {
        public enum ItemType
        {
            person,
            group,
            project,
            document,
            researchobject
        }

        public ItemType _itemType { get; set; }
        public HashSet<string> _items { get; set; }

        public DenormalizerItemQueue(ItemType itemType, HashSet<string> items)
        {
            _itemType = itemType;
            _items = items;
        }
    }
}
