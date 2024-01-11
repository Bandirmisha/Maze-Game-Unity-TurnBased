
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class Arrow
    {
        private Model model => Model.inst;

        public Vector3 currentPosition;

        public Vector3 direction { get; set; }

        public Arrow(Vector3 startPos)
        {
            currentPosition = startPos;
            SetRandomDirection();
        }

        private void SetRandomDirection()
        {
            List<Vector3> directions = new()
            {
                Vector3.back,
                Vector3.forward,
                Vector3.left,
                Vector3.right,
            };

            List<Vector3> possibleArrowDirection = new();
            for (int i = 0; i < directions.Count; i++)
            {
                Vector3 tempPos = currentPosition + directions[i];
                if (tempPos.x >= 0 && tempPos.z * (-1) >= 0 && tempPos.x < model.mazeWidth && tempPos.z * (-1) < model.mazeHeight)
                {
                    if (model.Field[(int)tempPos.x, (int)tempPos.z * (-1)].type == CellType.Floor)
                    {
                        possibleArrowDirection.Add(directions[i]);
                    }
                }
            }

            direction = possibleArrowDirection[Random.Range(0, possibleArrowDirection.Count)];
        }

        public void Move()
        {
            Vector3 buf = currentPosition + direction/20;

            if (buf.x >= 0 && buf.z*(-1) >= 0)
            {
                if (Vector3.Distance(currentPosition, model.playerCurrentPosition) < 0.05f)
                {
                    model.PlayerTakeDamage(5);
                    currentPosition = new Vector3 (-1, -1, -1);
                }
                else if (model.Field[(int)buf.x, (int)buf.z*(-1)].type == CellType.Floor)
                {
                    currentPosition = buf;
                }
                else
                {
                    currentPosition = new Vector3(-1, -1, -1);
                }
            }

            
        }
    }
}
