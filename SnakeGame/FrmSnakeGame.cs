using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class FrmSnakeGame : Form
    {
        private const int sixeField = 30;
        private const int rowsNumber = 15;
        private const int ColumnsNumber = 15;
        private const int fieldLeftOffset = 40;
        private const int fieldTopOffset = 15;
        private const int InitialSnakeSpeedInterval = 300;
        private const int speedSnakeIncrement = 5;

        private enum SnakeDirection
        {
            Left, Right, Up, Down
        }

        private SnakeDirection snakeDirection = SnakeDirection.Up;
        private LinkedList<Point> snake = new LinkedList<Point>();
        private Point food;
        private Random rand = new Random();
        private bool isGameEnded;
        private bool isGamePaused;
        private int points = 0;
        private int foodEaten = 0;

        public FrmSnakeGame()
        {
            InitializeComponent();
        }

        private void InitializeSnake()
        {
            snakeDirection = SnakeDirection.Up;
            snake.Clear();
            snake.AddFirst(new Point(rowsNumber - 1, ColumnsNumber / 2 - 1));
        }

        private void MoveSnake()
        {
            LinkedListNode<Point> head = snake.First;
            Point newHead = new Point(0, 0);
            switch (snakeDirection)
            {
                case SnakeDirection.Left:
                    newHead = new Point(head.Value.X, head.Value.Y - 1);
                    break;
                case SnakeDirection.Right:
                    newHead = new Point(head.Value.X, head.Value.Y + 1);
                    break;
                case SnakeDirection.Down:
                    newHead = new Point(head.Value.X + 1, head.Value.Y);
                    break;
                case SnakeDirection.Up:
                    newHead = new Point(head.Value.X - 1, head.Value.Y);
                    break;
            }

            if (snake.Any(point => point.X == newHead.X && point.Y == newHead.Y))
            {
                Invalidate();
                GameOver();
                return;
            }

            snake.AddFirst(newHead);

            if (newHead.X == food.X && newHead.Y == food.Y)
            {
                AddPlayerPoints();
                foodEaten++;
                GenerateFood();
            }
            else
            {
                snake.RemoveLast();
            }
        }

        private void ChangeSnakeDirection(SnakeDirection restrictedDirection, SnakeDirection newDirection)
        {
            if (snakeDirection != restrictedDirection)
            {
                snakeDirection = newDirection;
            }
        }

        private void GenerateFood()
        {
            bool isFoodClashWithSnake;
            do
            {
                food = new Point(rand.Next(0, rowsNumber), rand.Next(0, ColumnsNumber));
                isFoodClashWithSnake = false;
                foreach (Point p in snake)
                {
                    if (p.X == food.X && p.Y == food.Y)
                    {
                        isFoodClashWithSnake = true;
                        break;
                    }
                }
            }
            while (isFoodClashWithSnake);
            timerforGame.Interval -= speedSnakeIncrement;
        }

        private void FrmSnakeGame_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            BackColor = Color.Black;
            FirsStart();
        }

        private void FirsStart()
        {
            if (MessageBox.Show("Начнем игру?", "Старт", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                StartGame();
            }
        }
        private void StartGame()
        {
            GenerateFood();
            InitializeSnake();
            isGameEnded = false;
            timerforGame.Start();
            timerforGame.Interval = InitialSnakeSpeedInterval;
            points = 0;
        }

        private void GameOver()
        {
            isGameEnded = true;
            timerforGame.Stop();
            if (MessageBox.Show("Конец игры! Начать заново?", "Конец игры", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                StartGame();
            }
        }

        private bool IsGameOver()
        {
            LinkedListNode<Point> head = snake.First;
            switch (snakeDirection)
            {
                case SnakeDirection.Left:
                    return head.Value.Y - 1 < 0;
                case SnakeDirection.Right:
                    return head.Value.Y + 1 >= ColumnsNumber;
                case SnakeDirection.Down:
                    return head.Value.X + 1 >= rowsNumber;
                case SnakeDirection.Up:
                    return head.Value.X - 1 < 0;
            }
            return false;
        }

        private void PauseOrUnpauseGame()
        {
            if (!isGamePaused)
            {
                timerforGame.Stop();
                timerforGame.Stop();
                Invalidate();
            }
            else
            {
                timerforGame.Start();
                timerforGame.Start();
            }
            isGamePaused = !isGamePaused;
        }

        private void AddPlayerPoints()
        {
            if (food.X == 0 && food.Y == 0 || food.X == rowsNumber - 1 && food.Y == 0 || food.X == rowsNumber - 1 && food.Y == ColumnsNumber - 1 || food.X == 0 && food.Y == ColumnsNumber - 1)
            {
                points += 10;
            }
            else if (food.X == 0 || food.X == rowsNumber - 1 || food.Y == 0 || food.Y == ColumnsNumber - 1)
            {
                points += 5;
            }
            else
            {
                points += 1;
            }
        }

        private void FrmSnakeGame_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawGrid(g);
            DrawFood(g);
            DrawSnake(g);
            DrawStatsAndKeyboardHints(g);
        }

        private void DrawGrid(Graphics g)
        {
            for (int row = 0; row <= rowsNumber; row++)
            {
                g.DrawLine(Pens.Cyan,
                    new Point(fieldLeftOffset, fieldTopOffset + row * sixeField),
                    new Point(fieldLeftOffset + sixeField * rowsNumber, fieldTopOffset + row * sixeField)
                );

                for (int col = 0; col <= ColumnsNumber; col++)
                {
                    g.DrawLine(Pens.Cyan,
                        new Point(fieldLeftOffset + col * sixeField, fieldTopOffset),
                        new Point(fieldLeftOffset + col * sixeField, fieldTopOffset + sixeField * ColumnsNumber)
                    );
                }
            }
        }

        private void DrawSnake(Graphics g)
        {
            foreach (Point p in snake)
            {
                g.FillRectangle(Brushes.Lime, new Rectangle(
                    fieldLeftOffset + p.Y * sixeField + 1,
                    fieldTopOffset + p.X * sixeField + 1,
                    sixeField - 1,
                    sixeField - 1));
            }
        }

        private void DrawFood(Graphics g)
        {
            g.FillRectangle(Brushes.Red, new Rectangle(
                fieldLeftOffset + food.Y * sixeField + 1,
                fieldTopOffset + food.X * sixeField + 1,
                sixeField - 1,
                sixeField - 1));
        }

        private void TimerforGame_Tick(object sender, EventArgs e)
        {
            if (IsGameOver())
            {
                GameOver();
            }
            else
            {
                MoveSnake();
                Invalidate();
            }
        }

        private void DrawStatsAndKeyboardHints(Graphics g)
        {
            Font fontStats = new Font("Consolas", 14);
            int statsLeftOffset = fieldLeftOffset + sixeField * ColumnsNumber + 10;
            g.DrawString(string.Format("Длина змейки: {0}", snake.Count), fontStats, Brushes.Lime, new Point(statsLeftOffset, 10));
            g.DrawString(string.Format("Очки: {0}", points), fontStats, Brushes.Goldenrod, new Point(statsLeftOffset, 30));
            g.DrawString(string.Format("Еды съедено: {0}", foodEaten), fontStats, Brushes.Crimson, new Point(statsLeftOffset, 50));

            g.DrawString("Управление:", fontStats, Brushes.White, new Point(statsLeftOffset, 160));
            g.DrawString("Вверх: ↑ или W", fontStats, Brushes.White, new Point(statsLeftOffset, 190));
            g.DrawString("Вниз:  ↓ или S", fontStats, Brushes.White, new Point(statsLeftOffset, 210));
            g.DrawString("Влево: ← или A", fontStats, Brushes.White, new Point(statsLeftOffset, 230));
            g.DrawString("Влево: → или D", fontStats, Brushes.White, new Point(statsLeftOffset, 250));
            g.DrawString("Пауза: [Space]", fontStats, Brushes.White, new Point(statsLeftOffset, 270));
            g.DrawString("Старт: [Space]", fontStats, Brushes.White, new Point(statsLeftOffset, 290));
            g.DrawString("Выход: [Escape]", fontStats, Brushes.White, new Point(statsLeftOffset, 310));

            if (isGamePaused)
            {
                g.DrawString("Игра на паузе...", fontStats, Brushes.Yellow, new Point(statsLeftOffset, 350));
            }
            fontStats.Dispose();
        }

        private void FrmSnakeGame_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    ChangeSnakeDirection(SnakeDirection.Right, SnakeDirection.Left);
                    break;
                case Keys.Right:
                case Keys.D:
                    ChangeSnakeDirection(SnakeDirection.Left, SnakeDirection.Right);
                    break;
                case Keys.Down:
                case Keys.S:
                    ChangeSnakeDirection(SnakeDirection.Up, SnakeDirection.Down);
                    break;
                case Keys.Up:
                case Keys.W:
                    ChangeSnakeDirection(SnakeDirection.Down, SnakeDirection.Up);
                    break;
                case Keys.Escape:
                    timerforGame.Stop();
                    Close();
                    break;
                case Keys.Space:
                    if (isGameEnded && !timerforGame.Enabled)
                    {
                        StartGame();
                    }
                    else
                    {
                        PauseOrUnpauseGame();
                    }
                    break;
            }
        }
    }
}
