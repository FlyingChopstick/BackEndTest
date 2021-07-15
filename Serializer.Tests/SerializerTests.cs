using System;
using System.IO;
using Serializer.Model;
using Xunit;

namespace Serializer.Tests
{
    public class SerializerTests
    {
        [Fact]
        public void Serialize_ShouldWriteToFile()
        {
            DLList list = DLList.NewFilledList();
            ListSerializer ls = new();

            Random rnd = new(DateTime.Now.Second);
            string filename = rnd.Next().ToString();
            using (Stream writeStream = File.OpenWrite(filename))
            {
                ls.Serialize(list.Head, writeStream);
            }


            Assert.True(File.Exists(filename));
            Assert.True(File.ReadAllText(filename) != string.Empty);
        }


        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Deserialize_ShouldReturnFullList(int listSize)
        {
            DLList list = DLList.NewFilledList(listSize);
            ListSerializer ls = new();


            Random rnd = new(DateTime.Now.Second);
            string filename = rnd.Next().ToString();
            using (Stream writeStream = File.OpenWrite(filename))
            {
                ls.Serialize(list.Head, writeStream);
            }


            ListNode newHead;
            using (Stream readStream = File.OpenRead(filename))
            {
                newHead = ls.Deserialize(readStream);
            }
            // check head data
            Assert.Equal(list.Head.Data, newHead.Data);

            // go through the whole list
            ListNode node = newHead;
            for (int i = 0; i < listSize - 1; i++)
            {
                //check data
                Assert.Equal(node.Data, newHead.Data);

                //check Random data
                if (node.Random is not null)
                {
                    Assert.Equal(node.Random.Data, newHead.Random.Data);
                }

                node = node.Next;
                newHead = newHead.Next;
            }
            //check that the last node has no followers
            Assert.Null(node.Next);

            //go backwards
            for (int i = 0; i < listSize - 1; i++)
            {
                //check data
                Assert.Equal(node.Data, newHead.Data);

                node = node.Previous;
                newHead = newHead.Previous;
            }
            //check that the head has no ancestors
            Assert.Null(node.Previous);
        }
    }
}
