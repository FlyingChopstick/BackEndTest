using System;
using System.IO;
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


            string filename = DateTime.Now.ToString();
            using Stream writeStream = File.OpenWrite(filename);
            ls.Serialize(list.Head, writeStream);


            Assert.True(File.Exists(filename));
            Assert.True(File.ReadAllText(filename) != string.Empty);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Deserialize_ShouldReturnWorkingHead(int listSize)
        {
            DLList list = DLList.NewFilledList(listSize);
            ListSerializer ls = new();


            string filename = DateTime.Now.ToString();
            using (Stream writeStream = File.OpenWrite(filename))
            {
                ls.Serialize(list.Head, writeStream);
            }


            ListNode newHead;
            using (Stream readStream = File.OpenRead(filename))
            {
                newHead = ls.Deserialize(readStream);
            }

            Assert.Equal(list.Head.Data, newHead.Data);
            Assert.Equal(list.Head.Next, newHead.Next);
            Assert.Equal(list.Head.Previous, newHead.Previous);
            Assert.Equal(list.Head.Random, newHead.Random);

            ListNode node = newHead;
            for (int i = 0; i < listSize; i++)
            {
                node = node.Next;
            }
        }
    }
}
