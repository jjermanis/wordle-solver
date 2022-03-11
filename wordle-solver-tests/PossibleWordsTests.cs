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
    }
}