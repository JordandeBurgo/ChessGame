using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CollegeProject
{
  public abstract class Piece
  {
    String pieceType;
    Panel piecePanel;
    string typeOfCheck;
    bool hasMoved;
    int noMoves;

    public Piece(String type, Panel image, bool moved)
    {

      image.BackgroundImage = (Image)
          (Properties.Resources.ResourceManager.GetObject(type));

      pieceType = type;
      piecePanel = image;
      hasMoved = moved;
      noMoves = 0;
      //Sets original values of the piece
    }

    public string checkType()
    {
      return typeOfCheck;
    }

    public abstract void setMoves(BoardGen board, bool checkUp, bool suicide);
    //To be overriden by child classes

    public abstract List<Panel> getMoves();
    //To be overriden by child classes

    public abstract bool capture();

    public String getType()
    {
      return pieceType;
    }

    public void setPanel(Panel newPanel)
    {
      piecePanel = newPanel;
    }

    public Panel getPanel()
    {
      return piecePanel;
    }

    public abstract double getValue(BoardGen board);

    public bool checkCheck(BoardGen board)
    {
      bool check = false;

      Piece piece = null;

      //Need to check all pieces on board because discovered check exists
      foreach (Piece y in board.getPieces())
      {

        List<Panel> possibleMoves = new List<Panel>();
        possibleMoves = y.getMoves();
        if (y.getMoves() != null)
        {
          foreach (Panel x in possibleMoves)
          {

            foreach (Piece c in board.getPieces())
            {
              if (c.getPanel() == x)
              {
                piece = c;
                if (piece.getType().Contains("King") &&
                   (y.getType().Substring(0, 1) !=
                   piece.getType().Substring(0, 1)))
                {
                  typeOfCheck = (piece.getType().Substring(0, 1)
                      == "W") ? "W" : "B";
                  return true;
                }
              }
            }
          }
        }
        //Checks if any piece has a possible move that contains "King",
        //if it does it is check

      }

      return check;

    }

    public void setMoved(bool move)
    {
      hasMoved = move;
    }

    public bool getMoved()
    {
      return hasMoved;
    }

    public int numberMoves()
    {
      return noMoves;
    }

    public void increaseNoMoves()
    {
      noMoves++;
    }

    public void decreaseNoMoves()
    {
      noMoves--;
    }
  }
}
