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
        public Vector3 currentPosition;
        public Vector3 targetPosition;
        public Vector3 animShift;
        public int HP;
        public string Quest;
        public bool isKeyPicked;
        public bool isMoving;

        public PlayerModel()
        {
            currentPosition = new Vector3(1, 0, -1);
            targetPosition = currentPosition;
            isKeyPicked = false;
            isMoving = false;

            HP = 100;
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

        public void SetDestination(Vector3 direction)
        {
            isMoving = true;
            targetPosition += direction;
            animShift = new Vector3(direction.x * 0.05f, direction.y * 0.05f, direction.z * 0.05f);
        }

        

        public void Attack()
        {
            foreach (Zombie zombie in ViewModel.instance.zombies)
            {

                if (zombie.targetPosition.x == targetPosition.x - 1 && zombie.targetPosition.z == targetPosition.z ||
                    zombie.targetPosition.x == targetPosition.x && zombie.targetPosition.z == targetPosition.z - 1 ||
                    zombie.targetPosition.x == targetPosition.x + 1 && zombie.targetPosition.z == targetPosition.z ||
                    zombie.targetPosition.x == targetPosition.x && zombie.targetPosition.z == targetPosition.z + 1)
                {
                    zombie.TakeDamage(1);
                }
            }

            foreach (Skeleton skeleton in ViewModel.instance.skeletons)
            {
                if (skeleton.targetPosition.x == targetPosition.x - 1 && skeleton.targetPosition.z == targetPosition.z ||
                    skeleton.targetPosition.x == targetPosition.x && skeleton.targetPosition.z == targetPosition.z - 1 ||
                    skeleton.targetPosition.x == targetPosition.x + 1 && skeleton.targetPosition.z == targetPosition.z ||
                    skeleton.targetPosition.x == targetPosition.x && skeleton.targetPosition.z == targetPosition.z + 1)
                {
                    skeleton.TakeDamage(1);
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
