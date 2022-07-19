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

    public class SyncDataFormatter<TTypeResolver> : Formatter<TTypeResolver, global::MyData.SyncData>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::MyData.SyncData value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (1 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, global::System.Guid>(ref bytes, startOffset, offset, 0, value.Key);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 1, value.TypeAsString);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 1);
            }
        }

        public override global::MyData.SyncData Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new SyncDataObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class SyncDataObjectSegment<TTypeResolver> : global::MyData.SyncData, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 16, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _TypeAsString;

        // 0
        public override global::System.Guid Key
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, global::System.Guid>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, global::System.Guid>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 1
        public override string TypeAsString
        {
            get
            {
                return _TypeAsString.Value;
            }
            set
            {
                _TypeAsString.Value = value;
            }
        }


        public SyncDataObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 1, __elementSizes);

            _TypeAsString = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 1, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (1 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, global::System.Guid>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 1, ref _TypeAsString);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 1);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612