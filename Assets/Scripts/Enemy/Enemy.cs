using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

namespace MazeGame
{
    public class Enemy : IEnemy
    {
        private PlayerModel player => ViewModel.instance.playerModel;

        public Vector3 targetPosition { get; set; }
        public Vector3 currentPosition { get; private set; }
        private Vector3 AnimShift { get; set; }
        public int HP { get; set; }
        protected bool alive { get; set; }
        private bool isMoving { get; set; }
        private bool canAttack { get; set; }

        private float attackCooldown { get; }
        private float currentTime { get; set; }

        public Enemy()
        {
            Vector3 startPos = GetStartPos();
            currentPosition = startPos;
            targetPosition = startPos;

            HP = 5;
            alive = true;

            attackCooldown = 1f;
            canAttack = true;
        }

        public virtual void Event()
        {
            if(!alive)
            {
                return;
            }

            if (!canAttack)
            {
                currentTime += Time.fixedDeltaTime;
                if (currentTime >= attackCooldown)
                {
                    currentTime = 0;
                    canAttack = true;
                }
            }


            if (isPlayerNearby() && !isMoving)
            {
                if (canAttack)
                    Attack();
                return;
            }

            if (isMoving)
                Move();
            else
                SetRandomDestination();
        }

        private Vector3 GetStartPos()
        {
            Vector3 vec;

            do
            {
                vec = new Vector3(UnityEngine.Random.Range(3,ViewModel.instance.field.width), 0, -UnityEngine.Random.Range(3, ViewModel.instance.field.height));
            }
            while (ViewModel.instance.field.field[(int)vec.x, -(int)vec.z].type == CellType.Wall);

            return vec;
        }

        public void Move()
        {
            if (Vector3.Distance(currentPosition, targetPosition) > 0.05f)
            {
                currentPosition += AnimShift;
            }
            else
            {
                currentPosition = targetPosition;
                isMoving = false;
            }
        }

        public void Attack()
        {
            canAttack = false;
            ViewModel.instance.playerModel.TakeDamage(2);
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;

            if (HP <= 0)
            {
                alive = false;

                currentPosition = new Vector3(0, -1, 0);
                targetPosition = currentPosition;

                return;
            }
        }

        private bool isPlayerNearby()
        {
            if (player.targetPosition.x == targetPosition.x - 1 && player.targetPosition.z == targetPosition.z ||
                player.targetPosition.x == targetPosition.x && player.targetPosition.z == targetPosition.z - 1 ||
                player.targetPosition.x == targetPosition.x + 1 && player.targetPosition.z == targetPosition.z ||
                player.targetPosition.x == targetPosition.x && player.targetPosition.z == targetPosition.z + 1)
                return true;
            else
                return false;
        }

        private void SetRandomDestination()
        {
            List<Vector3> directions = new()
            {
                Vector3.back,
                Vector3.forward,
                Vector3.left,
                Vector3.right,
            };

            List<Vector3> possibleDirection = new();
            for (int i = 0; i < directions.Count; i++)
            {
                if (CheckDestination(directions[i]))
                    possibleDirection.Add(directions[i]);
            }

            if (possibleDirection.Count > 0)
            {
                var direction = Random.Range(0, possibleDirection.Count) switch
                {
                    0 => possibleDirection[0],
                    1 => possibleDirection[1],
                    2 => possibleDirection[2],
                    3 => possibleDirection[3],
                    _ => throw new System.NotImplementedException(),
                };

                isMoving = true;
                targetPosition += direction;
                AnimShift = new Vector3(direction.x * 0.03f, direction.y * 0.03f, direction.z * 0.03f);
            }

        }

        private bool CheckDestination(Vector3 direction)
        {
            Vector3 tempPos = targetPosition + direction;

            if (ViewModel.instance.field.field[(int)tempPos.x, (int)tempPos.z * (-1)].type == CellType.Floor)
            {
                if (ViewModel.instance.zombies.Concat(ViewModel.instance.skeletons).Any(x => x.targetPosition == tempPos) ||
                    ViewModel.instance.playerModel.targetPosition == tempPos)
                    return false;

                return true;
            }

            return false;
        } 
    }
}
