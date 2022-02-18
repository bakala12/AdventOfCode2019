namespace Day24
{
    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : notnull
        where TValue : new()
    {
        public new TValue this[TKey key]
        {
            get
            {
                if(TryGetValue(key, out TValue? val) && val != null)
                    return val;
                var v = new TValue();
                base[key] = v;
                return v;
            }
            set
            {
                base[key] = value;
            }
        }
    }
}