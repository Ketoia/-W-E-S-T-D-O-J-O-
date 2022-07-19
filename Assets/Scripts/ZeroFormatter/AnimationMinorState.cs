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


    public class AnimationMinorStateFormatter<TTypeResolver> : Formatter<TTypeResolver, global::AnimationMinorState>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 4;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::AnimationMinorState value)
        {
            return BinaryUtil.WriteInt32(ref bytes, offset, (Int32)value);
        }

        public override global::AnimationMinorState Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 4;
            return (global::AnimationMinorState)BinaryUtil.ReadInt32(ref bytes, offset);
        }
    }


    public class NullableAnimationMinorStateFormatter<TTypeResolver> : Formatter<TTypeResolver, global::AnimationMinorState?>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 5;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::AnimationMinorState? value)
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

        public override global::AnimationMinorState? Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 5;
            var hasValue = BinaryUtil.ReadBoolean(ref bytes, offset);
            if (!hasValue) return null;

            return (global::AnimationMinorState)BinaryUtil.ReadInt32(ref bytes, offset + 1);
        }
    }



    public class AnimationMinorStateEqualityComparer : IEqualityComparer<global::AnimationMinorState>
    {
        public bool Equals(global::AnimationMinorState x, global::AnimationMinorState y)
        {
            return (Int32)x == (Int32)y;
        }

        public int GetHashCode(global::AnimationMinorState x)
        {
            return (int)x;
        }
    }



}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612