using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MazeGame
{
    public class PlayerModel 
    {
        public Vector3 currentPosition { get; private set; }
        public Vector3 targetPosition { get; private set; }
        private Vector3 animShift { get; set; }
        public int HP { get; set; }
        public string Quest { get; set; }
        public bool isKeyPicked { get; set; }
        public bool isMoving { get; set; }
        public bool canMove { get; set; }

        public PlayerModel()
        {
            currentPosition = new Vector3(1, 0, -1);
            targetPosition = currentPosition;
            isKeyPicked = false;
            isMoving = false;
            canMove = true;
            HP = 100;
        }

        public void Move()
        {
            if (!canMove) return;

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

        public void SetDestination(Vector3 direction)
        {
            if (!CheckDestination(direction))
                return;

            isMoving = true;
            targetPosition += direction;
            animShift = new Vector3(direction.x * 0.05f, direction.y * 0.05f, direction.z * 0.05f);
        }

        private bool CheckDestination(Vector3 direction)
        {
            Vector3 tempPos = targetPosition + direction;

            if (ViewModel.instance.field.field[(int)tempPos.x, (int)tempPos.z * (-1)].type == CellType.Floor)
            {
                if (ViewModel.instance.zombies.Concat(ViewModel.instance.skeletons).Any(x => x.targetPosition == tempPos))
                    return false;
                
                return true;
            }

            return false;
        }

        public void Attack()
        {
            foreach (Enemy enemy in ViewModel.instance.zombies.Concat(ViewModel.instance.skeletons))
            {
                if (enemy.targetPosition.x == targetPosition.x - 1 && enemy.targetPosition.z == targetPosition.z ||
                    enemy.targetPosition.x == targetPosition.x && enemy.targetPosition.z == targetPosition.z - 1 ||
                    enemy.targetPosition.x == targetPosition.x + 1 && enemy.targetPosition.z == targetPosition.z ||
                    enemy.targetPosition.x == targetPosition.x && enemy.targetPosition.z == targetPosition.z + 1)
                {
                    enemy.TakeDamage(1);
                }
            }
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;

            if (HP <= 0)
            {
                ViewModel.instance.onGameEnd.Invoke();
                return;
            }

            ViewModel.instance.view.uiManager.onPlayerHealthChanged.Invoke();
        }

        public void PickUpKey()
        {
            isKeyPicked = true;
            ViewModel.instance.SetQuest(1);
        }

        public void Quit()
        {
            if(isKeyPicked)
            {
                ViewModel.instance.onGameEnd.Invoke();
            }
            else
            {
                Debug.Log("Нужно найти ключ!");
            }
        }

    }
}
