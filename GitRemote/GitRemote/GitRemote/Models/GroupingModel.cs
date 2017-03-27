using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GitRemote.Models
{
    public class GroupingModel<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public GroupingModel(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach ( var item in items )
            {
                Items.Add(item);
            }
        }
    }
}
