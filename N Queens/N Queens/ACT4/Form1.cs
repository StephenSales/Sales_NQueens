using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;

namespace ACT4
{
    public partial class Form1 : Form
    {
        int side;
        int n = 6;
        SixState startState;
        SixState currentState;
        int moveCounter;

        //bool stepMove = true;

        public Form1()
        {
            InitializeComponent();

            side = pictureBox1.Width / n;

            startState = randomSixState();
            currentState = new SixState(startState);

            updateUI();
            label1.Text = "Attacking pairs: " + getAttackingPairs(startState);
            label2.Text = "A* Search Algorithm";
        }

        private void updateUI()
        {
            //pictureBox1.Refresh();
            pictureBox2.Refresh();

            //label1.Text = "Attacking pairs: " + getAttackingPairs(startState);
            label3.Text = "Attacking pairs: " + getAttackingPairs(currentState);
            label4.Text = "Moves: " + moveCounter;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // draw squares
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Blue, i * side, j * side, side, side);
                    }
                    // draw queens
                    if (j == startState.Y[i])
                        e.Graphics.FillEllipse(Brushes.Fuchsia, i * side, j * side, side, side);
                }
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            // draw squares
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, i * side, j * side, side, side);
                    }
                    // draw queens
                    if (j == currentState.Y[i])
                        e.Graphics.FillEllipse(Brushes.Fuchsia, i * side, j * side, side, side);
                }
            }
        }

        private SixState randomSixState()
        {
            Random r = new Random();
            SixState random = new SixState(r.Next(n),
                                             r.Next(n),
                                             r.Next(n),
                                             r.Next(n),
                                             r.Next(n),
                                             r.Next(n));

            return random;
        }

        private int getAttackingPairs(SixState f)
        {
            int attackers = 0;
            for (int rf = 0; rf < n; rf++)
            {
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // horizontal attackers
                    if (f.Y[rf] == f.Y[tar]) attackers++;
                }
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // diagonal down attackers
                    if (f.Y[tar] == f.Y[rf] + tar - rf) attackers++;
                }
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // diagonal up attackers
                    if (f.Y[rf] == f.Y[tar] + tar - rf) attackers++;
                }
            }
            return attackers;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not applicable for A* search algorithms");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            startState = randomSixState();
            currentState = new SixState(startState);

            moveCounter = 0;

            updateUI();
            pictureBox1.Refresh();
            label1.Text = "Attacking pairs: " + getAttackingPairs(startState);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PriorityQueue openSet = new PriorityQueue();
            Dictionary<SixState, int> gScore = new Dictionary<SixState, int>(); // stores g values

            // Initialize the starting state
            openSet.Enqueue(currentState, getAttackingPairs(currentState));
            gScore[currentState] = 0;
            moveCounter = 0;

            while (!openSet.IsEmpty())
            {
                // Get the current state with the lowest f = g + h score
                SixState current = openSet.Dequeue();

                // Check if we have reached the goal (no attacking pairs)
                if (getAttackingPairs(current) == 0)
                {
                    currentState = current;
                    updateUI();
                    MessageBox.Show("Solution found!");
                    return;
                }

                // Explore all possible moves from this state
                foreach (Point move in GetAllPossibleMoves(current))
                {
                    SixState neighbor = applyMove(new SixState(current), move);
                    int tentative_gScore = gScore[current] + 1; // Move cost is 1

                    // If this path is better than any previous one
                    if (!gScore.ContainsKey(neighbor) || tentative_gScore < gScore[neighbor])
                    {
                        gScore[neighbor] = tentative_gScore;
                        int fScore = tentative_gScore + getAttackingPairs(neighbor); // f = g + h
                        openSet.Enqueue(neighbor, fScore);

                        // Update the UI and move count for each move
                        moveCounter++;
                        currentState = neighbor;
                        updateUI();
                    }
                }
            }

            MessageBox.Show("No solution found!");
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private SixState applyMove(SixState state, Point move)
        {
            state.Y[move.X] = move.Y;
            return state;
        }

        private List<Point> GetAllPossibleMoves(SixState state)
        {
            List<Point> moves = new List<Point>();
            for (int i = 0; i < n; i++) // For each column
            {
                for (int j = 0; j < n; j++) // Try placing a queen in each row of that column
                {
                    if (state.Y[i] != j) // Avoid placing a queen where it already is
                        moves.Add(new Point(i, j));
                }
            }
            return moves;
        }


    }
}
