using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CollegeProject.Tests
{
    [TestClass()]
    public class BoardGenTests
    {
        BoardGen gen = new BoardGen();

        [TestMethod()]
        public void TestBoardSize()
        {
            //This procedure ensures the board is an 8 by 8 grid
            Panel[,] board = gen.GenerateBoard();
            if (!(board.GetLength(0) == 8 && board.GetLength(1) == 8))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void TestNumberOfPieces()
        {
            /*This procedure ensures there are 32
              pieces on the board to begin with*/
            Panel[,] board = gen.GenerateBoard();
            if (gen.getPieces().Count != 32)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void TestPiecesOnBoard()
        {
            //This ensures all pieces are of a valid type
            Panel[,] board = gen.GenerateBoard();
            String[] pieces = { "BPawn", "WPawn", "BRook", "WRook", "BKnight", "WKnight",
                "BBishop", "WBishop", "BQueen", "WQueen", "BKing", "WKing" };
            foreach (Piece p in gen.getPieces())
            {
                if (!(pieces.Contains(p.getType())))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public void TestPiecesLocation()
        {
            //This ensures that pieces are in the
            //correct relative positions
            Panel[,] board = gen.GenerateBoard();
            foreach (Piece p in gen.getPieces())
            {
                if (p.getType().Contains("Pawn") &&
                    (p.getPanel().Location.Y != 40) &&
                    (p.getPanel().Location.Y != 240))
                {
                    Assert.Fail();
                }
                if (p.getType().Contains("Rook") &&
                    (p.getPanel().Location.Y != 0) &&
                    (p.getPanel().Location.Y != 280) &&
                    (p.getPanel().Location.X != 0) &&
                    (p.getPanel().Location.X != 280))
                {
                    Assert.Fail();
                }
                if (p.getType().Contains("Knight") &&
                    (p.getPanel().Location.Y != 0) &&
                    (p.getPanel().Location.Y != 280) &&
                    (p.getPanel().Location.X != 40) &&
                    (p.getPanel().Location.X != 240))
                {
                    Assert.Fail();
                }
                if (p.getType().Contains("Bishop") &&
                    (p.getPanel().Location.Y != 0) &&
                    (p.getPanel().Location.Y != 280) &&
                    (p.getPanel().Location.X != 80) &&
                    (p.getPanel().Location.X != 200))
                {
                    Assert.Fail();
                }
                if (p.getType().Contains("Queen") &&
                    (p.getPanel().Location.Y != 0) &&
                    (p.getPanel().Location.Y != 280) &&
                    (p.getPanel().Location.X != 120))
                {
                    Assert.Fail();
                }
                if (p.getType().Contains("King") &&
                    (p.getPanel().Location.Y != 0) &&
                    (p.getPanel().Location.Y != 280) &&
                    (p.getPanel().Location.X != 160))
                {
                    Assert.Fail();
                }
            }
        }
    }
}