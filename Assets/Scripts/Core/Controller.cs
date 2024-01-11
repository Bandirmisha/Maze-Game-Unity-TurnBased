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
    public class Controller : MonoBehaviour
    {
        private ViewModel vm => ViewModel.instance;

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
                vm.onInputAction.Invoke(KeyCode.Mouse0);
         
            if (Input.GetKey(KeyCode.W))
                vm.onInputAction.Invoke(KeyCode.W);
            else if (Input.GetKey(KeyCode.A))
                vm.onInputAction.Invoke(KeyCode.A);
            else if (Input.GetKey(KeyCode.S))
                vm.onInputAction.Invoke(KeyCode.S);
            else if (Input.GetKey(KeyCode.D))
                vm.onInputAction.Invoke(KeyCode.D);
        }
    }
}

