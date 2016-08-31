using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DanielCook.Core.Extensions
{
    public static class SerializableExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            return Disposable.Using(() => new MemoryStream(),
                (ms) =>
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    ms.Position = 0;

                    return (T)formatter.Deserialize(ms);
                });
        }
    }
}
