
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MazeGame
{
    public class Arrow : MonoBehaviour
    {
        private Field field => ViewModel.instance.field;

        [HideInInspector] public Vector3 direction { get; set; }

        private void OnEnable()
        {
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
                Vector3 tempPos = transform.position + directions[i];

                if (tempPos.x >= 0 && tempPos.z * (-1) >= 0 && tempPos.x < field.width && tempPos.z * (-1) < field.height)
                {
                    if (field.field[(int)tempPos.x, (int)tempPos.z * (-1)].type == CellType.Floor)
                    {
                        possibleArrowDirection.Add(directions[i]);
                    }
                }
            }

            direction = Random.Range(0, possibleArrowDirection.Count) switch
            {
                0 => possibleArrowDirection[0],
                1 => possibleArrowDirection[1],
                2 => possibleArrowDirection[2],
                3 => possibleArrowDirection[3],
                _ => throw new System.NotImplementedException(),
            };
        }


        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            transform.position += direction * 4f * Time.fixedDeltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Если игрок
            if (collision.gameObject.layer == 7)
            {
                ViewModel.instance.playerModel.TakeDamage(5);
            }
            
            Destroy(this.gameObject);
        }
    }
}
