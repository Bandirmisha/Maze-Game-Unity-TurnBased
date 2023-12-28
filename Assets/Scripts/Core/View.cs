using MazeGame;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGame
{
    public class View : MonoBehaviour
    {
        [field: SerializeField] public UI_Game uiManager { get; private set; }

        private Field field => ViewModel.instance.field;

        private PlayerModel playerModel => ViewModel.instance.playerModel;
        private List<Zombie> zombieModels => ViewModel.instance.zombies;
        private List<Skeleton> skeletonModels => ViewModel.instance.skeletons;

        [HideInInspector] public List<GameObject> walls { get; set; }
        [HideInInspector] public List<GameObject> floors { get; set; }

        [HideInInspector] public GameObject player { get; set; }
        [HideInInspector] public List<GameObject> zombies { get; set; }
        [HideInInspector] public List<GameObject> skeletons { get; set; }

        [HideInInspector] public GameObject key { get; set; }
        [HideInInspector] public GameObject exit { get; set; }

        //Пустые объекты-родители для группировки
        private static GameObject wallsParent;
        public static GameObject floorsParent;

        //Префабы
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
        public GameObject arrowPrefab;

        private void Start()
        {
            wallsParent = new GameObject();
            floorsParent = new GameObject();
            wallsParent.name = "Walls";
            floorsParent.name = "Floors";

            walls = new List<GameObject>();
            floors = new List<GameObject>();
            zombies = new List<GameObject>();
            skeletons = new List<GameObject>();

            CreateInvironment();
            key = CreateGameObject(keyPrefab, field.keyPos);
            exit = CreateGameObject(exitPrefab, field.exitPos);

            player = CreateGameObject(playerPrefab, playerModel.currentPosition);
            CreateZombies();
            CreateSkeletons();
        }

        private void Update()
        {
            player.transform.position = playerModel.currentPosition;

            for (int i = 0; i < zombieModels.Count; i++)
            {
                zombies[i].transform.position = zombieModels[i].currentPosition;
                zombies[i].GetComponentInChildren<Slider>().value = zombieModels[i].HP;
            }

            for (int i = 0; i < skeletonModels.Count; i++)
            {
                skeletons[i].transform.position = skeletonModels[i].currentPosition;
                skeletons[i].GetComponentInChildren<Slider>().value = skeletonModels[i].HP;
            }
        }

        private void CreateInvironment()
        {
            for (int j = 0; j < field.height; j++)
            {
                for (int i = 0; i < field.width; i++)
                {
                    //Стены
                    if (field.field[i, j].type == CellType.Wall)
                    {
                        walls.Add(CreateGameObject(wallPrefab, new Vector3(i, 0, -j)));
                        walls[walls.Count - 1].transform.SetParent(wallsParent.transform);
                    }
                    //Проход
                    else if (field.field[i, j].type == CellType.Floor)
                    {
                        floors.Add(CreateGameObject(floorPrefab, new Vector3(i, 0, -j)));
                        floors[floors.Count - 1].transform.SetParent(floorsParent.transform);
                    }
                }
            }
        }

        private void CreateZombies()
        {
            foreach (var zombie in zombieModels)
            {
                zombies.Add(CreateGameObject(zombiePrefab, zombie.currentPosition));
            }
        }

        private void CreateSkeletons()
        {
            foreach (var skeleton in skeletonModels)
            {
                skeletons.Add(CreateGameObject(skeletonPrefab, skeleton.currentPosition));
            }
        }

        public GameObject CreateGameObject(GameObject gameObj, Vector3 transform)
        {
            return Instantiate(gameObj, transform, Quaternion.identity);
        }

    }
}
