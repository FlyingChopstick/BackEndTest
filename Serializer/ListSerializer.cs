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

            var nodes = SaveNodes(head);
            bf.Serialize(s, nodes);
        }

        private List<SavedNode> SaveNodes(ListNode head)
        {
            var currentNode = head;
            List<string> data = new();
            List<string> random = new();
            List<int> rndIndices = new();

            while (currentNode is not null)
            {
                data.Add(currentNode.Data);
                random.Add(currentNode.Random?.Data);

                currentNode = currentNode.Next;
            }

            for (int i = 0; i < random.Count; i++)
            {
                rndIndices.Add(FindRandomIndex(random[i], data));
            }

            List<SavedNode> savedNodes = new();
            for (int i = 0; i < data.Count; i++)
            {
                savedNodes.Add(new()
                {
                    Data = data[i],
                    IndexRandom = rndIndices[i]
                });
            }

            return savedNodes;
        }
        private ListNode RestoreNodes(List<SavedNode> savedNodes)
        {
            List<ListNode> nodes = new();
            ListNode head = new()
            {
                Previous = null,
                Next = null,
                Data = savedNodes[0].Data,
                Random = null
            };
            nodes.Add(head);
            var previous = head;

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

            return head;
        }
        private int FindRandomIndex(string random, List<string> data)
        {
            if (random is null)
            {
                return -1;
            }
            for (int i = 0; i < data.Count; i++)
            {
                if (random == data[i])
                {
                    return i;
                }
            }

            throw new ArgumentException();
        }

    }
}
