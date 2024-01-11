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
        protected Model model => Model.inst;

        public Vector3 targetPosition { get; set; }
        public Vector3 currentPosition { get; private set; }
        private Vector3 AnimShift { get; set; }
        public int HP { get; set; }
        protected bool alive { get; set; }
        private bool isMoving { get; set; }
        private bool canAttack { get; set; }

        private float attackCooldown { get; }
        private float currentTime { get; set; }

        public Enemy(Vector3 startPos)
        {
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
                currentTime += Time.deltaTime;
                if (currentTime >= attackCooldown)
                {
                    currentTime = 0;
                    canAttack = true;
                }
            }

            if (model.isPlayerNearby(targetPosition) && !isMoving)
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
            model.PlayerTakeDamage(2);
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
                if (model.CheckDestination(targetPosition,directions[i]))
                    possibleDirection.Add(directions[i]);
            }

            if (possibleDirection.Count > 0)
            {
                var direction = possibleDirection[Random.Range(0, possibleDirection.Count)];

                isMoving = true;
                targetPosition += direction;
                AnimShift = new Vector3(direction.x * 0.01f, direction.y * 0.01f, direction.z * 0.01f);
            }

        }

        
    }
}
