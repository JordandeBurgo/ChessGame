using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollegeProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollegeProject.Tests
{
    [TestClass()]
    public class PawnTests
    {
        [TestMethod()]
        public void getMovesTestPawn()
        {
            /*By putting the pawn on a specific panel, I have looked at 
            what moves the pawn should have on this panel, and made sure
            this is the same as what the function getMoves() returns*/
            BoardGen board = new BoardGen();
            Panel[,] gen = board.GenerateBoard();
            Piece TestPiece = new Pawn("WPawn", gen[3, 4], true);
            TestPiece.setMoves(board, false, false);
            List<Panel> DesiredPossibleMoves = new List<Panel> { gen[3, 3], gen[3, 4]};

            if (TestPiece.getMoves().Except(DesiredPossibleMoves).ToList().Count() != 0)
            {

                Assert.Fail();
            }
        }
    }
}