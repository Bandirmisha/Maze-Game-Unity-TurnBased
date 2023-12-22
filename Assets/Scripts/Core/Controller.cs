using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MazeGame
{
    public class Controller : MonoBehaviour
    {
        private Player player;

        private void Start()
        {
            player = GetComponent<Player>();
        }

        private void Update()
        {
            if (!player.isMoving)
                HandleInput();
        }

        private void HandleInput()
        {
            Vector3 direction;

            if (Input.GetKey(KeyCode.W))
            {
                direction = new Vector3(0, 0, 1);
                if (CheckDestination(direction))
                    player.SetDestination(direction);
            }

            else if (Input.GetKey(KeyCode.A))
            {
                direction = new Vector3(-1, 0, 0);
                if (CheckDestination(direction))
                    player.SetDestination(direction);
            }

            else if (Input.GetKey(KeyCode.S))
            {
                direction = new Vector3(0, 0, -1);
                if (CheckDestination(direction))
                    player.SetDestination(direction);
            }

            else if (Input.GetKey(KeyCode.D))
            {
                direction = new Vector3(1, 0, 0);
                if (CheckDestination(direction))
                    player.SetDestination(direction);
            }

            if (Input.GetMouseButtonDown(0))
            {
                player.Attack();
            }
        }

        private bool CheckDestination(Vector3 direction)
        {
            Vector3 tempPos = player.position + direction;
            
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

            if (GameManager.instance.field.field[(int)tempPos.x, (int)tempPos.z*(-1)] == 5)
            {
                return true;
            }
            else return false;

        }

    }
}

