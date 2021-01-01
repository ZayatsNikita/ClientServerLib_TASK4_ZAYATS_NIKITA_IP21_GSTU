using NUnit.Framework;
using ClientServerLib;

namespace ServersTest
{
    [TestFixture]
    class MessageConverterTest
    {
        [TestCase("јб¬гƒе®ж«и…кЋмЌоѕр—т”ф’ц„шўъџьЁюя", "AbVgDeEzjZiJkLmNoPrStUfKxcChshShchYEayuYa")]
        [TestCase("аЅв√д≈Є∆з»й лћнќп–с“у‘х÷чЎщЏыьэё€", "aBvGdEeZjzIjKlMnOpRsTuFkxCchShshchyeaYuya")]
        [TestCase("¬сЄ пон€л. ќстовайтесь на св€зи, i'll be back", "Vse ponyal. Ostovajtes na svyazi, i'll be back")]
        public void ConvertRussianToTranslitTest_StringWithRussianLetters_ConvertedString(string data, string expected)
        {
            string actual = MessageConverter.ConvertRussianToTranslit(data);
            Assert.AreEqual(actual, expected);
        }
    }
}