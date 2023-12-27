using MazeGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            ViewModel.instance.playerModel.PickUpKey();
            Destroy(this.gameObject);
        }
    }
}
