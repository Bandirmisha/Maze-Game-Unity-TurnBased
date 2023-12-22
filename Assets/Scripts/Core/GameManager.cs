using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System;

namespace MazeGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public Action onGameEnd;
        public UI_Game uiManager;

        public Field field;

        [HideInInspector] public List<GameObject> walls;
        [HideInInspector] public List<GameObject> floors;

        [HideInInspector] public GameObject player;
        [HideInInspector] public List<GameObject> zombies;
        [HideInInspector] public List<GameObject> skeletons;

        [HideInInspector] public GameObject key;
        [HideInInspector] public GameObject exit;

        //Пустые объекты-родители для группировки
        private static GameObject wallsParent;
        public static GameObject floorsParent;

        [Space]
        [Header("Окружение")]
        public GameObject wallPrefab;
        public GameObject floorPrefab;

        [Space]
        [Header("Игрок")]
        public GameObject playerPrefab;

        [Space]
        [Header("Ключевые объекты")]
        public GameObject keyPrefab;
        public GameObject exitPrefab;

        [Space]
        [Header("Враги")]
        public GameObject zombiePrefab;
        public GameObject skeletonPrefab;


        private void OnEnable()
        {
            instance = this;

            onGameEnd += BlockControls;
            onGameEnd += uiManager.ShowGameEndMenu;

            wallsParent = new GameObject();
            floorsParent = new GameObject();
            wallsParent.name = "Walls";
            floorsParent.name = "Floors";

            field = new Field();
            walls = new List<GameObject>();
            floors = new List<GameObject>();
            zombies = new List<GameObject>();
            skeletons = new List<GameObject>();

            CreateInvironment();
            key = CreateGameObject(keyPrefab, field.keyPos);
            exit = CreateGameObject(exitPrefab, field.exitPos);

            player = CreateGameObject(playerPrefab, new Vector3(1, 0, -1));

            CreateZombies();
            CreateSkeletons();
        }

        private void CreateInvironment()
        {
            for (int j = 0; j < field.height; j++)
            {
                for (int i = 0; i < field.width; i++)
                {
                    //Стены
                    if (field.field[i, j] == 0)
                    {
                        walls.Add(CreateGameObject(wallPrefab, new Vector3(i, 0, -j)));
                        walls[walls.Count-1].transform.SetParent(wallsParent.transform);
                    }
                    //Проход
                    else if (field.field[i, j] == 5)
                    {
                        floors.Add(CreateGameObject(floorPrefab, new Vector3(i, 0, -j)));
                        floors[floors.Count - 1].transform.SetParent(floorsParent.transform);
                    }

                }
            }
        }

        public GameObject CreateGameObject(GameObject gameObj, Vector3 transform)
        {
            return Instantiate(gameObj, transform, Quaternion.identity);
        }

        private void CreateZombies()
        {
            int zombieCount = UnityEngine.Random.Range(5, 10);
            for (int i = 0; i < zombieCount; i++)
            {
                zombies.Add(CreateGameObject(zombiePrefab, GetEnemyStartPos()));
            }
        }

        private void CreateSkeletons()
        {
            int skeletonCount = UnityEngine.Random.Range(3, 6);
            for (int i = 0; i < skeletonCount; i++)
            {
                skeletons.Add(CreateGameObject(skeletonPrefab, GetEnemyStartPos()));
            }
        }

        private Vector3 GetEnemyStartPos()
        {
            Vector3 vec;

            do
            {
                vec = new Vector3(UnityEngine.Random.Range(3, field.width), 0, -UnityEngine.Random.Range(3, field.height));
            }
            while (field.field[(int)vec.x, -(int)vec.z] == 0);
                
            return vec;
        }
        
        

        private void BlockControls()
        {
            player.SetActive(false);
        }
    }
}
