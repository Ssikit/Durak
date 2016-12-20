using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Minesweeper
{
    class Board
    {
        private Cell[,] board; 

        public Cell[,] GetBoard { get { return board; } }

        private int height, width;

        public int Height { get { return height; } }

        public int Width { get { return width; } }

        public int bombs;

        public int userBombsLeft; 

        public bool isWin { get; set; }

        private int cellsCount; 

        public bool IsGameOver { get; set; }

        public Board(int bombsCount, int width, int height)
        {
            InitializeBoard(bombsCount, width, height);
        }

        public void InitializeBoard(int bombsCount, int width, int height)
        {
            cellsCount = width * height; // присваиваем количетсво клеток на поле 

            this.height = height;
            this.width = width;

            userBombsLeft = bombsCount;
            bombs = bombsCount;

            board = new Cell[height, width];

            for (int i = 0; i < board.GetLength(0); i++) // i - по вертикали , J - по горизонтали
                for (int j = 0; j < board.GetLength(1); j++)
                    board[i, j] = new Cell(j, i);

            Random rnd = new Random(); 

            int x = 0;
            int y = 0;

            for (int i = 0; i < bombs; i++)
            {
                x = rnd.Next(0, board.GetLength(0));
                y = rnd.Next(0, board.GetLength(1));
                if (board[x, y].IsMine)
                    i--;
                else
                    board[x, y].IsMine = true;
            }

            for (int i = 0; i < board.GetLength(0); i++) 
                for (int j = 0; j < board.GetLength(1); j++)
                    board[i, j].nearMines = CloseMines(j, i);
        }

        private int CloseMines(int x, int y) 
        {
            int minx, miny, maxx, maxy;
            int result = 0;

            int width = Properties.Settings.Default.width;
            int height = Properties.Settings.Default.height;

            minx = (x <= 0 ? 0 : x - 1); 
            miny = (y <= 0 ? 0 : y - 1);
            maxx = (x >= width - 1 ? width : x + 2);
            maxy = (y >= height - 1 ? height : y + 2);

            for (int i = miny; i < maxy; i++)
                for (int j = minx; j < maxx; j++)
                    if (board[i, j].IsMine)
                        result++;
            return result;
        }


        public void LeftClick(Cell cell)
        {
            if (!cell.IsClick)
            {
                cell.IsClick = true;
                if (cell.IsMine)
                {
                    IsGameOver = true;
                }
                else
                {
                    cellsCount--;
                    if (cellsCount == bombs)
                        isWin = true;

                    int width = Properties.Settings.Default.width;
                    int height = Properties.Settings.Default.height;
                    int minx = (cell.x <= 0 ? 0 : cell.x - 1);  
                    int miny = (cell.y <= 0 ? 0 : cell.y - 1);
                    int maxx = (cell.x >= width - 1 ? width : cell.x + 2);
                    int maxy = (cell.y >= height - 1 ? height : cell.y + 2);


                    for (int i = miny; i < maxy; i++)
                        for (int j = minx; j < maxx; j++)
                        {
                            if (board[i, j].nearMines == 0)
                                LeftClick(board[i, j]);
                            if (cell.nearMines == 0)
                                LeftClick(board[i, j]);
                        }
                }
            }

        }

        public void GameOver() 
        {
            foreach (Cell cell in board)
                if (cell.IsMine)
                    cell.IsClick = true;
        }




        public void RightClick(Cell cell)
        {
            if (!cell.IsClick)
            {
                cell.IsFlag = !cell.IsFlag;
                userBombsLeft = cell.IsFlag ? userBombsLeft - 1 : userBombsLeft + 1;
            }
        }

        public void FirstClick(Cell cell)
        {
            if (cell.IsMine)
            {
            }
        }
    }
}
