using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollegeProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace CollegeProject.Tests
{

    [TestClass()]
    public class PieceTests
    {
        Panel testPanel = new Panel();

        [TestMethod()]
        public void checkTypeTest()
        {
            /*By moving the white bishops to a panel where it would
              put the black king in check (and clearing the rest of
              the board, this procedure ensures the correct type of check
              is recognised*/
            BoardGen board = new BoardGen();
            Panel[,] gen = board.GenerateBoard();
            foreach (Piece p in board.getPieces().ToList())
            {
                if (!p.getType().Contains("King") && !(p.getType() == "WBishop"))
                {
                    board.RemovePiece(p);
                }
            }
            board.setPanelsInUse();

            foreach (Piece p in board.getPieces())
            {
                if (p.getType() == "WBishop")
                {
                    p.setPanel(gen[0, 4]);
                    p.setMoves(board, false, false);
                    if ((p.checkCheck(board)))
                    {
                        if(p.checkType() != "B")
                        {
                            Assert.Fail();
                        }
                    }
                }
            }
        }

        [TestMethod()]
        public void getTypeTest()
        {
            //This tests that the getType() functions returns
            //the actual piece type
            Piece testPiece = new Pawn("BPawn", testPanel, true);
            if(testPiece.getType() != "BPawn")
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void getPanelTest()
        {
            //This procedure ensures that getPanel() returns the 
            //panel the piece is at
            Piece testPiece = new Pawn("BPawn", testPanel, true);
            if(testPiece.getPanel() != testPanel)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void checkCheckTest()
        {
          /*By moving the white bishops to a panel where it would
            put the black king in check (and clearing the rest of
            the board, this procedure ensures check is recognised*/
            BoardGen board = new BoardGen();
            Panel[,] gen = board.GenerateBoard();
            foreach (Piece p in board.getPieces().ToList())
            {
                if(!p.getType().Contains("King") && !(p.getType() == "WBishop"))
                {
                    board.RemovePiece(p);
                }
            }
            board.setPanelsInUse();
            
            foreach (Piece p in board.getPieces())
            {
                if (p.getType() == "WBishop")
                {
                    p.setPanel(gen[0, 4]);
                    p.setMoves(board, false, false);
                    if (!(p.checkCheck(board)))
                    {
                        Assert.Fail();
                    }
                }
            } 
        }
    }
}