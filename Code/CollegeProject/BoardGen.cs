using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CollegeProject
{
  public class BoardGen
  {
    Panel[,] boardPanels;
    List<Piece> pieces = new List<Piece>();
    List<Panel> inUse;
    Pawn pawn;
    Rook rook;
    Knight knight;
    Bishop bishop;
    Queen queen;
    King king;
    MoveCalculator MoveCalc = new MoveCalculator();
    List<Piece> possibleBPieces;
    List<Piece> possibleWPieces;

    public BoardGen()
    {

    }


    public Panel[,] GenerateBoard()
    {

      int tileSize = 40;
      int gridSize = 8;
      //Size of squares
      Color color1 = System.Drawing.ColorTranslator.FromHtml("#80A83E"); //#6
      Color color2 = System.Drawing.ColorTranslator.FromHtml("#D9DD76"); //#c
                                                                         //Colour of sqaures

      boardPanels = new Panel[gridSize, gridSize];

      //Nested for loop to get the 8 by 8 grid set up
      for (int i = 0; i < gridSize; i++)
      {
        for (int x = 0; x < gridSize; x++)
        {

          //Set the panel details
          Panel newPanel = new Panel
          {
            Size = new Size(tileSize, tileSize),
            Location = new Point(tileSize * i, tileSize * x),
            BackgroundImageLayout = ImageLayout.Stretch,
          };

          String peiceType = null;

          //Set what colour the piece should be
          peiceType = (tileSize * x <= 40) ? "B" : "W";

          //Setting what type of pieces go where and geneerating the pieces
          if (tileSize * x == 40 || tileSize * x == 240)
          {
            peiceType += "Pawn";
            pawn = new Pawn(peiceType, newPanel, false);
            pieces.Add(pawn);
          }
          else if ((tileSize * i == 0 || tileSize * i == 280) &&
                  (tileSize * x == 0 || tileSize * x == 280))
          {
            peiceType += "Rook";
            rook = new Rook(peiceType, newPanel, false);
            pieces.Add(rook);
          }
          else if ((tileSize * i == 40 || tileSize * i == 240) &&
                  (tileSize * x == 0 || tileSize * x == 280))
          {
            peiceType += "Knight";
            knight = new Knight(peiceType, newPanel, false);
            pieces.Add(knight);
          }
          else if ((tileSize * i == 80 || tileSize * i == 200) &&
                  (tileSize * x == 0 || tileSize * x == 280))
          {
            peiceType += "Bishop";
            bishop = new Bishop(peiceType, newPanel, false);
            pieces.Add(bishop);
          }
          else if ((tileSize * i == 160) &&
                  (tileSize * x == 0 || tileSize * x == 280))
          {
            peiceType += "King";
            king = new King(peiceType, newPanel, false);
            pieces.Add(king);
          }
          else if ((tileSize * i == 120) &&
                  (tileSize * x == 0 || tileSize * x == 280))
          {
            peiceType += "Queen";
            queen = new Queen(peiceType, newPanel, false);
            pieces.Add(queen);
          }

          boardPanels[i, x] = newPanel;

          if (i % 2 == 0)
          {
            newPanel.BackColor = x % 2 != 0 ? color1 : color2;
          }
          else
          {
            newPanel.BackColor = x % 2 != 0 ? color2 : color1;
          }
        }
      }

      //update what panels are currently taken
      setPanelsInUse();
      //return the board that has been generated
      return boardPanels;
    }

    public Panel[,] getPanels()
    {
      return boardPanels;
    }

    public List<Piece> getPieces()
    {
      return pieces;
    }

    public void setPanelsInUse()
    {
      inUse = new List<Panel>();
      foreach (Panel x in boardPanels)
      {
        foreach (Piece p in pieces)
        {
          if (x == p.getPanel())
          {
            inUse.Add(x);
          }
        }
      }
    }

    public List<Panel> getPanelsInUse()
    {
      return inUse;
    }

    public void RemovePiece(Piece piece)
    {
      pieces.Remove(piece);
      inUse.Remove(piece.getPanel());
    }

    public void AddPiece(Piece piece)
    {
      pieces.Add(piece);
      inUse.Add(piece.getPanel());
    }

    public List<Panel> getAllPossibleBMoves(BoardGen board)
    {
      List<Panel> allPossibleBMoves = new List<Panel>();
      possibleBPieces = new List<Piece>();
      foreach (Piece x in getPieces().ToList())
      {
        x.setMoves(board, false, false);
        if (x.getType().Substring(0, 1) == "B")
        {
          foreach (Panel p in x.getMoves())
          {
            allPossibleBMoves.Add(p);
            possibleBPieces.Add(x);
          }
        }
      }
      return allPossibleBMoves;
    }

    public List<Panel> getAllPossibleWMoves(BoardGen board)
    {
      List<Panel> allPossibleWMoves = new List<Panel>();
      possibleWPieces = new List<Piece>();
      foreach (Piece x in getPieces().ToList())
      {
        x.setMoves(board, false, false);
        if (x.getType().Substring(0, 1) == "W")
        {
          foreach (Panel p in x.getMoves())
          {
            allPossibleWMoves.Add(p);
            possibleWPieces.Add(x);
          }
        }
      }
      return allPossibleWMoves;
    }

    public List<Piece> getPossibleBPieces()
    {
      return possibleBPieces;
    }

    public List<Piece> getPossibleWPieces()
    {
      return possibleWPieces;
    }

  }
}
