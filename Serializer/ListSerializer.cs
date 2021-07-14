using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Serializer.Model;

namespace Serializer
{
    /// <summary>
    /// Container to store node info for serialization
    /// </summary>
    [Serializable]
    public struct SavedNode
    {
        /// <summary>
        /// Index of an associated random node. == -1 if no Random node is associated.
        /// </summary>
        public int IndexRandom { get; init; }
        /// <summary>
        /// Node data
        /// </summary>
        public string Data { get; init; }
    }

    public class ListSerializer : IListSerializer
    {
        public ListNode Deserialize(Stream s)
        {
            BinaryFormatter bf = new();

            try
            {
                return RestoreNodes((List<SavedNode>)bf.Deserialize(s));
            }
            catch (Exception)
            {
                throw new ArgumentException("Could not deserialize the stream.");
            }
        }
        public void Serialize(ListNode head, Stream s)
        {
            BinaryFormatter bf = new();

            bf.Serialize(s, SaveNodes(head));
        }



        private readonly List<string> _nodeData = new();
        private readonly List<string> _randomData = new();
        private readonly List<int> _randomIndices = new();

        /// <summary>
        /// Converts nodes into serializeable list, storing their order and Random topography
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        private List<SavedNode> SaveNodes(ListNode head)
        {
            //List<string> _randomData = new();
            //List<int> _randomIndices = new();

            var currentNode = head;
            _randomData.Clear();
            _randomIndices.Clear();
            _nodeData.Clear();

            while (currentNode is not null)
            {
                _nodeData.Add(currentNode.Data);
                _randomData.Add(currentNode.Random?.Data);

                currentNode = currentNode.Next;
            }

            List<SavedNode> savedNodes = new();
            for (int i = 0; i < _randomData.Count; i++)
            {
                _randomIndices.Add(FindRandomIndex(_randomData[i]));
                savedNodes.Add(new()
                {
                    Data = _nodeData[i],
                    IndexRandom = _randomIndices[i]
                });
            }


            return savedNodes;
        }
        private ListNode RestoreNodes(List<SavedNode> savedNodes)
        {
            List<ListNode> nodes = new();
            nodes.Add(new()
            {
                Previous = null,
                Next = null,
                Data = savedNodes[0].Data,
                Random = null
            });
            var previous = nodes[0];

            for (int i = 1; i < savedNodes.Count; i++)
            {
                ListNode newNode = new()
                {
                    Next = null,
                    Previous = previous,
                    Random = null,
                    Data = savedNodes[i].Data,
                };

                nodes.Add(newNode);
                previous.Next = newNode;
                previous = newNode;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                int index = savedNodes[i].IndexRandom;
                if (index >= 0)
                {
                    nodes[i].Random = nodes[index];
                }
            }

            return nodes[0];
        }
        /// <summary>
        /// In the list of nodes, find one with the query data
        /// </summary>
        /// <param name="random">Query</param>
        /// <returns>Index of a node or -1 if the query is null</returns>
        private int FindRandomIndex(string random)
        {
            if (random is null)
            {
                return -1;
            }
            for (int i = 0; i < _nodeData.Count; i++)
            {
                if (random == _nodeData[i])
                {
                    return i;
                }
            }

            throw new ArgumentException($"The required Random node does not exist. Node data was: {random}");
        }
    }
}
