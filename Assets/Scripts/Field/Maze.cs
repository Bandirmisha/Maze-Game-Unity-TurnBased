﻿using UnityEngine;
using System.Collections.Generic;

namespace MazeGame
{
    public class Maze
    {
        public Cell[,] field { get; set; }
        public int width;
        public int height;

        public Vector3 keyPos;
        public Vector3 exitPos;

        public Maze()
        {
            width = Random.Range(35, 50);
            height = Random.Range(15, 25);
            if (width % 2 == 0) { width++; }
            if (height % 2 == 0) { height++; }

            field = new Cell[width, height];
            keyPos = new Vector3(0, 0, 0);
            exitPos = new Vector3(0, 0, 0);

            Generate();
        }

        private void Generate()
        {
            Stack<Cell> backTrack = new();
            Cell currentCell = new(1, 1, CellType.Floor);
            Cell neighbourCell;
            
            // Заполнение
            for (int j = 0; j < height; j++)
            {
                for(int i = 0; i < width; i++)
                {
                    //Создание границ
                    if (i == 0 || j == 0 || i == width - 1 || j == height - 1 || i % 2 == 0 || j % 2 == 0)
                    {
                        field[i, j] = new(i, j, CellType.Wall);
                    }
                    else field[i, j] = new(i, j, CellType.FloorCheckbox);
                }
            }

            //Начальная точка посещена
            field[1, 1] = currentCell;
            backTrack.Push(currentCell);

            // Нахождение кол-ва всех непосещенных соседей
            int a = (width - 1) / 2;
            int b = (height - 1) / 2;
            int count = a * b - 1;

            //Генерация
            int deadEndCount = 0;
            int counter = 0;
            while (counter != count)
            {
                //Нахождение соседей
                List<Cell> neigbours = getNeigbours(width, height, currentCell);

                //Удаление посещенных соседей
                for (int i = 0; i < neigbours.Count; i++)
                {
                    int x = neigbours[i].X;
                    int y = neigbours[i].Y;

                    if (field[x, y].type == CellType.Floor)
                    {
                        neigbours.RemoveAt(i);

                        if(i>=0) { i--; }
                    }
                }

                if (neigbours.Count > 0)
                {
                    //Выбор случайного соседа
                    neighbourCell = neigbours[Random.Range(0, neigbours.Count)];

                    //Сосед отмечен посещенным
                    field[neighbourCell.X, neighbourCell.Y].type = CellType.Floor;

                    //Соединение
                    if (neighbourCell.X > currentCell.X)
                        field[neighbourCell.X - 1, currentCell.Y].type = CellType.Floor;
                    else if (neighbourCell.X < currentCell.X)
                        field[currentCell.X - 1, currentCell.Y].type = CellType.Floor;
                    else if (neighbourCell.Y > currentCell.Y)
                        field[currentCell.X, neighbourCell.Y - 1].type = CellType.Floor;
                    else if (neighbourCell.Y < currentCell.Y)
                        field[currentCell.X, currentCell.Y - 1].type = CellType.Floor;

                    //Отслеживание пути следования
                    backTrack.Push(neighbourCell);

                    //Смещение "указателя"
                    currentCell = neighbourCell;

                    counter++;


                    if (deadEndCount == 1 || deadEndCount == 3 || deadEndCount == 5)
                    {
                        deadEndCount++;
                    }
                   

                }
                else
                {
                    if (backTrack.Count > 0)
                    {
                        //Создание ключа и выхода
                        if (keyPos.x == 0 && keyPos.y == 0 && deadEndCount == 0)
                        {
                            deadEndCount++;
                            keyPos = new(currentCell.X, 0, -currentCell.Y);
                        }
                        else
                        {
                            if (exitPos.x == 0 && exitPos.y == 0 && deadEndCount == 6)
                            {
                                exitPos = new(currentCell.X, 0, -currentCell.Y);
                            }
                        }

                        if (deadEndCount == 2 || deadEndCount == 4)
                        {
                            deadEndCount++;
                        }
                       
                        currentCell = backTrack.Pop();
                    }
                }
            }

            if (exitPos.x == 0 && exitPos.y == 0)
            {
                exitPos = new(currentCell.X, currentCell.Y);
            }
        }

        private List<Cell> getNeigbours(int width, int height, Cell currCell)
        {
            List<Cell> neigbours = new List<Cell>();

            int[,] direction =
            {
                {-2,0},
                {0,2},
                {2,0},
                {0,-2}
            };

            //Проход по всем направлениям
            for (int i = 0; i < 4; i++)
            {
                Cell bufCell = currCell;

                bufCell.X += direction[i, 0];
                bufCell.Y += direction[i, 1];

                if (bufCell.X < 0 || bufCell.X >= width || bufCell.Y < 0 || bufCell.Y >= height)
                { }
                else neigbours.Add(bufCell);
            }

            return neigbours;
        }
    }
}
