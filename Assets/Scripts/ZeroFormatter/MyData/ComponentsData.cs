#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace MyData
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class ComponentsDataFormatter<TTypeResolver> : Formatter<TTypeResolver, global::MyData.ComponentsData>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly Formatter<TTypeResolver, global::System.Guid> formatter0;
        readonly Formatter<TTypeResolver, string> formatter1;
        readonly Formatter<TTypeResolver, global::MyData.MyTransportObject> formatter2;
        
        public override bool NoUseDirtyTracker
        {
            get
            {
                return formatter0.NoUseDirtyTracker
                    && formatter1.NoUseDirtyTracker
                    && formatter2.NoUseDirtyTracker
                ;
            }
        }

        public ComponentsDataFormatter()
        {
            formatter0 = Formatter<TTypeResolver, global::System.Guid>.Default;
            formatter1 = Formatter<TTypeResolver, string>.Default;
            formatter2 = Formatter<TTypeResolver, global::MyData.MyTransportObject>.Default;
            
        }

        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::MyData.ComponentsData value)
        {
            var startOffset = offset;
            offset += formatter0.Serialize(ref bytes, offset, value.ComponentID);
            offset += formatter1.Serialize(ref bytes, offset, value.TypeAsString);
            offset += formatter2.Serialize(ref bytes, offset, value.MyTransportObjects);
            return offset - startOffset;
        }

        public override global::MyData.ComponentsData Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 0;
            int size;
            var item0 = formatter0.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item1 = formatter1.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            var item2 = formatter2.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;
            byteSize += size;
            
            return new global::MyData.ComponentsData(item0, item1, item2);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612