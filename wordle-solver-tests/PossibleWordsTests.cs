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
            Assert.IsTrue(WordleUtil.IsWordValid("arose", "YXXGG", "lapse"));
        }

        [TestMethod]
        public void IsWordValid_Tutor_Meter()
        {
            Assert.IsTrue(WordleUtil.IsWordValid("tutor", "XXGXG", "meter"));
        }

        [TestMethod]
        public void CalcResult_Speed_Abide()
        {
            Assert.AreEqual("XXYXY", WordleUtil.CalcResult("speed", "abide"));
        }

        [TestMethod]
        public void CalcResult_Speed_Erase()
        {
            Assert.AreEqual("YXYYX", WordleUtil.CalcResult("speed", "erase"));
        }

        [TestMethod]
        public void CalcResult_Speed_Steal()
        {
            Assert.AreEqual("GXGXX", WordleUtil.CalcResult("speed", "steal"));
        }

        [TestMethod]
        public void CalcResult_Speed_Crepe()
        {
            Assert.AreEqual("XYGYX", WordleUtil.CalcResult("speed", "crepe"));
        }
    }
}