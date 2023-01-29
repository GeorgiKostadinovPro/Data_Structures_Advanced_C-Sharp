namespace HashTable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class HashTable<TKey, TValue> : IEnumerable<KeyValue<TKey, TValue>>
    {
        private const int DefaultCapacity = 4;
        private const float LoadFactor = 0.75f;
        private LinkedList<KeyValue<TKey, TValue>>[] slots;

        public HashTable() 
            : this(DefaultCapacity)
        { }

        public HashTable(int capacity)
        {
            this.slots = new LinkedList<KeyValue<TKey, TValue>>[capacity];
        }

        public int Count { get; private set; }

        public int Capacity
        { 
            get 
            { 
                return this.slots.Length; 
            }    
        }

        public void Add(TKey key, TValue value)
        {
            this.GrowIfNeeded();

            int index = Math.Abs(key.GetHashCode()) % this.Capacity;

            if (this.slots[index] == null)
            {
                this.slots[index] = new LinkedList<KeyValue<TKey, TValue>>();
            }

            foreach (var element in this.slots[index])
            {
                if (element.Key.Equals(key))
                {
                    throw new ArgumentException("Key already exists!", key.ToString());
                }
            }

            var newElement = new KeyValue<TKey, TValue>(key, value);
            this.slots[index].AddLast(newElement);
            this.Count++;
        }

        public bool AddOrReplace(TKey key, TValue value)
        {
            try
            {
                this.Add(key, value);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("Key already exists!")
                    && ex.ParamName == key.ToString())
                {
                    int index = Math.Abs(key.GetHashCode()) % this.Capacity;

                    var keyValuePair = this.slots[index]
                        .FirstOrDefault(kvp => kvp.Key.Equals(key));
                    
                    keyValuePair.Value = value;
                    return true;
                }

                throw ex;
            }

            return false;
        }

        public TValue Get(TKey key)
        {
            var kvp = this.Find(key);

            if (kvp == null)
            {
                throw new KeyNotFoundException();
            }

            return kvp.Value;
        }

        public TValue this[TKey key]
        {
            get
            {
                return this.Get(key);
            }
            set
            {
                this.AddOrReplace(key, value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var element = this.Find(key);

            if(element != null)
            {
                value = element.Value;
                return true;
            }

            value = default;
            return false;
        }

        public KeyValue<TKey, TValue> Find(TKey key)
        {
            int index = Math.Abs(key.GetHashCode()) % this.Capacity;

            if (this.slots[index] != null)
            {
                foreach (var kvp in this.slots[index])
                {
                    if (kvp.Key.Equals(key))
                    {
                        return kvp;
                    }
                }
            }

            return null;
        }

        public bool ContainsKey(TKey key)
        {
            return this.Find(key) != null;
        }

        public bool Remove(TKey key)
        {
            int index = Math.Abs(key.GetHashCode()) % this.Capacity;

            if (this.slots[index] != null)
            {
                var currentKvp = this.slots[index].First;

                while (currentKvp != null)
                {
                    if (currentKvp.Value.Key.Equals(key))
                    {
                        this.slots[index].Remove(currentKvp);
                        this.Count--;
                        return true;
                    }

                    currentKvp = currentKvp.Next;
                }
            }

            return false;
        }

        public void Clear()
        {
            this.slots = new LinkedList<KeyValue<TKey, TValue>>[DefaultCapacity];
            this.Count = 0;
        }

        public IEnumerable<TKey> Keys
        { 
            get
            {
                return this.Select(kvp => kvp.Key);
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                var result = new List<TValue>();

                foreach (var kvp in this)
                {
                    result.Add(kvp.Value);
                }

                return result;
            }
        }

        public IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
        {
            foreach (var slot in this.slots)
            {
                if (slot != null)
                {
                    foreach (var kvp in slot)
                    {
                        yield return kvp;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator(); 

        // Helpers
        private void GrowIfNeeded()
        {
            if ((float)(this.Count + 1) / this.Capacity
                >= LoadFactor)
            {
                var newHashTable = new HashTable<TKey, TValue>(this.Capacity * 2);

                foreach (var element in this)
                {
                    newHashTable.Add(element.Key, element.Value);
                }

                this.slots = newHashTable.slots;
                this.Count = newHashTable.Count;
            }
        }
    }
}
