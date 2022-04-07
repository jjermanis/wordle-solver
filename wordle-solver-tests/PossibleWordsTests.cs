using Microsoft.VisualStudio.TestTools.UnitTesting;
using wordle_solver;

namespace wordle_solver_tests
{
    [TestClass]
    public class PossibleWordsTests
    {
        [TestMethod]
        public void IsWordValid_Arose_Lapse()
        {
            Assert.IsTrue(PossibleWords.IsWordValid("arose", "YXXGG", "lapse"));
        }

        [TestMethod]
        public void IsWordValid_Tutor_Meter()
        {
            Assert.IsTrue(PossibleWords.IsWordValid("tutor", "XXGXG", "meter"));
        }

        [TestMethod]
        public void CalcResult_Speed_Abide()
        {
            Assert.AreEqual("XXYXY", PossibleWords.CalcResult("speed", "abide"));
        }

        [TestMethod]
        public void CalcResult_Speed_Erase()
        {
            Assert.AreEqual("YXYYX", PossibleWords.CalcResult("speed", "erase"));
        }

        [TestMethod]
        public void CalcResult_Speed_Steal()
        {
            Assert.AreEqual("GXGXX", PossibleWords.CalcResult("speed", "steal"));
        }

        [TestMethod]
        public void CalcResult_Speed_Crepe()
        {
            Assert.AreEqual("XYGYX", PossibleWords.CalcResult("speed", "crepe"));
        }

    }
}