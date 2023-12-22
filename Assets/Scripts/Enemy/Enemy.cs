using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace MazeGame
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        private Player player => GameManager.instance.player.GetComponent<Player>();

        public Vector3 position;
        private Vector3 animShift;
        private int HP;
        protected bool alive;
        private bool isMoving;
        private bool canAttack;

        [SerializeField] private float attackCooldown;
        private float currentTime;

        public Slider healthBar;

        private void Start()
        {
            position = transform.position;

            HP = 5;
            healthBar.value = HP;
            alive = true;

            canAttack = true;

        }

        protected virtual void FixedUpdate()
        {
            if(!alive)
            {
                //Проваливание под землю
                if (transform.position.y > -10f)
                    transform.position += new Vector3(0, -0.1f, 0);
                else Destroy(this.gameObject);

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
        public void Attack()
        {
            canAttack = false;
            GameManager.instance.player.GetComponent<Player>().TakeDamage(2);
        }
        public void TakeDamage(int damage)
        {
            HP -= damage;
            healthBar.value = HP;

            if (HP <= 0)
            {
                alive = false;

                gameObject.SetActive(false);
                gameObject.transform.position = new Vector3(-1, -1, -1);
                position = gameObject.transform.position;

                return;
            }
        }

        private bool isPlayerNearby()
        {
            if (player.position.x == position.x - 1 && player.position.z == position.z ||
                player.position.x == position.x && player.position.z == position.z - 1 ||
                player.position.x == position.x + 1 && player.position.z == position.z ||
                player.position.x == position.x && player.position.z == position.z + 1)
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
                    position += direction;
                    animShift = new Vector3(direction.x * 0.03f, direction.y * 0.03f, direction.z * 0.03f);
                    return;
                }

                iter++;
            }
        }

        private bool CheckDestination(Vector3 direction)
        {
            Vector3 tempPos = position + direction;

            var zombies = GameManager.instance.zombies;
            var skeletons = GameManager.instance.skeletons;

            for (int i = 0; i < zombies.Count; i++)
            {
                if (tempPos == zombies[i].GetComponent<Zombie>().position)
                {
                    return false;
                }
            }

            for (int i = 0; i < skeletons.Count; i++)
            {
                if (tempPos == skeletons[i].GetComponent<Skeleton>().position)
                {
                    return false;
                }
            }

            if (GameManager.instance.player.GetComponent<Player>().position == tempPos)
            {
                return false;
            }
            
            if (GameManager.instance.field.field[(int)tempPos.x, (int)tempPos.z * (-1)] != 5)
            {
                return false;
            }

            return true;

        }

        
    }
}
