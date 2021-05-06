using CollegeProject.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CollegeProject
{
  public partial class Form1 : Form
  {

    //declaration of variables
    List<Panel> undoPanel = new List<Panel>();
    List<Piece> undoPiece = new List<Piece>();
    Pawn piecePosRemove;
    int noUndos = 0;
    List<int> undos = new List<int>();
    string gameModes;
    int WhiteSeconds;
    int BlackSeconds;
    public bool timed = false;
    public bool suicide = false;
    private bool WhiteMove = true;
    private Color tempBackColour;
    private Piece movingPiece = null;
    private Panel movingPiecePanel;
    public BoardGen generation = new BoardGen();
    private MoveCalculator moveCalc = new MoveCalculator();
    private Panel[,] board;
    private Panel actionPanel;
    private List<Piece> pieces;
    private Piece newQueen;
    PictureBox[] pictureBoxesB;
    PictureBox[] pictureBoxesW;
    Panel toUpdate = null;
    Piece updatePiece = null;
    bool updateReq = false;
    int bPiecesTaken = 0;
    int wPiecesTaken = 0;


    public Form1(String gameMode)
    {
      gameModes = gameMode;
      if (gameMode == "M3S") //if the gameMode is M3S then the user is playing suicide chess
      {
        suicide = true;
      }
      board = generation.GenerateBoard();
      //Generate the board
      pictureBoxesB = new PictureBox[15];
      int z = 330;
      int y = 60;
      if (gameMode == "M1B" || gameMode == "M2B") //if the gameMode is M1B or M2B then the game is timed
      {
        WhiteSeconds = gameMode == "M1B" ? 600 : 60; //and the time is set accordingly
        BlackSeconds = WhiteSeconds;
        timed = true;
      }

      foreach (PictureBox x in pictureBoxesB) //Generates the interface where taken black pieces are stored
      {
        PictureBox newPictureBox = new PictureBox
        {
          Size = new Size(25, 25),
          BackgroundImageLayout = ImageLayout.Stretch,
        };
        newPictureBox.Location = new Point(z, y);
        z += 25;
        if (z > 505)
        {
          z = 330;
          y += 25;
        }
        pictureBoxesB[bPiecesTaken ] = newPictureBox;
        Controls.Add(pictureBoxesB[bPiecesTaken ]);
        bPiecesTaken ++;

      }

      pictureBoxesW = new PictureBox[15];
      z = 330;
      y = 215;
      bPiecesTaken  = 0;

      foreach (PictureBox x in pictureBoxesW) //Generates the interface where taken white pieces
      {
        PictureBox newPictureBox = new PictureBox
        {
          Size = new Size(25, 25),
          BackgroundImageLayout = ImageLayout.Stretch,
        };
        newPictureBox.Location = new Point(z, y);
        z += 25;
        if (z > 505)
        {
          z = 330;
          y += 25;
        }
        pictureBoxesW[bPiecesTaken ] = newPictureBox;
        Controls.Add(pictureBoxesW[bPiecesTaken ]);
        bPiecesTaken ++;

      }

      foreach (Panel x in board)
      {
        x.MouseEnter += new EventHandler(panel_MouseEnter);
        x.MouseLeave += new EventHandler(panel_MouseLeave);
        x.MouseDown += new MouseEventHandler(panel_MouseDown);
        Controls.Add(x);
      }
      //Add the action events to the panels and then render the panels.
      bPiecesTaken  = 0;
      InitializeComponent();
      if (gameMode == "M1B" || gameMode == "M2B") //formats the time to display in minutes
      {
        label4.ForeColor = Color.Red;
        label3.Text = gameMode == "M1B" ? "10:00" : "01:00";
        label4.Text = label3.Text;
      }

      timer1.Start(); //starts the timer

    }

    private void panel_MouseEnter(object sender, EventArgs e)
    {
      if (timer1.Enabled)
      {
        actionPanel = (Panel)sender;
        tempBackColour = actionPanel.BackColor;
        actionPanel.BackColor = ColorTranslator.FromHtml("#ebe867");
      }


      //When mouse is over a panel it appears yellow
    }

    private void panel_MouseLeave(object sender, EventArgs e)
    {
      if (timer1.Enabled)
      {
        actionPanel = (Panel)sender;
        actionPanel.BackColor = tempBackColour;
      }


      //Puts panel back to original colour when mouse not over it
    }

    private void panel_MouseDown(object sender, MouseEventArgs e)
    {

      Panel actionPanel = (Panel)sender;
      bool castle = false;

      //This section is for when no piece has been clicked yet
      if (timer1.Enabled) //if the game hasn't ended
      {
        if (movingPiece == null)
        {
          pieces = generation.getPieces();

          //Gets the piece that was clicked
          foreach (Piece p in pieces)
          {
            if (p.getPanel() == actionPanel)
            {
              movingPiece = p;
              movingPiecePanel = actionPanel;
              break;
            }
          }

          //If the panel that was clicked actually contains a piece
          try
          {
            //And it's that peice's colours turn
            if ((WhiteMove &&
                movingPiece.getType().Substring(0, 1).Equals("W")) ||
               (!WhiteMove &&
               movingPiece.getType().Substring(0, 1).Equals("B")))
            {
              movingPiece.setMoves(generation, true, suicide);
              //Get that piece's moves

              foreach (Panel x in movingPiece.getMoves())
              {
                x.BackColor = ColorTranslator.FromHtml("#FFFC7F");
                //And display them
              }

            }
          }
          catch (NullReferenceException)
          {
            //Could notify the player they clicked an empty sqaure here
          }
        }

        //This section is for when a piece has already been clicked
        else
        {
          movingPiece.setMoves(generation, true, suicide);
          //Update the moves of that piece for their new position

          //Get the colours of the board back to normal
          for (int k = 0; k <= 7; k++)
          {
            for (int s = 0; s <= 7; s++)
            {

              if (k % 2 == 0)
              {
                generation.getPanels()[k, s].BackColor =
                    s % 2 != 0 ? ColorTranslator.FromHtml("#80A83E") : ColorTranslator.FromHtml("#D9DD76");
              }
              else
              {
                generation.getPanels()[k, s].BackColor =
                    s % 2 != 0 ? ColorTranslator.FromHtml("#D9DD76") : ColorTranslator.FromHtml("#80A83E");
              }
            }
          }

          //If that panel clicked was not the same as the first panel the person clicked
          if (actionPanel != movingPiecePanel && movingPiece.getType().Substring(0, 1) == (WhiteMove ? "W" : "B"))
          {
            //And the panel is valid
            try
            {
              if (movingPiece.getMoves().Contains(actionPanel))
              {

                if (generation.getPanelsInUse().Contains(actionPanel))
                {
                  foreach (Piece z in generation.getPieces().ToList())
                  {
                    if (z.getPanel() == actionPanel)
                    {
                      generation.RemovePiece(z);
                      if (z.getType().Substring(0, 1) == "W")
                      {
                        pictureBoxesB[bPiecesTaken ].BackgroundImage = z.getPanel().BackgroundImage;
                        bPiecesTaken ++;

                      }
                      else
                      {
                        pictureBoxesW[wPiecesTaken ].BackgroundImage = z.getPanel().BackgroundImage;
                        wPiecesTaken ++;
                      }
                    }
                  }
                  actionPanel.BackgroundImage = null;
                  /*If the panel was a possible move and contains a
                  piece, the piece that it already contains needs
                  to be disposed of and stored in the interface where
                  taken pieces are displayed*/

                }
                if (movingPiece.getType().Contains("King"))
                {
                  if ((actionPanel == generation.getPanels()[6, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0] || actionPanel == generation.getPanels()[2, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0]) && !movingPiece.getMoved())
                  {
                    castle = true;
                  }
                }
                //Recognises when the king is trying to castle


                if ((WhiteMove &&
                    movingPiece.getType().Substring(0, 1).Equals("W")) ||
                   (!WhiteMove &&
                   movingPiece.getType().Substring(0, 1).Equals("B")))
                {
                  actionPanel.BackgroundImage =
                  movingPiece.getPanel().BackgroundImage;

                  undoPiece.Add(movingPiece);
                  undoPanel.Add(movingPiece.getPanel());
                  noUndos++;

                  movingPiece.setPanel(actionPanel);

                  if (updateReq && movingPiece.getPanel() != updatePiece.getPanel())
                  {
                    updatePiece.setPanel(toUpdate);
                    generation.RemovePiece(piecePosRemove);
                    updateReq = false;
                    //if a pawn has moved foward two last move, it can no longer be taken as if it were on the first sqaure, so don't allow that to happen.
                  }
                  else if (updateReq)
                  {
                    if (movingPiece.getType().Substring(0, 1) == "W")
                    {
                      pictureBoxesW[wPiecesTaken  - 1].BackgroundImage = Resources.BPawn;
                    }
                    else
                    {
                      pictureBoxesB[bPiecesTaken  - 1].BackgroundImage = Resources.WPawn;
                    }
                    updatePiece.getPanel().BackgroundImage = movingPiece.getPanel().BackgroundImage;
                    piecePosRemove.getPanel().BackgroundImage = null;
                    generation.RemovePiece(piecePosRemove);
                    updatePiece.setPanel(null);

                    updateReq = false;

                    //If a pawn has moved foward two last move, and a pawn has taken it as if it were on the first square, this allows it to be taken.
                  }
                  if (movingPiece.getType().Contains("Pawn"))
                  {
                    if (!movingPiece.getMoved() && movingPiece.getPanel().Location.Y == movingPiecePanel.Location.Y + ((movingPiece.getType().Contains("W")) ? -80 : 80))
                    {
                      foreach (Panel x in generation.getPanels())
                      {
                        if (x.Location.Y + (movingPiece.getType().Contains("W") ? -40 : 40) == actionPanel.Location.Y && x.Location.X == actionPanel.Location.X)
                        {
                          toUpdate = actionPanel;
                          updatePiece = movingPiece;
                          movingPiece.setPanel(x);
                          updateReq = true;
                          piecePosRemove = new Pawn(updatePiece.getType(), actionPanel, true);
                          generation.AddPiece(piecePosRemove);
                          break;
                        }
                      }
                    }
                  }

                  generation.setPanelsInUse();
                  movingPiecePanel.BackgroundImage = null;
                  movingPiece.setMoved(true);
                  movingPiece.increaseNoMoves();

                  //As long as the right person (whose turn it is) is
                  //moving then this bit above moves it

                  if (castle)  //if the king is attempting to castle
                  {
                    int action = -1;
                    if (actionPanel == generation.getPanels()[6, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0])
                    {
                      foreach (Piece x in generation.getPieces())
                      {
                        if (x.getPanel() == generation.getPanels()[7, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0])
                        {
                          movingPiece = x;
                          action = 5;
                        }
                      }
                    }
                    if (actionPanel == generation.getPanels()[2, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0])
                    {
                      foreach (Piece x in generation.getPieces())
                      {
                        if (x.getPanel() == generation.getPanels()[0, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0])
                        {
                          movingPiece = x;
                          action = 3;
                        }
                      }
                    }
                    actionPanel = generation.getPanels()[action, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0];
                    movingPiecePanel = movingPiece.getPanel();
                    actionPanel.BackgroundImage =
                    movingPiece.getPanel().BackgroundImage;

                    undoPiece.Add(movingPiece);
                    undoPanel.Add(movingPiece.getPanel());
                    noUndos++;


                    movingPiece.setPanel(actionPanel);

                    generation.setPanelsInUse();
                    movingPiecePanel.BackgroundImage = null;
                    movingPiece.setMoved(true);
                    movingPiece.increaseNoMoves();

                    //moves the castle to thing other side of the king
                  }




                  if (movingPiece.getType() == "WPawn" ^
                      movingPiece.getType() == "BPawn")
                  {
                    if ((movingPiece.getPanel().Location.Y == 0 &&
                        movingPiece.getType() == "WPawn") ^
                        (movingPiece.getPanel().Location.Y == 280 &&
                        movingPiece.getType() == "BPawn"))
                    {
                      generation.RemovePiece(movingPiece);
                      newQueen = new Queen
                          (movingPiece.getType().Substring(0, 1) +
                          "Queen", movingPiece.getPanel(), true);
                      generation.AddPiece(newQueen);
                    }
                  }
                  //If a pawn reaches the end, it becoms a queen

                  int totalPossibleMoves = 0;
                  foreach (Piece y in generation.getPieces().ToList())
                  {
                    y.setMoves(generation, true, suicide);
                    if ((WhiteMove &&
                       y.getType().Substring(0, 1) == "B") ^
                       (!WhiteMove &&
                       y.getType().Substring(0, 1) == "W"))
                    {
                      totalPossibleMoves += y.getMoves().Count;
                    }

                  }
                  //Make sure there are some possible moves

                  if (!(gameModes == "M3S") && movingPiece.checkCheck(generation))
                  {
                    MessageBox.Show("Check!");
                    //Check check, if it is check, tell the players
                    if (totalPossibleMoves == 0)
                    {
                      //There are no possible moves, it's checkemate
                      timer1.Stop();
                      MessageBox.Show("Check mate! " + (movingPiece.checkType() == "B" ? "White" : "Black") + " wins!");
                      //EndGame()
                    }
                  }

                  double val = moveCalc.getBoardValue(generation);
                  WhiteMove = !WhiteMove;
                  movingPiece = null;
                  //Next players move and a piece has not been clicked
                  if (gameModes == "S1S")
                  {

                    generation.setPanelsInUse();
                    actionPanel = moveCalc.bestMove(generation, -10000, 10000);
                    movingPiece = moveCalc.getMovingPiece();
                    movingPiecePanel = moveCalc.getMovingPiecePanel();

                    if (generation.getPanelsInUse().Contains(actionPanel))
                    {
                      foreach (Piece z in generation.getPieces().ToList())
                      {
                        if (z.getPanel() == actionPanel)
                        {
                          generation.RemovePiece(z);
                          if (z.getType().Substring(0, 1) == "W")
                          {
                            pictureBoxesB[bPiecesTaken ].BackgroundImage = z.getPanel().BackgroundImage;
                            bPiecesTaken ++;

                          }
                          else
                          {
                            pictureBoxesW[wPiecesTaken ].BackgroundImage = z.getPanel().BackgroundImage;
                            wPiecesTaken ++;
                          }
                        }
                      }
                      actionPanel.BackgroundImage = null;
                      /*If the panel was a possible move and contains a
                      piece, the piece that it already contains needs
                      to be disposed of and stored in the interface where
                      taken pieces are displayed*/

                    }
                    if (movingPiece.getType().Contains("King"))
                    {
                      if ((actionPanel == generation.getPanels()[6, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0] || actionPanel == generation.getPanels()[2, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0]) && !movingPiece.getMoved())
                      {
                        castle = true;
                      }
                    }
                    //Recognises when the king is trying to castle


                    if ((WhiteMove &&
                        movingPiece.getType().Substring(0, 1).Equals("W")) ||
                       (!WhiteMove &&
                       movingPiece.getType().Substring(0, 1).Equals("B")))
                    {
                      actionPanel.BackgroundImage =
                      movingPiece.getPanel().BackgroundImage;

                      undoPiece.Add(movingPiece);
                      undoPanel.Add(movingPiece.getPanel());
                      noUndos++;

                      movingPiece.setPanel(actionPanel);

                      if (updateReq && movingPiece.getPanel() != updatePiece.getPanel())
                      {
                        updatePiece.setPanel(toUpdate);
                        generation.RemovePiece(piecePosRemove);
                        updateReq = false;
                        //if a pawn has moved foward two last move, it can no longer be taken as if it were on the first sqaure, so don't allow that to happen.
                      }
                      else if (updateReq)
                      {
                        if (movingPiece.getType().Substring(0, 1) == "W")
                        {
                          pictureBoxesW[wPiecesTaken  - 1].BackgroundImage = Resources.BPawn;
                        }
                        else
                        {
                          pictureBoxesB[bPiecesTaken  - 1].BackgroundImage = Resources.WPawn;
                        }
                        updatePiece.getPanel().BackgroundImage = movingPiece.getPanel().BackgroundImage;
                        piecePosRemove.getPanel().BackgroundImage = null;
                        generation.RemovePiece(piecePosRemove);
                        updatePiece.setPanel(null);

                        updateReq = false;

                        //If a pawn has moved foward two last move, and a pawn has taken it as if it were on the first square, this allows it to be taken.
                      }
                      if (movingPiece.getType().Contains("Pawn"))
                      {
                        if (!movingPiece.getMoved() && movingPiece.getPanel().Location.Y == movingPiecePanel.Location.Y + ((movingPiece.getType().Contains("W")) ? -80 : 80))
                        {
                          foreach (Panel x in generation.getPanels())
                          {
                            if (x.Location.Y + (movingPiece.getType().Contains("W") ? -40 : 40) == actionPanel.Location.Y && x.Location.X == actionPanel.Location.X)
                            {
                              toUpdate = actionPanel;
                              updatePiece = movingPiece;
                              movingPiece.setPanel(x);
                              updateReq = true;
                              piecePosRemove = new Pawn(updatePiece.getType(), actionPanel, true);
                              generation.AddPiece(piecePosRemove);
                              break;
                            }
                          }
                        }
                      }

                      generation.setPanelsInUse();
                      movingPiecePanel.BackgroundImage = null;
                      movingPiece.setMoved(true);
                      movingPiece.increaseNoMoves();

                      //As long as the right person (whose turn it is) is
                      //moving then this bit above moves it

                      if (castle)  //if the king is attempting to castle
                      {
                        int action = -1;
                        if (actionPanel == generation.getPanels()[6, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0])
                        {
                          foreach (Piece x in generation.getPieces())
                          {
                            if (x.getPanel() == generation.getPanels()[7, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0])
                            {
                              movingPiece = x;
                              action = 5;
                            }
                          }
                        }
                        if (actionPanel == generation.getPanels()[2, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0])
                        {
                          foreach (Piece x in generation.getPieces())
                          {
                            if (x.getPanel() == generation.getPanels()[0, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0])
                            {
                              movingPiece = x;
                              action = 3;
                            }
                          }
                        }
                        actionPanel = generation.getPanels()[action, movingPiece.getType().Substring(0, 1) == "W" ? 7 : 0];
                        movingPiecePanel = movingPiece.getPanel();
                        actionPanel.BackgroundImage =
                        movingPiece.getPanel().BackgroundImage;

                        undoPiece.Add(movingPiece);
                        undoPanel.Add(movingPiece.getPanel());
                        noUndos++;


                        movingPiece.setPanel(actionPanel);

                        generation.setPanelsInUse();
                        movingPiecePanel.BackgroundImage = null;
                        movingPiece.setMoved(true);
                        movingPiece.increaseNoMoves();

                        //moves the castle to thing other side of the king
                      }




                      if (movingPiece.getType() == "WPawn" ^
                          movingPiece.getType() == "BPawn")
                      {
                        if ((movingPiece.getPanel().Location.Y == 0 &&
                            movingPiece.getType() == "WPawn") ^
                            (movingPiece.getPanel().Location.Y == 280 &&
                            movingPiece.getType() == "BPawn"))
                        {
                          generation.RemovePiece(movingPiece);
                          newQueen = new Queen
                              (movingPiece.getType().Substring(0, 1) +
                              "Queen", movingPiece.getPanel(), true);
                          generation.AddPiece(newQueen);
                        }
                      }
                      //If a pawn reaches the end, it becoms a queen

                      totalPossibleMoves = 0;
                      foreach (Piece y in generation.getPieces().ToList())
                      {
                        y.setMoves(generation, true, suicide);
                        if ((WhiteMove &&
                           y.getType().Substring(0, 1) == "B") ^
                           (!WhiteMove &&
                           y.getType().Substring(0, 1) == "W"))
                        {
                          totalPossibleMoves += y.getMoves().Count;
                        }

                      }
                      //Make sure there are some possible moves

                      if (!(gameModes == "M3S") && movingPiece.checkCheck(generation))
                      {
                        MessageBox.Show("Check!");
                        //Check check, if it is check, tell the players
                        if (totalPossibleMoves == 0)
                        {
                          //There are no possible moves, it's checkemate
                          timer1.Stop();
                          MessageBox.Show("Check mate! " + (movingPiece.checkType() == "B" ? "White" : "Black") + " wins!");
                          //EndGame()
                        }
                      }

                      WhiteMove = !WhiteMove;
                      movingPiece = null;
                    }
                    else
                    {
                      movingPiece = null;
                    }
                  }

                }
              }
              else
              {
                movingPiece = null;
              }
            }
            catch (NullReferenceException)
            {
              movingPiece = null;
            }
            undos.Add(noUndos);
            noUndos = 0;

          }
        }
      }

    }

    private void timer1_Tick(object sender, EventArgs e)
    {

      //Each time the timer ticks
      if (timed) // if this is a timed game
      {
        if (WhiteMove) //if it is white's move
        {

          if (WhiteSeconds == 1)
          {
            label4.Text = "00:00";
            MessageBox.Show("White out of time. Black wins!");
            timer1.Stop();
            //If white is out of time, end the game and announce black as the winner
          }
          label4.ForeColor = Color.Red;
          label3.ForeColor = Color.White;
          int seconds = WhiteSeconds % 60;
          int minutes = WhiteSeconds / 60;
          string time = minutes.ToString("00") + ":" + seconds.ToString("00");
          label4.Text = time;
          WhiteSeconds--;

          //decrease the time and display the new time


        }
        else
        {

          if (BlackSeconds == 1)
          {
            label3.Text = "00:00";
            MessageBox.Show("Black out of time. White wins!");
            timer1.Stop();
            //If black is out of time, end the game and announce white as winner.
          }
          label3.ForeColor = Color.Red;
          label4.ForeColor = Color.White;
          int seconds = BlackSeconds % 60;
          int minutes = BlackSeconds / 60;
          string time = minutes.ToString("00") + ":" + seconds.ToString("00");
          label3.Text = time;
          BlackSeconds--;

          //decrease time and display new time


        }
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      MessageBox.Show("White had resigned. Black Wins!");
      timer1.Stop();
      //If the white has resigned, end then game and display the information
    }

    private void button2_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Black had resigned. White Wins!");
      timer1.Stop();
      //If the black has resigned, end then game and display the information
    }

    private void button3_Click(object sender, EventArgs e)
    {
      if (timed)
      {
        timer1.Stop();
      }
      MainMenu menu = new MainMenu();
      menu.Show();
      this.Hide();
      //If quit is pressed, go back to the main menu.
    }

    private void button4_Click(object sender, EventArgs e)
    {
      bool taken = false;

      try
      {
        for (int i = 0; i < undos[undos.Count - 1]; i++)
        {
          movingPiece = undoPiece[undoPiece.Count - (i + 1)];
          if (!generation.getPieces().Contains(movingPiece))
          {
            movingPiece.getPanel().BackgroundImage = movingPiece.getType().Contains("W") ? pictureBoxesB[bPiecesTaken  - 1].BackgroundImage : pictureBoxesW[wPiecesTaken  - 1].BackgroundImage;
            generation.AddPiece(movingPiece);
            if (movingPiece.getType().Contains("W"))
            {
              pictureBoxesB[bPiecesTaken - 1].BackgroundImage = null;
              bPiecesTaken --;
            }
            else
            {
              pictureBoxesB[wPiecesTaken  - 1].BackgroundImage = null;
              wPiecesTaken --;
            }
            //If there was a piece taken before the undo, remove it from the GUI
            taken = true;
          }
          if (movingPiece == updatePiece)
          {
            piecePosRemove.getPanel().BackgroundImage = null;
            piecePosRemove.setPanel(null);
            generation.RemovePiece(piecePosRemove);
            toUpdate = null;
            updatePiece = null;
            movingPiece.getPanel().BackgroundImage = movingPiece.getType().Contains("W") ? Resources.WPawn : Resources.BPawn;
            //Prepare the sqaure the piece needs to return to if necesary
          }

          movingPiecePanel = undoPiece[undoPiece.Count - (i + 1)].getPanel();
          actionPanel = undoPanel[undoPanel.Count - (i + 1)];
          actionPanel.BackgroundImage = movingPiece.getPanel().BackgroundImage;

          movingPiece.setPanel(actionPanel);
          generation.setPanelsInUse();
          movingPiecePanel.BackgroundImage = null;
          movingPiece.decreaseNoMoves();
          if (movingPiece.numberMoves() <= 0)
          {
            movingPiece.setMoved(false);
          }
          //return everything to the way it was before the move occured
        }
        for (int x = 0; x < undos[undos.Count - 1]; x++)
        {
          undoPiece.Remove(undoPiece[undoPiece.Count - 1]);
          undoPanel.Remove(undoPanel[undoPanel.Count - 1]);
        }

        undos.Remove(undos[undos.Count - 1]);

        if (updateReq || taken)
        {
          panel_MouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
          updateReq = false;
        }
        //update the undos so that it wont be the same undo over and over again
      }
      catch (System.ArgumentOutOfRangeException)
      {
        MessageBox.Show("You can't go back any further.");
        //if there aren't any undos in the list, you can't go back more
      }
      noUndos = 0;
    }
  }
}
