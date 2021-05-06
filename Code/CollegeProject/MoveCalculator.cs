using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollegeProject
{
  public class MoveCalculator
  {
    Piece piece;
    List<Piece> possibleWPieces;
    List<Piece> possibleBPieces;
    List<Panel> allPossibleBMoves;
    List<Panel> allPossibleWMoves;

    public Panel bestMove(BoardGen board, double alpha, double beta)
    {
      Panel bestMove = null;
      double bestVal = 100000;
      bool removed = false;
      Piece removedPiece = null;
      board.setPanelsInUse();
      allPossibleBMoves = board.getAllPossibleBMoves(board);
      allPossibleWMoves = board.getAllPossibleWMoves(board);
      possibleWPieces = board.getPossibleWPieces();
      possibleBPieces = board.getPossibleBPieces();

      for (var i = 0; i < allPossibleBMoves.Count(); i++)
      {
        Panel temp = possibleBPieces[i].getPanel();
        if (board.getPanelsInUse().Contains(allPossibleBMoves[i]))
        {
          foreach (Piece y in board.getPieces().ToList())
          {
            if (y.getPanel() == allPossibleBMoves[i])
            {
              removedPiece = y;
              removed = true;
              board.RemovePiece(y);
              //take piece if we're taking a piece
            }
          }
        }
        possibleBPieces[i].setPanel(allPossibleBMoves[i]);
        board.setPanelsInUse();
        board.getAllPossibleWMoves(board);
        board.getPossibleWPieces();
        double tempVal = minimax(board, true, 1, board.getAllPossibleWMoves(board),
                                 board.getPossibleWPieces(), alpha, beta);
        //start a search on this move to see where it leads
        if (tempVal < bestVal)
        {
          bestVal = tempVal;
          beta = tempVal;
          bestMove = allPossibleBMoves[i];
          piece = possibleBPieces[i];
          //if this move leads to a good board value then tempVal will be small/more
          //negative.If it is then we update bestVal, update beta and update the best
          //move and the piece it moves to get there
        }
        possibleBPieces[i].setPanel(temp);
        if (removed)
        {
          removed = false;
          board.AddPiece(removedPiece);
          //return the removed piece to its position
        }
        board.setPanelsInUse();
        //update board
      }
      return bestMove;
      //returns the best move we found
    }

    public double minimax(BoardGen board, bool max, int depth, List<Panel>
                          possibleMoves, List<Piece> possiblePieces, double alpha,
                          double beta)
    {
      double bestValue;
      bool removed = false;
      Piece removedPiece = null;
      board.setPanelsInUse();

      if (depth == 0)
      {
        return getBoardValue(board);
        //We have searched as far as we need to and can return what the board value
        //would be in this position
      }
      if (max) //if the player is trying to maximise score
      {
        double bestVal = -100000; //set bestVal to some very large negative number so
                                  //that it easily beaten

        for (var i = 0; i < possibleMoves.Count(); i++) //for every move possible from
                                                        //this position
        {
          Panel temp = possiblePieces[i].getPanel();
          if (board.getPanelsInUse().Contains(possibleMoves[i]))
          {
            foreach (Piece y in board.getPieces().ToList())
            {
              if (y.getPanel() == possibleMoves[i])
              {
                removedPiece = y;
                removed = true;
                board.RemovePiece(y);
                //take piece if we're taking a piece
              }
            }
          }
          possiblePieces[i].setPanel(possibleMoves[i]);
          board.setPanelsInUse();
          board.getAllPossibleWMoves(board);
          board.getPossibleWPieces();
          //update the board for one of the possible moves
          double tempVal = minimax(board, !max, depth - 1,
                                   board.getAllPossibleBMoves(board),
                                   board.getPossibleBPieces(), alpha, beta);
          //dive deeper into a search looking ahead by 1 furhter witht tempVal
          if (tempVal > bestVal)
          {
            bestVal = tempVal;
            alpha = tempVal;

            //if tempVal is better than the current bestVal then this is the new
            //tempVal and alpha is set to tempVal
          }
          possiblePieces[i].setPanel(temp);
          if (removed)
          {
            removed = false;
            board.AddPiece(removedPiece);
            //put taken piece back
          }
          board.setPanelsInUse();

          if (alpha >= beta)
          {
            return bestVal;
            //If alpha is greater or equal to beta we can prune the search
          }

        }
        bestValue = bestVal;
      }
      else
      {
        double bestVal = 100000; //We're minimising so some large value will be easily
                                //beaten here
        for (var i = 0; i < possibleMoves.Count(); i++)
        {
          Panel temp = possiblePieces[i].getPanel();
          if (board.getPanelsInUse().Contains(possibleMoves[i]))
          {
            foreach (Piece y in board.getPieces().ToList())
            {
              if (y.getPanel() == possibleMoves[i])
              {
                removedPiece = y;
                removed = true;
                board.RemovePiece(y);
                //take piece if we're taking a piece
              }
            }
          }
          possiblePieces[i].setPanel(possibleMoves[i]);
          board.setPanelsInUse();
          board.getAllPossibleBMoves(board);
          board.getPossibleBPieces();
          //update the board to do this move
          double tempVal = minimax(board, !max, depth - 1,
                                   board.getAllPossibleWMoves(board),
                                   board.getPossibleWPieces(), alpha, beta);
          //go deeper to see if this move will lead anywhere good
          beta = Math.Min(beta, tempVal);
          if (tempVal < bestVal)
          {
            bestVal = tempVal;
            beta = tempVal;
            //if this move leads somewhere good then tempVal will be less than bestVal
            //as black tries to minimise, so this val is set to bestVal
          }
          possiblePieces[i].setPanel(temp);
          if (removed)
          {
            removed = false;
            board.AddPiece(removedPiece);
            //return the piece that was taken
          }
          board.setPanelsInUse();
          if (alpha >= beta)
          {
            return bestVal;
            //if alpha is greater than beta then we can prune the search a bit
          }

        }
        bestValue = bestVal;
      }
      return bestValue;
      //return the best value we have found
    }

    public Piece getMovingPiece()
    {
      return piece;
    }
    public Panel getMovingPiecePanel()
    {
      return piece.getPanel();
    }

    public double getBoardValue(BoardGen board)
    {
      double boardValue = 0; //boardValue starts as zero
      foreach (Piece p in board.getPieces().ToList())
      {
        if (p.getType().Substring(0, 1) == "B")
        {
          boardValue = boardValue - p.getValue(board);
          //when there's a black piece the board value becomes smaller/more negative
        }
        else
        {
          boardValue = boardValue + p.getValue(board);
          //otherwise the board value becomes bigger
        }
      }
      return boardValue;
    }
  }
}
