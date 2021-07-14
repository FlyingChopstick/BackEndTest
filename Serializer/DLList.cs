using System;
using Serializer.Model;

namespace Serializer
{
    /// <summary>
    /// Linked List wrapper used for tests
    /// </summary>
    public class DLList
    {
        public ListNode Head { get; private set; } = null;
        public bool IsEmpty { get; private set; } = true;


        /// <summary>
        /// Add new item to the list
        /// </summary>
        /// <param name="data"></param>
        public void Add(string data)
        {
            if (IsEmpty)
            {
                Head = new()
                {
                    Previous = null,
                    Next = null,
                    Random = null,
                    Data = data
                };

                IsEmpty = false;
                return;
            }


            ListNode lastNode = Head;
            ListNode previous = lastNode;

            while (lastNode.Next is not null)
            {
                lastNode = lastNode.Next;
                previous = lastNode;
            }


            lastNode.Next = new()
            {
                Previous = previous,
                Next = null,
                Random = null,
                Data = data,
            };
        }
        /// <summary>
        /// Add another list member as random for the target
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="referencing">This node will be added as Random</param>
        public void SetReference(int target, int referencing)
        {
            ListNode node = Head;
            for (int i = 0; i < target; i++)
            {
                node = node.Next;
            }

            ListNode reference = Head;
            for (int i = 0; i < referencing; i++)
            {
                reference = reference.Next;
            }

            node.Random = reference;
        }

        public static DLList NewFilledList(int listSize = 100)
        {
            DLList list = new();

            for (int i = 0; i < listSize; i++)
            {
                list.Add($"Item #{i}");
            }

            list.SetReference(3, 1);
            list.SetReference(5, 7);
            list.SetReference(4, 2);


            for (int i = 0; i < listSize; i++)
            {
                list.SetReference(i, GetRandomIndex(i, listSize));
            }

            return list;
        }
        private static int GetRandomIndex(int requesterIndex, int maxIndex)
        {
            Random rnd = new(DateTime.Now.Second);
            int refIndex = rnd.Next(0, maxIndex);

            while (refIndex == requesterIndex)
            {
                refIndex = rnd.Next();
            }

            return refIndex;
        }
    }
}
