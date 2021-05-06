using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CollegeProject
{
  [Serializable]
  public class Knight : Piece
  {
    List<Panel> possibleMoves;
    bool capturePossible = false;
    double[,] KnightTable;

    public Knight(string type, Panel image, bool moved) : base(type, image, moved)
    {
      KnightTable = new double[8, 8]
          { { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 },
                  { -4.0, -2.0,  0.0,  0.0,  0.0,  0.0, -2.0, -4.0 },
                  { -3.0,  0.0,  1.0,  1.5,  1.5,  1.0,  0.0, -3.0 },
                  { -3.0,  0.5,  1.5,  2.0,  2.0,  1.5,  0.5, -3.0 },
                  { -3.0,  0.0,  1.5,  2.0,  2.0,  1.5,  0.0, -3.0 },
                  { -3.0,  0.5,  1.0,  1.5,  1.5,  1.0,  0.5, -3.0 },
                  { -4.0, -2.0,  0.0,  0.5,  0.5,  0.0, -2.0, -4.0 },
                  { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 }
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
              double val = (getType().Substring(0, 1) == "W") ? KnightTable[x, y] : KnightTable[x, y];
              return val + 30;
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
      List<Panel> panelsRemove = new List<Panel>();

      /*A knight moves in a L shape pattern, any "L" shape (3 squares in a 
      straight line and then one to the left or right).*/

      foreach (Panel x in board.getPanels())
      {
        if ((getPanel().Location.X == x.Location.X + 40 ||
            getPanel().Location.X == x.Location.X - 40) &&
            (getPanel().Location.Y == x.Location.Y + 80 ||
            getPanel().Location.Y == x.Location.Y - 80))
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
        if ((getPanel().Location.X == x.Location.X + 80 ||
            getPanel().Location.X == x.Location.X - 80) &&
            (getPanel().Location.Y == x.Location.Y + 40 ||
            getPanel().Location.Y == x.Location.Y - 40))
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


      /*This part gets all the possible moves, no need to remove panels being
        blocked as a knight can jump over pieces. The only thing that would
        make it not possible is if a same coloured piece is on the panel, this
        was easy enough to add to the if statements*/

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

          if (checkCheck(board) && checkType() ==
              getType().Substring(0, 1))
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

      /* Exactly like all other pieces, this part checks all of the current 
         possible moves for the the bisop to see if any would leave the player
         in check, if it would, it is not a valid move and is removed. */

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
      else if (!capturePossible && suicide)  //if there no captures possible in suicide chess
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
