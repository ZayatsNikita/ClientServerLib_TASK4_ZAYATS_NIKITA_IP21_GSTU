using NUnit.Framework;
using ClientServerLib;

namespace ServersTest
{
    [TestFixture]
    class MessageConverterTest
    {
        [TestCase("��������������������������������", "AbVgDeEzjZiJkLmNoPrStUfKxcChshShchYEayuYa")]
        [TestCase("�����Ÿ��������������������������", "aBvGdEeZjzIjKlMnOpRsTuFkxCchShshchyeaYuya")]
        [TestCase("�� �����. ����������� �� �����, i'll be back", "Vse ponyal. Ostovajtes na svyazi, i'll be back")]
        public void ConvertRussianToTranslitTest_StringWithRussianLetters_ConvertedString(string data, string expected)
        {
            string actual = MessageConverter.ConvertRussianToTranslit(data);
            Assert.AreEqual(actual, expected);
        }
    }
}