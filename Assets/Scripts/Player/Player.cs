using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MazeGame
{
    public class Player : MonoBehaviour
    {
        public Vector3 position;
        private Vector3 animShift;
        public int HP;
        public string Quest;
        public bool isKeyPicked;
        public bool isMoving;

        private void Start()
        {
            position = transform.position;
            isKeyPicked = false;
            isMoving = false;

            HP = 100;
            SetQuest(0);
        }

        private void Update()
        {
            if(isMoving)
            {
                Move();
            }
        }

        private void Move()
        {
            if (Vector3.Distance(transform.position, position) > 0.05f)
            {
                transform.position += animShift;
            }
            else
            {
                transform.position = position;
                isMoving = false;
            }
        }

        public void SetDestination(Vector3 direction)
        {
            isMoving = true;
            position += direction;
            animShift = new Vector3(direction.x * 0.05f, direction.y * 0.05f, direction.z * 0.05f);
        }

        private void SetQuest(int index)
        {
            switch (index)
            {
                case 0: Quest = "Найдите ключ"; break;
                case 1: Quest = "Доберитесь до выхода"; break;
            }
            GameManager.instance.uiManager.onQuestChanged.Invoke();
        }

        public void Attack()
        {
            foreach (GameObject zomb in GameManager.instance.zombies)
            {
                Zombie zombie = zomb.GetComponent<Zombie>();

                if (zombie.position.x == position.x - 1 && zombie.position.z == position.z ||
                    zombie.position.x == position.x && zombie.position.z == position.z - 1 ||
                    zombie.position.x == position.x + 1 && zombie.position.z == position.z ||
                    zombie.position.x == position.x && zombie.position.z == position.z + 1)
                {
                    zombie.TakeDamage(1);
                }
            }

            foreach (GameObject skel in GameManager.instance.skeletons)
            {
                Skeleton skeleton = skel.GetComponent<Skeleton>();

                if (skeleton.position.x == position.x - 1 && skeleton.position.z == position.z ||
                    skeleton.position.x == position.x && skeleton.position.z == position.z - 1 ||
                    skeleton.position.x == position.x + 1 && skeleton.position.z == position.z ||
                    skeleton.position.x == position.x && skeleton.position.z == position.z + 1)
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
                GameManager.instance.onGameEnd.Invoke();
                return;
            }

            GameManager.instance.uiManager.onPlayerHealthChanged.Invoke();
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
                GameManager.instance.onGameEnd.Invoke();
            }
            else
            {
                Debug.Log("Нужно найти ключ!");
            }
        }

    }
}
