using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Serializer.Model;

namespace Serializer
{
    [Serializable]
    public struct SavedNode
    {
        public int IndexRandom { get; init; }
        public string Data { get; init; }
    }

    public class ListSerializer : IListSerializer
    {
        public ListNode Deserialize(Stream s)
        {
            BinaryFormatter bf = new();

            return RestoreNodes((List<SavedNode>)bf.Deserialize(s));
        }
        public void Serialize(ListNode head, Stream s)
        {
            BinaryFormatter bf = new();

            bf.Serialize(s, SaveNodes(head));
        }



        private readonly List<string> _nodeData = new();
        private List<SavedNode> SaveNodes(ListNode head)
        {
            var currentNode = head;
            //List<string> _nodeData = new();
            List<string> random = new();
            List<int> rndIndices = new();
            _nodeData.Clear();

            while (currentNode is not null)
            {
                _nodeData.Add(currentNode.Data);
                random.Add(currentNode.Random?.Data);

                currentNode = currentNode.Next;
            }

            for (int i = 0; i < random.Count; i++)
            {
                rndIndices.Add(FindRandomIndex(random[i]));
            }

            List<SavedNode> savedNodes = new();
            for (int i = 0; i < _nodeData.Count; i++)
            {
                savedNodes.Add(new()
                {
                    Data = _nodeData[i],
                    IndexRandom = rndIndices[i]
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

            throw new ArgumentException();
        }
    }
}
