using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MazeGame
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel player => ViewModel.instance.playerModel;

        private void Update()
        {
            if (!player.isMoving)
                HandleInput();
            else 
                player.Move();       
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
                player.Attack();

            if (Input.GetKey(KeyCode.W))
                player.SetDestination(Vector3.forward);
            else if (Input.GetKey(KeyCode.A))
                player.SetDestination(Vector3.left);
            else if (Input.GetKey(KeyCode.S))
                player.SetDestination(Vector3.back);
            else if (Input.GetKey(KeyCode.D))
                player.SetDestination(Vector3.right);
        }
    }
}

