using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CollegeProject
{
  [Serializable]
  public class Queen : Piece
  {
    List<Panel> possibleMoves;
    bool capturePossible = false;
    double[,] QueenTable;

    public Queen(string type, Panel image, bool moved) : base(type, image, moved)
    {
      QueenTable = new double[8, 8]
          { { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 },
                  { -1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0 },
                  { -1.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0 },
                  { -0.5,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5 },
                  {  0.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5 },
                  { -1.0,  0.5,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0 },
                  { -1.0,  0.0,  0.5,  0.0,  0.0,  0.0,  0.0, -1.0 },
                  { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 }
          };
    }

    public override double getValue(BoardGen board)
    {
      for (int x = 0; x < 8; x++)
      {
        for (int y = 0; y < 8; y++)
        {
          if (board.getPanels()[x, y] == getPanel())
          {
            if (board.getPanels()[x, y] == getPanel())
            {
              double val = (getType().Substring(0, 1) == "W") ? QueenTable[x, y] : QueenTable[y, x];
              return val + 90;
            }
          }
        }
      }
      return 0;
    }

    public override void setMoves(BoardGen board, bool checkUp, bool suicide)
    {
      possibleMoves = new List<Panel>();
      Panel tempPan = null;
      capturePossible = false;

      /*Queens move like a combination of a rook and bishop. Along a diagnol or
        in a straight line*/

      foreach (Panel x in board.getPanels())
      {
        if ((getPanel().Location.Y == x.Location.Y ||
            getPanel().Location.X == x.Location.X))
        {
          if (board.getPanelsInUse().Contains(x))
          {

            foreach (Piece y in board.getPieces())
            {
              if (y.getPanel() == x)
              {
                if (getType().Substring(0, 1) !=
                    y.getType().Substring(0, 1))
                {
                  possibleMoves.Add(x);
                }
              }
            }
          }
          else
          {
            possibleMoves.Add(x);
          }
        }

      }

      //Gets all the moves a queen could make if the board were empty

      List<Panel> panelsRemove = new List<Panel>();

      foreach (Panel x in board.getPanels())
      {
        if (getPanel().Location.X == x.Location.X &&
            board.getPanelsInUse().Contains(x))
        {
          if (x.Location.Y > getPanel().Location.Y)
          {
            foreach (Panel g in possibleMoves)
            {
              if (g.Location.Y > x.Location.Y)
              {
                panelsRemove.Add(g);
              }
            }
          }
          else if (x.Location.Y < getPanel().Location.Y)
          {
            foreach (Panel g in possibleMoves)
            {
              if (g.Location.Y < x.Location.Y)
              {
                panelsRemove.Add(g);
              }
            }
          }
        }
        if (getPanel().Location.Y == x.Location.Y &&
            board.getPanelsInUse().Contains(x))
        {
          if (x.Location.X > getPanel().Location.X)
          {
            foreach (Panel g in possibleMoves)
            {
              if (g.Location.X > x.Location.X)
              {
                panelsRemove.Add(g);
              }
            }

          }
          else if (x.Location.X < getPanel().Location.X)
          {
            foreach (Panel g in possibleMoves)
            {
              if (g.Location.X < x.Location.X)
              {
                panelsRemove.Add(g);
              }
            }
          }
        }
      }
      Piece piece = null;

      foreach (Panel x in board.getPanels())
      {
        for (int i = 0; i < 8; i++)
        {
          foreach (Piece c in board.getPieces())
          {
            if (c.getPanel() == x)
            {
              piece = c;
            }
          }

          if ((getPanel().Location.X == x.Location.X + (40 * i) ||
              getPanel().Location.X == x.Location.X - (40 * i)) &&
              (getPanel().Location.Y == x.Location.Y + (40 * i) ||
              getPanel().Location.Y == x.Location.Y - (40 * i)))
          {
            if (board.getPanelsInUse().Contains(x))
            {

              foreach (Piece y in board.getPieces())
              {
                if (y.getPanel() == x)
                {
                  if (getType().Substring(0, 1) !=
                      y.getType().Substring(0, 1))
                  {
                    possibleMoves.Add(x);
                  }
                }
              }
            }
            else
            {
              possibleMoves.Add(x);
            }
          }
        }

      }

      foreach (Panel x in board.getPanels())
      {
        for (int i = 0; i < 8; i++)
        {
          if ((getPanel().Location.X == x.Location.X + (40 * i) ||
              getPanel().Location.X == x.Location.X - (40 * i)) &&
              (getPanel().Location.Y == x.Location.Y + (40 * i) ||
              getPanel().Location.Y == x.Location.Y - (40 * i)) &&
              board.getPanelsInUse().Contains(x))
          {
            if (x.Location.Y < getPanel().Location.Y &&
                x.Location.X < getPanel().Location.X)
            {
              foreach (Panel g in possibleMoves)
              {
                if (g.Location.Y < x.Location.Y &&
                    g.Location.X < x.Location.X)
                {
                  panelsRemove.Add(g);

                }
              }
            }
            else if (x.Location.Y < getPanel().Location.Y &&
                x.Location.X > getPanel().Location.X)
            {
              foreach (Panel g in possibleMoves)
              {
                if (g.Location.Y < x.Location.Y &&
                    g.Location.X > x.Location.X)
                {
                  panelsRemove.Add(g);

                }
              }
            }
            else if (x.Location.Y > getPanel().Location.Y &&
                     x.Location.X < getPanel().Location.X)
            {
              foreach (Panel g in possibleMoves)
              {
                if (g.Location.Y > x.Location.Y &&
                    g.Location.X < x.Location.X)
                {
                  panelsRemove.Add(g);
                }
              }
            }
            else if (x.Location.Y > getPanel().Location.Y &&
                x.Location.X > getPanel().Location.X)
            {
              foreach (Panel g in possibleMoves)
              {
                if (g.Location.Y > x.Location.Y &&
                    g.Location.X > x.Location.X)
                {
                  panelsRemove.Add(g);
                }
              }
            }
          }
        }
      }

      foreach (Panel x in panelsRemove)
      {
        possibleMoves.Remove(x);
      }

      foreach (Panel x in possibleMoves)
      {
        foreach (Piece p in board.getPieces())
        {
          if (x == p.getPanel())
          {
            capturePossible = true;
          }
        }
      }


      //Removes any moves blocked by another piece.

      if (checkUp && !suicide) //if checking for check and this is not suicide chess (as chess doesn't exist in suicide chess)
      {
        Piece toRem = null;
        bool removed = false;
        tempPan = getPanel();
        foreach (Panel x in possibleMoves)
        {
          if (board.getPanelsInUse().Contains(x))
          {
            foreach (Piece z in board.getPieces().ToList())
            {
              if (z.getPanel() == x)
              {
                toRem = z;
                removed = true;
                board.RemovePiece(z);
              }
            }
          }

          setPanel(x);

          board.setPanelsInUse();

          foreach (Piece y in board.getPieces().ToList())
          {
            if (!y.getType().Contains(getType()))
            {
              y.setMoves(board, false, suicide);
            }

          }

          if (checkCheck(board) && checkType() == getType().Substring(0, 1))
          {
            panelsRemove.Add(x);
          }

          setPanel(tempPan);

          if (removed)
          {
            board.AddPiece(toRem);
            removed = false;
          }

          board.setPanelsInUse();
        }

        foreach (Panel x in panelsRemove)
        {
          possibleMoves.Remove(x);
        }
      }

      /* This part checks all of the current possible moves for the
         the bisop to see if any would leave the player in check, if it
         would, it is not a valid move and is removed. */

      if (capturePossible && suicide) //if it is suicide chess and there is a capture possible
      {
        List<Panel> newPossibleMoves = new List<Panel>();
        foreach (Panel x in possibleMoves)
        {
          foreach (Piece y in board.getPieces())
          {
            if (x == y.getPanel())
            {
              newPossibleMoves.Add(x);
            }
          }
        }
        possibleMoves = newPossibleMoves;
        //captures are the only possible moves if a capture is possible
      }
      else if (!capturePossible && suicide) //if there no captures possible in suicide chess
      {
        foreach (Piece x in board.getPieces())
        {
          if (x.capture() && x.getType().Substring(0, 1) == getType().Substring(0, 1))
          {
            possibleMoves = new List<Panel>();
            break;
            //if there are no possible captures, but there are possible captures from other pieces, this piece cannot move
          }
        }
      }
    }
    public override List<Panel> getMoves()
    {
      return possibleMoves;
    }

    public override bool capture()
    {
      return capturePossible;
    }
  }
}
