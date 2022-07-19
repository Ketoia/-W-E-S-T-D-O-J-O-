#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments
{
    using global::System;
    using global::System.Collections.Generic;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;


    public class AnimationMainStateFormatter<TTypeResolver> : Formatter<TTypeResolver, global::AnimationMainState>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::AnimationMainState value)
        {
            return BinaryUtil.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public override global::AnimationMainState Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 4;
            return (global::AnimationMainState)BinaryUtil.ReadInt32(ref bytes, offset);
        }
    }


    public class NullableAnimationMainStateFormatter<TTypeResolver> : Formatter<TTypeResolver, global::AnimationMainState?>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 5;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::AnimationMainState? value)
        {
            BinaryUtil.WriteBoolean(ref bytes, offset, value.HasValue);
            if (value.HasValue)
            {
                BinaryUtil.WriteInt32(ref bytes, offset + 1, (Int32)value.Value);
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, offset + 5);
            }

            return 5;
        }

        public override global::AnimationMainState? Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 5;
            var hasValue = BinaryUtil.ReadBoolean(ref bytes, offset);
            if (!hasValue) return null;

            return (global::AnimationMainState)BinaryUtil.ReadInt32(ref bytes, offset + 1);
        }
    }



    public class AnimationMainStateEqualityComparer : IEqualityComparer<global::AnimationMainState>
    {
        public bool Equals(global::AnimationMainState x, global::AnimationMainState y)
        {
            return (Int32)x == (Int32)y;
        }

        public int GetHashCode(global::AnimationMainState x)
        {
            return (int)x;
        }
    }



}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612