using Moq;
using ModelNineMenMorris.Model;
using ModelNineMenMorris.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace TestNMM_Model
{
    [TestClass]
    public class TestNMM
    {
        private Table table_ = null!;
        private Mock<DataAccess> _mock = null!;
        [TestInitialize]
        public void InitDocuStatTest()
        {
            _mock = new Mock<DataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(table_.board));
            table_ = new Table(_mock.Object);
            table_.board.FromTo_ += new EventHandler<NMMFromToEventArgs>(From_To);
            //table_.board.End_ += new EventHandler<NMMGameOverEventArgs>(Game_Over);
        }
        [TestMethod]
        public void TestConstruct()
        {
            bool allTransparent = true;
            for(int i = 0; i < table_.board.FieldsColor.Length; i++)
            {
                if(table_.board.FieldsColor[i] != ColorFields.TRANSP)
                {
                    allTransparent = false;
                }
            }
            Assert.IsTrue(allTransparent);
        }
        [TestMethod]
        public void TestPlaceOne()
        {
            table_.board.move(7);
            Assert.AreEqual(ColorFields.BLUE, table_.board.FieldsColor[7]);
        }
        [TestMethod]
        public void TestTurns()
        {
            table_.board.move(10);
            Assert.AreEqual(17, table_.board.turnsValue);
        }
        [TestMethod]
        public void TestSwitchPlayer()
        {
            table_.board.move(11);
            table_.board.move(19);
            Assert.AreEqual(ColorFields.GREEN, table_.board.FieldsColor[19]);
        }
        [TestMethod]
        public void TestCannotPlace()
        {
            table_.board.move(11);
            table_.board.move(19);
            table_.board.move(19);
            Assert.AreEqual(ColorFields.GREEN, table_.board.FieldsColor[19]);
        }
        [TestMethod]
        public void TestRemovePhase()
        {
            table_.board.move(0);
            table_.board.move(5);
            table_.board.move(9);
            table_.board.move(8);
            table_.board.move(21);
            Assert.IsTrue(table_.board.removeValue);
        }
        [TestMethod]
        public void TestRemove()
        {
            table_.board.move(0);
            table_.board.move(5);
            table_.board.move(1);
            table_.board.move(8);
            table_.board.move(2);
            table_.board.move(5);
            Assert.AreEqual(ColorFields.TRANSP, table_.board.FieldsColor[5]);
        }
        [TestMethod]
        public void TestRemoveDecrease()
        {
            table_.board.move(0);
            table_.board.move(5);
            table_.board.move(1);
            table_.board.move(8);
            table_.board.move(2);
            table_.board.move(5);
            Assert.AreEqual(8, table_.board.green0Value);
        }
        [TestMethod]
        public void TestRemovingIfAllNextToEachOther()
        {
            table_.board.move(0); //blue
            table_.board.move(2); //green
            table_.board.move(9); //blue
            table_.board.move(14); //green
            table_.board.move(21); //blue
            table_.board.move(14); //remove
            table_.board.move(14); //green
            table_.board.move(8); //blue
            table_.board.move(16); //green
            table_.board.move(12); //blue
            table_.board.move(19); //green
            table_.board.move(17); //blue
            table_.board.move(19); //remove
            table_.board.move(23); //green
            table_.board.move(9); //remove
            Assert.AreEqual(ColorFields.TRANSP, table_.board.FieldsColor[9]);
        }
        public void Phase1()
        {
            table_.board.move(0);
            table_.board.move(1);
            table_.board.move(2);
            table_.board.move(3);
            table_.board.move(4);
            table_.board.move(5);
            table_.board.move(6);
            table_.board.move(7);
            table_.board.move(8);
            table_.board.move(9);
            table_.board.move(10);
            table_.board.move(11);
            table_.board.move(12);
            table_.board.move(13);
            table_.board.move(14);
            table_.board.move(15);
            table_.board.move(16);
            table_.board.move(17);
        }
        public void Phase2()
        {
            Phase1();
            table_.board.move(10); //blue
            table_.board.move(18); //blue
            table_.board.move(3);  //green
            table_.board.move(10); //green
            table_.board.move(4); //remove
            table_.board.move(16); //blue
            table_.board.move(19); //blue
            table_.board.move(5); //green
            table_.board.move(4); //green
            table_.board.move(6); //remove
            table_.board.move(19); //blue
            table_.board.move(16); //blue
            table_.board.move(7); //green
            table_.board.move(6); //green
            table_.board.move(0); //remove
            table_.board.move(16); //blue
            table_.board.move(19); //blue
            table_.board.move(6); //green
            table_.board.move(7); //green
            table_.board.move(8); //remove
            table_.board.move(19); //blue
            table_.board.move(16); //blue
            table_.board.move(7); //green
            table_.board.move(6); //green
            table_.board.move(12); //remove
            table_.board.move(16); //blue
            table_.board.move(19); //blue
            table_.board.move(6); //green
            table_.board.move(7); //green
            table_.board.move(2); //remove
        }
        [TestMethod]
        public void TestSecondPhaseChoosing()
        {
            Phase1();
            table_.board.move(16);
            Assert.AreEqual(Move.PLACE, table_.board.movesValue);
        }
        [TestMethod]
        public void TestSecondPhaseMoving()
        {
            Phase1();
            table_.board.move(16);
            table_.board.move(19);
            Assert.AreEqual(ColorFields.TRANSP, table_.board.FieldsColor[16]);
            Assert.AreEqual(ColorFields.BLUE, table_.board.FieldsColor[19]);
        }
        [TestMethod]
        public void TestSecondPhaseCannotMoveToTransparent()
        {
            Phase1();
            table_.board.move(16);
            table_.board.move(23);
            Assert.AreEqual(ColorFields.BLUE, table_.board.FieldsColor[16]);
            Assert.AreEqual(ColorFields.TRANSP, table_.board.FieldsColor[23]);
        }
        [TestMethod]
        public void TestSecondPhaseCannotMoveToOwnField()
        {
            Phase1();
            table_.board.move(16);
            table_.board.move(0);
            Assert.AreEqual(ColorFields.BLUE, table_.board.FieldsColor[16]);
            Assert.AreEqual(ColorFields.BLUE, table_.board.FieldsColor[0]);
        }
        [TestMethod]
        public void TestSecondPhaseCannotMoveToOtherPlayersField()
        {
            Phase1();
            table_.board.move(16);
            table_.board.move(13);
            Assert.AreEqual(ColorFields.BLUE, table_.board.FieldsColor[16]);
            Assert.AreEqual(ColorFields.GREEN, table_.board.FieldsColor[13]);
        }
        [TestMethod]
        public void TestHaveToPass()
        {
            table_.board.move(0);
            table_.board.move(2);
            table_.board.move(1);
            table_.board.move(4);
            table_.board.move(3);
            table_.board.move(10);
            table_.board.move(9);
            table_.board.move(21);
            table_.board.move(17);
            table_.board.move(8);
            table_.board.move(12);
            table_.board.move(15);
            table_.board.move(16);
            table_.board.move(19);
            table_.board.move(20);
            table_.board.move(13);
            table_.board.move(5);
            table_.board.move(6);
            Assert.AreEqual(Turns.GREEN, table_.board.player_Value);
        }
        [TestMethod]
        public void TestThirdPhase()
        {
            Phase2();
            table_.board.move(14);
            table_.board.move(20);
            Assert.AreEqual(ColorFields.TRANSP, table_.board.FieldsColor[14]);
            Assert.AreEqual(ColorFields.BLUE, table_.board.FieldsColor[20]);
        }
        [TestMethod]
        public void TestEndGame()
        {
            Phase2();
            table_.board.move(14); //blue
            table_.board.move(20); //blue
            table_.board.move(17); //remove
            table_.board.move(7); //green
            table_.board.move(6); //green
            table_.board.move(19); //remove
            Assert.IsTrue(table_.board.endValue);
        }
        [TestMethod]
        public void TestNewGame()
        {
            Phase2();
            table_.board.newGame();
            bool allTransparent = true;
            for (int i = 0; i < table_.board.FieldsColor.Length; i++)
            {
                if (table_.board.FieldsColor[i] != ColorFields.TRANSP)
                {
                    allTransparent = false;
                }
            }
            Assert.IsTrue(allTransparent);
        }
        [TestMethod]
        public async Task TestLoad()
        {
            Phase1();
            await table_.LoadGameAsync(".txt");
            Assert.AreEqual(ColorFields.GREEN, table_.board.FieldsColor[1]);
            _mock.Verify(dataAccess => dataAccess.LoadAsync(".txt"), Times.Once());
        }
        [TestMethod]
        [ExpectedException(typeof(FileFormatException))]
        public async Task TestLoadWrongFileFormat()
        {
            await table_.LoadGameAsync("");
        }
        [TestMethod]
        public void TestBoardIndex()
        {
            table_.board.move(0);
            table_.board.move(23);
            Assert.AreEqual(ColorFields.BLUE, table_.board.FieldsColor[0]);
            Assert.AreEqual(ColorFields.GREEN, table_.board.FieldsColor[23]);
        }
        [TestMethod]
        [ExpectedException(typeof(IncorrectIndexException))]
        public void TestBoardOutOfIndexing()
        {
            table_.board.move(24);
        }
        [TestMethod]
        [ExpectedException(typeof(IncorrectIndexException))]
        public void TestBoardOutOfIndexing2()
        {
            table_.board.move(-1);
        }
        private void From_To(Object? sender, NMMFromToEventArgs e)
        {
            Assert.AreNotEqual(e.ind, e.transpInd);
        }
    }
}