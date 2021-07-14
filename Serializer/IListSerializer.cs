using System.IO;
using Serializer.Model;

namespace Serializer
{
    /// <summary>
    /// Serializer interface for list based on the ListNode
    /// </summary>
    public interface IListSerializer
    {
        /// <summary>
        /// Serializes all nodes in the list, including topology of the Random links, into stream
        /// </summary>
        void Serialize(ListNode head, Stream s);

        /// <summary>
        /// Deserializes the list from the stream, returns the head node of the list
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when a stream has invalid data</exception>
        ListNode Deserialize(Stream s);
    }
}
