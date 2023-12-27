using MazeGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    void Update()
    {
        transform.position = ViewModel.instance.playerModel.currentPosition + new Vector3(0, 11, -2);
    }
}
