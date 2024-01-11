using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MazeGame
{
    public class Player 
    {
        private Model model => Model.inst;

        public Vector3 currentPosition { get; private set; }
        public Vector3 targetPosition { get; private set; }
        private Vector3 animShift { get; set; }
        public int HP { get; set; }
        public string Quest { get; set; }
        public bool isKeyPicked { get; set; }
        public bool isMoving { get; set; }
        public bool canMove { get; set; }

        public Player()
        {
            currentPosition = new Vector3(1, 0, -1);
            targetPosition = currentPosition;
            isKeyPicked = false;
            isMoving = false;
            canMove = true;
            HP = 100;

            SetQuest(0);
        }

        public void SetQuest(int index)
        {
            switch (index)
            {
                case 0: Quest = "Найдите ключ"; break;
                case 1: Quest = "Доберитесь до выхода"; break;
            }
        }
        public void SetDestination(Vector3 direction)
        {
            if (!model.CheckDestination(targetPosition, direction))
                return;

            isMoving = true;
            targetPosition += direction;
            animShift = new Vector3(direction.x * 0.05f, direction.y * 0.05f, direction.z * 0.05f);
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

            if(currentPosition == model.keyPos)
                PickUpKey();
            if (currentPosition == model.exitPos)
                Quit();
        }

 

        public void Attack()
        {
            foreach (var enemyInfo in model.GetZombiesRenderData().Concat(model.GetSkeletonsRenderData()))
            {
                if (model.isPlayerNearby(enemyInfo.EnemyPosition))
                {
                    model.EnemyTakeDamage(enemyInfo.EnemyPosition, 1);
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
        }



        public void PickUpKey()
        {
            isKeyPicked = true;
            SetQuest(1);
        }
        public void Quit()
        {
            if(isKeyPicked)
            {
                canMove = false;
                ViewModel.instance.onGameEnd.Invoke();
            }
            else
            {
                Debug.Log("Нужно найти ключ!");
            }
        }
    }
}
