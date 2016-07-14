using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        enum GameStates
        {
            Stopped = 0,
            Started = 1
        }

        private GameStates GameState { get; set; }

        private Field field;
        private Field.FieldValue currFigure = Field.FieldValue.X;

        const int BUTTON_SIZE = 30;
        const int BUTTON_SPACE = 5;

        private Button[,] buttons;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "X&0";

            GameState = GameStates.Stopped;
        }

        private void DrawButtons()
        {
            if (buttons == null || buttons.Length != field.FieldSize * field.FieldSize)
            {
                if (buttons != null)
                {
                    foreach (Button btn in buttons)
                        this.Controls.Remove(btn);
                }
                buttons = new Button[field.FieldSize, field.FieldSize];  

                this.Width = 145 + field.FieldSize * (BUTTON_SIZE + BUTTON_SPACE);
                this.Height = Math.Max(250, field.FieldSize * (BUTTON_SIZE + BUTTON_SPACE));
            }

            for (int i = 0; i < field.FieldSize; i++)
                for (int j = 0; j < field.FieldSize; j++)
                {
                    if (buttons[i, j] == null)
                    {
                        var b = new Button();
                        b.Name = "btnCell" + i + j;
                        b.Size = new Size(BUTTON_SIZE, BUTTON_SIZE);
                        b.Left = j * (BUTTON_SPACE + BUTTON_SIZE);
                        b.Top = i * (BUTTON_SPACE + BUTTON_SIZE);
                        this.Controls.Add(b);
                        buttons[i, j] = b;
                        b.Tag = new Cell(i, j);
                        b.Click += btnCells_Click;
                    }
                    else
                    {
                        buttons[i, j].Enabled = true;
                    }
                    buttons[i, j].Text = field.GetCellValue(new Cell(i, j)).ToString();
                }
        }

        private void btnCells_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;

            var cell = (Cell)b.Tag;
            field.DoStep(cell, currFigure);
            b.Text = field.GetCellValue(cell).ToString();
            b.Enabled = false;

            if (CheckGame())
            {
                currFigure = field.ChancheFieldValue(currFigure);
                DoComputerStep();
            }
        }

        private void DoComputerStep()
        {
            var cell = field.GetCellByComputer(currFigure);
            field.DoStep(cell, currFigure);
            buttons[cell.I, cell.J].Text = field.GetCellValue(cell).ToString();
            buttons[cell.I, cell.J].Enabled = false;

            if (CheckGame())
            {
                currFigure = field.ChancheFieldValue(currFigure);
            }
        }

        private bool CheckGame()
        {
            if (field.AreAllCellsFilldIn(currFigure))
            {
                MessageBox.Show("GameOver");
                return false;
            }
            return true;
        }

        public int FieldSize
        {
            get
            {
                return int.Parse(txtSize.Text);
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            switch (GameState)
            {
                case GameStates.Stopped:
                    // старт
                    btnRestart.Text = "Stop";
                    txtSize.Enabled = false;

                    int newsize = (int)txtSize.Value; 
                    if (field == null)
                    {
                        field = new Field(newsize);
                    }
                    else
                    {
                        field.FieldSize = newsize;
                    }

                    DrawButtons();
                    break;

                case GameStates.Started:
                    // стоп
                    btnRestart.Text = "Start";
                    txtSize.Enabled = true;
                    break;
            }

            GameState = GameState == GameStates.Started ? GameStates.Stopped : GameStates.Started;
        }
    }
}


