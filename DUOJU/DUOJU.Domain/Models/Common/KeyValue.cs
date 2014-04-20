namespace DUOJU.Domain.Models.Common
{
    public class KeyValue<KT, VT>
    {
        public KeyValue() { }

        public KeyValue(KT key, VT value)
        {
            Key = key;
            Value = value;
        }

        public KT Key { get; set; }

        public VT Value { get; set; }
    }
}
