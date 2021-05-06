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
    public class BishopTests
    {

        [TestMethod()]
        public void getMovesTestBishop()
        {
          /*By putting the bishop on a specific panel, I have looked at 
            what moves the bishop should have on this panel, and made sure
            this is the same as what the function getMoves() returns*/
            
            BoardGen board = new BoardGen();
            Panel[,] gen = board.GenerateBoard();
            Piece TestPiece = new Bishop("WBishop", gen[3, 4], true);
            TestPiece.setMoves(board, false, false);
            List<Panel> DesiredPossibleMoves = new List<Panel> {gen[3, 4], gen[4, 5],
                gen[2, 3], gen[1, 2], gen[0, 1], gen[2, 5], gen[4, 3], gen[5, 2],
                gen[6, 1] };
            if (TestPiece.getMoves().Except(DesiredPossibleMoves).ToList().Count() != 0)
            {
                Assert.Fail();
            }
        }
    }
}