using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace MazeGame
{
    public class Enemy : IEnemy
    {
        private PlayerModel player => ViewModel.instance.playerModel;

        public Vector3 targetPosition;
        public Vector3 currentPosition;
        private Vector3 animShift;
        public int HP;
        protected bool alive;
        private bool isMoving;
        private bool canAttack;

        [SerializeField] private float attackCooldown;
        private float currentTime;

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

        public void Move()
        {
            if (Vector3.Distance(currentPosition, targetPosition) > 0.05f)
            {
                currentPosition += animShift;
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
            int iter = 0;
            while (iter<10)
            {
                Vector3 direction = Random.Range(0, 4) switch
                {
                    0 => new Vector3(0, 0, 1),
                    1 => new Vector3(-1, 0, 0),
                    2 => new Vector3(0, 0, -1),
                    3 => new Vector3(1, 0, 0),
                    _ => throw new System.NotImplementedException()
                };

                if (CheckDestination(direction))
                {
                    isMoving = true;
                    targetPosition += direction;
                    animShift = new Vector3(direction.x * 0.03f, direction.y * 0.03f, direction.z * 0.03f);
                    return;
                }

                iter++;
            }
        }

        private bool CheckDestination(Vector3 direction)
        {
            Vector3 tempPos = targetPosition + direction;

            var zombies = ViewModel.instance.zombies;
            var skeletons = ViewModel.instance.skeletons;

            for (int i = 0; i < zombies.Count; i++)
            {
                if (tempPos == zombies[i].targetPosition)
                {
                    return false;
                }
            }

            for (int i = 0; i < skeletons.Count; i++)
            {
                if (tempPos == skeletons[i].targetPosition)
                {
                    return false;
                }
            }

            if (ViewModel.instance.playerModel.targetPosition == tempPos)
            {
                return false;
            }
            
            if (ViewModel.instance.field.field[(int)tempPos.x, (int)tempPos.z * (-1)] != 5)
            {
                return false;
            }

            return true;

        }

        
    }
}
