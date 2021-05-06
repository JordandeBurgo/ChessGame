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
    public class PieceTests
    {
        [TestMethod()]
        public void getValueTest()
        {
            BoardGen board = new BoardGen();
            Panel[,] gen = board.GenerateBoard();
            Piece TestPiece = new Bishop("WBishop", gen[3, 4], true);
            if(TestPiece.getValue(board) != 31)
            {
                Assert.Fail();
            }
            
        }
    }
}