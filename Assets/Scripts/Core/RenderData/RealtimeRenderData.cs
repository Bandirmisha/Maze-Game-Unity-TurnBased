using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class RealtimeRenderData
    {
        public string playerHP;
        public string quest;

        public bool isKeyPicked;

        public Vector3 playerPosition;

        public List<EnemyRenderData> zombiesRD;
        public List<EnemyRenderData> skeletonsRD;
        public List<Vector3> arrowsPositions;

        public RealtimeRenderData(int hp,
                                  string ques,   
                                  bool iskeypicked,
                                  Vector3 playerPos,
                                  List<EnemyRenderData> zombiesRI,
                                  List<EnemyRenderData> skeletonsRI,
                                  List<Vector3> arrowsPos) 
        { 
            playerHP = hp.ToString();
            quest = ques;
            isKeyPicked = iskeypicked;
            playerPosition = playerPos;
            zombiesRD = zombiesRI;
            skeletonsRD = skeletonsRI;
            arrowsPositions = arrowsPos;
        }

    }
}
