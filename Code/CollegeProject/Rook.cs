using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollegeProject
{
    public class Rook : Piece
    {
        
        public List<Panel> possibleMoves;
        Panel tempPan = null;
        bool capturePossible = false;
        double[,] RookTable;
 
        public Rook(string type, Panel image, bool moved) : base(type, image, moved)
        {
            RookTable = new double[8, 8]
                { {  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0 },
                  {  0.5,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5 },
                  { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5 },
                  { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5 },
                  { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5 },
                  { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5 },
                  { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5 },
                  {  0.0,  0.0,  0.0,  0.5,  0.5,  0.0,  0.0,  0.0 }
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
                            double val = (getType().Substring(0, 1) == "W") ? RookTable[x, y] : RookTable[y, x];
                            return val + 50;
                        }
                    }
                }
            }
            return 0;
        }

        public override void setMoves(BoardGen board, bool checkUp, bool suicide)
        {
            possibleMoves = new List<Panel>();
            capturePossible = false;
            //Rooks can move in a straigh line, up, down, left and right.
            foreach(Panel x in board.getPanels()) {
                if ((getPanel().Location.Y == x.Location.Y ||
                    getPanel().Location.X == x.Location.X))
                {
                    if (board.getPanelsInUse().Contains(x))
                    {

                        foreach(Piece y in board.getPieces())
                        {
                            if(y.getPanel() == x)
                            {
                                if(getType().Substring(0, 1) !=
                                   y.getType().Substring(0 , 1))
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

            /*This part just gets all of the panels with the same x or y value as
              the rook in use*/

            List<Panel> panelsRemove = new List<Panel>();

            foreach(Panel x in board.getPanels())
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
            /*This part adds any moves that aren't possible due to a piece blocking
              the move to an array that is removed from possible moves*/
            

            foreach(Panel x in panelsRemove)
            {
                possibleMoves.Remove(x);
            }

            foreach(Panel x in possibleMoves)
            {
                foreach(Piece p in board.getPieces())
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
                            y.setMoves(board, false, suicide);
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
