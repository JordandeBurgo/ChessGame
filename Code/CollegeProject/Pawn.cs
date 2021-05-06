using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CollegeProject
{
  [Serializable]
  public class Pawn : Piece
  {
    public List<Panel> possibleMoves;
    bool capturePossible = false;
    double[,] PawnTable;

    public Pawn(string type, Panel image, bool moved) : base(type, image, moved)
    {
      if (type.Substring(0, 1) == "B")
      {
        PawnTable = new double[8, 8]
            { { 0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0 },
                      { 0.5,  1.0,  1.0, -2.0, -2.0,  1.0,  1.0,  0.5 },
                      { 0.5, -0.5, -1.0,  0.0,  0.0,  2.0,  1.0,  0.5 },
                      { 0.0,  0.0,  0.0,  2.0,  2.0,  1.0,  0.5,  0.0 },
                      { 0.5,  0.5,  1.0,  2.5,  2.5,  1.0,  0.5,  0.5 },
                      { 1.0,  1.0,  2.0,  3.0,  3.0,  2.0,  1.0,  1.0 },
                      { 5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0 },
                      { 0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0 }
            };
      }
      else
      {
        PawnTable = new double[8, 8]
            { { 0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0 },
                      { 5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0 },
                      { 1.0,  1.0,  2.0,  3.0,  3.0,  2.0,  1.0,  1.0 },
                      { 0.5,  0.5,  1.0,  2.5,  2.5,  1.0,  0.5,  0.5 },
                      { 0.0,  0.0,  0.0,  2.0,  2.0,  0.0,  0.0,  0.0 },
                      { 0.5, -0.5, -1.0,  0.0,  0.0, -1.0, -0.5,  0.5 },
                      { 0.5,  1.0,  1.0, -2.0, -2.0,  1.0,  1.0,  0.5 },
                      { 0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0 }
            };
      }

    }

    public override double getValue(BoardGen board)
    {
      for (int x = 0; x < 8; x++)
      {
        for (int y = 0; y < 8; y++)
        {
          if (board.getPanels()[x, y] == getPanel())
          {
            double val = (getType().Substring(0, 1) == "W") ? PawnTable[y, x] : PawnTable[y, x];
            return val + 10;
          }
        }
      }
      return 0;
    }

    public override void setMoves(BoardGen board, bool checkUp, bool suicide)
    {
      Piece piece = null;
      Panel tempPan = null;
      possibleMoves = new List<Panel>();
      List<Panel> panelsRemove = new List<Panel>();
      capturePossible = false;
      bool noMoves = false;

      /*White's pawns can move up the board, Black's move down. If it is
        the first move then a pawn can move two squares, otherwise only
        one. Pawns also take other pieces diagonaly.*/

      foreach (Panel x in board.getPanels())
      {
        foreach (Piece c in board.getPieces())
        {
          if (c.getPanel() == x)
          {
            piece = c;
          }
        }
        if (piece != null)
        {
          if (getType().Substring(0, 1).Equals("W"))
          {
            if (board.getPanelsInUse().Contains(x) &&
                (x.Location.Y == getPanel().Location.Y - 40) &&
                (x.Location.X == getPanel().Location.X))
            {
              noMoves = true;
              foreach (Panel z in board.getPanels())
              {
                if ((((getPanel().Location.Y == z.Location.Y + 40) &&
                (z.Location.X == getPanel().Location.X - 40)) ||
                (z.Location.X == getPanel().Location.X + 40)))
                {
                  noMoves = false;
                }
              }
            }
            else if (!(board.getPanelsInUse().Contains(x)) &&
                    (getPanel().Location.Y == 240) &&
                    ((x.Location.Y == getPanel().Location.Y - 40) ||
                    (x.Location.Y == getPanel().Location.Y - 80)) &&
                    (x.Location.X == getPanel().Location.X))
            {
              possibleMoves.Add(x);
            }
            else if (!(board.getPanelsInUse().Contains(x)) &&
                    (x.Location.Y == getPanel().Location.Y - 40) &&
                    (x.Location.X == getPanel().Location.X))
            {
              possibleMoves.Add(x);
            }
            else if (board.getPanelsInUse().Contains(x) &&
                    (getType().Substring(0, 1) !=
                    piece.getType().Substring(0, 1)))
            {
              if ((getPanel().Location.Y == x.Location.Y + 40 &&
                  (x.Location.X == getPanel().Location.X - 40 ||
                  x.Location.X == getPanel().Location.X + 40)))
              {
                possibleMoves.Add(x);

              }
            }
          }
          else if (getType().Substring(0, 1).Equals("B"))
          {
            if (board.getPanelsInUse().Contains(x) &&
                (x.Location.Y == getPanel().Location.Y + 40) &&
                (x.Location.X == getPanel().Location.X))
            {
              noMoves = true;
              foreach (Panel z in board.getPanels())
              {
                if ((((getPanel().Location.Y == z.Location.Y - 40) &&
                (z.Location.X == getPanel().Location.X - 40)) ||
                (z.Location.X == getPanel().Location.X + 40)))
                {
                  noMoves = false;
                }
              }

            }
            else if (!(board.getPanelsInUse().Contains(x)) &&
                    (getPanel().Location.Y == 40) &&
                    ((x.Location.Y == getPanel().Location.Y + 40) ||
                    (x.Location.Y == getPanel().Location.Y + 80)) &&
                    (x.Location.X == getPanel().Location.X))
            {
              possibleMoves.Add(x);
            }
            else if (!(board.getPanelsInUse().Contains(x)) &&
                    (x.Location.Y == getPanel().Location.Y + 40) &&
                    (x.Location.X == getPanel().Location.X))
            {
              possibleMoves.Add(x);
            }
            else if (board.getPanelsInUse().Contains(x) &&
                    (getType().Substring(0, 1) !=
                    piece.getType().Substring(0, 1)))
            {
              if ((getPanel().Location.Y == x.Location.Y - 40 &&
                  (x.Location.X == getPanel().Location.X - 40 ||
                  x.Location.X == getPanel().Location.X + 40)))
              {
                possibleMoves.Add(x);

              }
            }
          }
        }

      }

      /*This section adds all the moves the pawn could make if the board were
        empty to a list called possibleMoves and then all the ones that are
        made invalid due to other pieces are added to the list called
        panelsRemove*/

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

        if (noMoves)
        {
          possibleMoves = new List<Panel>();
        }
      }

      /* Exactly like all other pieces, this part checks all of the current 
         possible moves for the pawn to see if any would leave the player
         in check, if it would, it is not a valid move and is removed. */

      if (capturePossible && suicide)  //if it is suicide chess and there is a capture possible
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
