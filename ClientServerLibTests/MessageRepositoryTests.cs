using NUnit.Framework;
using System;
using ClientServerLib;
namespace ClientServerLibTests
{
    [TestFixture]
    class MessageRepositoryTests
    {
        [TestCase(1,"1")]
        [TestCase(2,"2","j")]
        [TestCase(1,"f","f","f")]
        public void AddTest_AddingAFewElements_TheNumberOfEntriesIsEqualToTheExpectedCouunt(int expected, params string[] elements)
        {
            MessageRepository messageRepository = new MessageRepository();
            foreach (string element in elements)
            {
                messageRepository.Add(element,element);
            }
            int actual = messageRepository.Count;
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void GetMessageHistoryOfTest_ExsistNode_ListWithData()
        {
            MessageRepository messageRepository = new MessageRepository();
            messageRepository.Add("nikita","1");
            messageRepository.Add("nikita","2");
            messageRepository.Add("nikita","3");

            int actual = messageRepository.GetMessageHistoryOf("nikita").Count;
            int expected = 3;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetMessageHistoryOfTest_NotExsistNode_ListWithData()
        {
            MessageRepository messageRepository = new MessageRepository();
            messageRepository.Add("nikita", "1");
            messageRepository.Add("nikita", "2");
            messageRepository.Add("nikita", "3");
            bool actual = false;
            try
            {
                messageRepository.GetMessageHistoryOf("vadim");
            }
            catch(ArgumentException)
            {
                actual = true;
            }
            Assert.IsTrue(actual);
        }


    }
}
