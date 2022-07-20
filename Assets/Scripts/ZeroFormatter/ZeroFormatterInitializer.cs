#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;
    using global::ZeroFormatter.Comparers;

    public static partial class ZeroFormatterInitializer
    {
        static bool registered = false;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Register()
        {
            if(registered) return;
            registered = true;
            // Enums
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::AnimationMainState>.Register(new ZeroFormatter.DynamicObjectSegments.AnimationMainStateFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::AnimationMainState>.Register(new ZeroFormatter.DynamicObjectSegments.AnimationMainStateEqualityComparer());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::AnimationMainState?>.Register(new ZeroFormatter.DynamicObjectSegments.NullableAnimationMainStateFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::AnimationMainState?>.Register(new NullableEqualityComparer<global::AnimationMainState>());
            
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::AnimationMinorState>.Register(new ZeroFormatter.DynamicObjectSegments.AnimationMinorStateFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::AnimationMinorState>.Register(new ZeroFormatter.DynamicObjectSegments.AnimationMinorStateEqualityComparer());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::AnimationMinorState?>.Register(new ZeroFormatter.DynamicObjectSegments.NullableAnimationMinorStateFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::AnimationMinorState?>.Register(new NullableEqualityComparer<global::AnimationMinorState>());
            
            // Objects
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncData>.Register(new MyData.SyncDataFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncDataString>.Register(new MyData.SyncDataStringFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncDataChar>.Register(new MyData.SyncDataCharFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncDataInt>.Register(new MyData.SyncDataIntFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncDataFloat>.Register(new MyData.SyncDataFloatFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncDataBool>.Register(new MyData.SyncDataBoolFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncDataVector3>.Register(new MyData.SyncDataVector3Formatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncDataVector3Int>.Register(new MyData.SyncDataVector3IntFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.SyncDataPlayerData>.Register(new MyData.SyncDataPlayerDataFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.MyTransportObject>.Register(new MyData.MyTransportObjectFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.ComponentsDataList>.Register(new MyData.ComponentsDataListFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            // Structs
            {
                var structFormatter = new MyData.Vector3Formatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.Vector3>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.Vector3?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.Vector3>(structFormatter));
            }
            {
                var structFormatter = new MyData.Vector3IntFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.Vector3Int>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.Vector3Int?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.Vector3Int>(structFormatter));
            }
            {
                var structFormatter = new MyData.ComponentsDataFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.ComponentsData>.Register(structFormatter);
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.ComponentsData?>.Register(new global::ZeroFormatter.Formatters.NullableStructFormatter<ZeroFormatter.Formatters.DefaultResolver, global::MyData.ComponentsData>(structFormatter));
            }
            // Unions
            // Generics
            ZeroFormatter.Formatters.Formatter.RegisterCollection<ZeroFormatter.Formatters.DefaultResolver, global::MyData.ComponentsData, global::System.Collections.Generic.List<global::MyData.ComponentsData>>();
            ZeroFormatter.Formatters.Formatter.RegisterCollection<ZeroFormatter.Formatters.DefaultResolver, global::System.Guid, global::System.Collections.Generic.List<global::System.Guid>>();
            ZeroFormatter.Formatters.Formatter.RegisterCollection<ZeroFormatter.Formatters.DefaultResolver, string, global::System.Collections.Generic.List<string>>();
        }
    }
}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612