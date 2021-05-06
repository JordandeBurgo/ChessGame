using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollegeProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace CollegeProject.Tests
{
    [TestClass()]
    public class KnightTests
    {
        [TestMethod()]
        public void getMovesTestKnight()
        {
            /*By putting the knight on a specific panel, I have looked at 
            what moves the knight should have on this panel, and made sure
            this is the same as what the function getMoves() returns*/
            BoardGen board = new BoardGen();
            Panel[,] gen = board.GenerateBoard();
            Piece TestPiece = new Knight("WKnight", gen[3, 4], true);
            TestPiece.setMoves(board, false, false);
            List<Panel> DesiredPossibleMoves = new List<Panel> { gen[1, 3], gen[1, 5],
                gen[2, 2], gen[4, 2], gen[5, 3], gen[5, 5], gen[3, 4]};

            if (TestPiece.getMoves().Except(DesiredPossibleMoves).ToList().Count() != 0)
            {
                Assert.Fail();
            }
        }
    }
}