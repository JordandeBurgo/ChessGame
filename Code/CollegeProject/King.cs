using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CollegeProject
{
  [Serializable]
  public class King : Piece
  {
    List<Panel> possibleMoves;
    double[,] KingTable;
    bool capturePossible = false;

    public King(string type, Panel image, bool moved) : base(type, image, moved)
    {
      KingTable = new double[8, 8]
          { { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
                  { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
                  { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
                  { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
                  { -2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0 },
                  { -1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0 },
                  {  2.0,  2.0,  0.0,  0.0,  0.0,  0.0,  2.0,  2.0 },
                  {  2.0,  3.0,  1.0,  0.0,  0.0,  1.0,  3.0,  2.0 }
          };
      if (type.Substring(0, 1) == "B")
      {
        KingTable = new double[8, 8]
        { {  2.0,  3.0,  1.0,  0.0,  0.0,  1.0,  3.0,  2.0 },
                  {  2.0,  2.0,  0.0,  0.0,  0.0,  0.0,  2.0,  2.0 },
                  { -1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0 },
                  { -2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0 },
                  { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
                  { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
                  { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
                  { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 }
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
            if (board.getPanels()[x, y] == getPanel())
            {
              double val = (getType().Substring(0, 1) == "W") ? KingTable[x, y] : KingTable[y, x];
              return val + 10000;
            }
          }
        }
      }

      return 0;
    }

    public override void setMoves(BoardGen board, bool checkUp, bool suicide)
    {
      possibleMoves = new List<Panel>();
      List<Panel> panelsRemove = new List<Panel>();
      Piece piece = null;
      Panel tempPan = null;
      capturePossible = false;


      //A king can move 1 sqaure in any direction.



      foreach (Panel x in board.getPanels())
      {
        foreach (Piece c in board.getPieces())
        {
          if (c.getPanel() == x)
          {
            piece = c;
          }
        }
        if (x.Location.X == getPanel().Location.X)
        {
          if (x.Location.Y == getPanel().Location.Y + 40 ||
              x.Location.Y == getPanel().Location.Y - 40)
          {
            possibleMoves.Add(x);
          }
        }
        if (x.Location.X == getPanel().Location.X + 40)
        {
          if (x.Location.Y == getPanel().Location.Y + 40 ||
              x.Location.Y == getPanel().Location.Y - 40 ||
              x.Location.Y == getPanel().Location.Y)
          {
            possibleMoves.Add(x);
          }
        }
        if (x.Location.X == getPanel().Location.X - 40)
        {
          if (x.Location.Y == getPanel().Location.Y + 40 ||
              x.Location.Y == getPanel().Location.Y - 40 ||
              x.Location.Y == getPanel().Location.Y)
          {
            possibleMoves.Add(x);
          }
        }
        /*Similarly to other pieces this section gets all moves that would 
          be possible for the king on an empty board*/


        if (piece != null)
        {
          if (piece.getType().Substring(0, 1) == getType().Substring(0, 1) &&
          board.getPanelsInUse().Contains(x))
          {
            panelsRemove.Add(x);
          }
          else
          {

          }
        }
        /*And this just adds all the blocked sqaures to a list of not
          possible moves*/

      }

      foreach (Panel x in panelsRemove)
      {
        possibleMoves.Remove(x);
      }
      bool castle1 = false;
      bool castle2 = false;
      if (!getMoved())
      {
        if (!board.getPanelsInUse().Contains(board.getPanels()[5, getType().Substring(0, 1) == "W" ? 7 : 0]) && !board.getPanelsInUse().Contains(board.getPanels()[6, getType().Substring(0, 1) == "W" ? 7 : 0]))
        {
          possibleMoves.Add(board.getPanels()[6, getType().Substring(0, 1) == "W" ? 7 : 0]);

          castle1 = true;
        }

        if (!board.getPanelsInUse().Contains(board.getPanels()[1, getType().Substring(0, 1) == "W" ? 7 : 0]) && !board.getPanelsInUse().Contains(board.getPanels()[2, getType().Substring(0, 1) == "W" ? 7 : 0]) && !board.getPanelsInUse().Contains(board.getPanels()[3, getType().Substring(0, 1) == "W" ? 7 : 0]))
        {
          possibleMoves.Add(board.getPanels()[2, getType().Substring(0, 1) == "W" ? 7 : 0]);

          castle2 = true;
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

          if (checkCheck(board))
          {
            if (castle1 && x == board.getPanels()[5, getType().Substring(0, 1) == "W" ? 7 : 0])
            {
              panelsRemove.Add(board.getPanels()[6, getType().Substring(0, 1) == "W" ? 7 : 0]);
            }
            if (castle2 && x == board.getPanels()[3, getType().Substring(0, 1) == "W" ? 7 : 0])
            {
              panelsRemove.Add(board.getPanels()[2, getType().Substring(0, 1) == "W" ? 7 : 0]);
            }
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



      }
      /* Exactly like all other pieces, this part checks all of the current 
         possible moves for the queen to see if any would leave the player
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
