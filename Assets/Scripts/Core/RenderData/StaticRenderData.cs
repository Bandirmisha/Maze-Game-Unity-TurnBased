using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class StaticRenderData
    {
        public List<Vector3> wallsPositions;
        public List<Vector3> floorsPositions;

        public Vector3 keyPosition;
        public Vector3 exitPosition;

        public StaticRenderData(int width,
                                int height,
                                Cell[,] field,
                                Vector3 keyPos,
                                Vector3 exitPos)
        {
            wallsPositions = new List<Vector3>();
            floorsPositions = new List<Vector3>();

            InitObjectsPositions(width, height, field);

            keyPosition = keyPos;
            exitPosition = exitPos;
        }

        private void InitObjectsPositions(int width, int height, Cell[,] field)
        {
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    //Стены
                    if (field[i, j].type == CellType.Wall)
                    {
                        wallsPositions.Add(new Vector3(i, 0, -j));
                    }
                    //Проход
                    else if (field[i, j].type == CellType.Floor)
                    {
                        floorsPositions.Add(new Vector3(i, 0, -j));
                    }
                }
            }
        }

        
    }

}
