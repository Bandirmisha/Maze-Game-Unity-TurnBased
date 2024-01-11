using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGame
{
    public class View : MonoBehaviour
    {
        private ViewModel viewModel => ViewModel.instance;
        [field: SerializeField] public UI_Game uiManager { get; private set; }
        private StaticRenderData staticRD { get; set; }
        private RealtimeRenderData realtimeRD { get; set; }

        #region Игровые объекты 
        private List<GameObject> walls { get; set; }
        private List<GameObject> floors { get; set; }

        private GameObject playerGameObject { get; set; }
        private List<GameObject> zombiesGameObjects { get; set; }
        private List<GameObject> skeletonsGameObjects { get; set; }
        private List<GameObject> arrowsGameObjects { get; set; }

        private GameObject key { get; set; }
        private GameObject exit { get; set; }
        #endregion

        #region Пустые объекты-родители для группировки
        private static GameObject wallsParent;
        private static GameObject floorsParent;
        #endregion

        #region Префабы
        [Space]
        [Header("Окружение")]
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject floorPrefab;

        [Space]
        [Header("Игрок")]
        [SerializeField] private GameObject playerPrefab;

        [Space]
        [Header("Ключевые объекты")]
        [SerializeField] private GameObject keyPrefab;
        [SerializeField] private GameObject exitPrefab;

        [Space]
        [Header("Враги")]
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private GameObject skeletonPrefab;
        [SerializeField] private GameObject arrowPrefab;
        #endregion

        private void Start()
        {
            wallsParent = new GameObject();
            floorsParent = new GameObject();
            wallsParent.name = "Walls";
            floorsParent.name = "Floors";

            walls = new List<GameObject>();
            floors = new List<GameObject>();
            zombiesGameObjects = new List<GameObject>();
            skeletonsGameObjects = new List<GameObject>();
            arrowsGameObjects = new List<GameObject>();

            staticRD = viewModel.GetStaticRenderData();
            realtimeRD = viewModel.GetRealtimeRenderData();

            CreateEnvironment();
            key = CreateGameObject(keyPrefab, staticRD.keyPosition);
            exit = CreateGameObject(exitPrefab, staticRD.exitPosition);

            playerGameObject = CreateGameObject(playerPrefab, realtimeRD.playerPosition);
            CreateZombies();
            CreateSkeletons();
        }

        private void Update()
        {
            realtimeRD = viewModel.GetRealtimeRenderData();

            UpdatePlayerData();
            UpdateEnemiesData();
            UpdateArrowsData();

            if (key && realtimeRD.isKeyPicked)
                Destroy(key);
        }
        private void UpdatePlayerData()
        {
            uiManager.DrawStats(realtimeRD.playerHP, realtimeRD.quest);
            playerGameObject.transform.position = realtimeRD.playerPosition;
        }
        private void UpdateEnemiesData()
        {
            for (int i = 0; i < realtimeRD.zombiesRD.Count; i++)
            {
                zombiesGameObjects[i].transform.position = realtimeRD.zombiesRD[i].EnemyPosition;
                zombiesGameObjects[i].GetComponentInChildren<Slider>().value = realtimeRD.zombiesRD[i].EnemyHP;
            }

            for (int i = 0; i < realtimeRD.skeletonsRD.Count; i++)
            {
                skeletonsGameObjects[i].transform.position = realtimeRD.skeletonsRD[i].EnemyPosition;
                skeletonsGameObjects[i].GetComponentInChildren<Slider>().value = realtimeRD.skeletonsRD[i].EnemyHP;
            }
        }
        private void UpdateArrowsData()
        {
            for (int i = 0; i < realtimeRD.arrowsPositions.Count; i++)
            {
                if (i > arrowsGameObjects.Count - 1)
                    arrowsGameObjects.Add(CreateGameObject(arrowPrefab, realtimeRD.arrowsPositions[i]));

                if (realtimeRD.arrowsPositions[i].y < 0)
                {
                    Destroy(arrowsGameObjects[i]);
                    arrowsGameObjects[i] = null;
                }

                if (arrowsGameObjects[i] is not null)
                    arrowsGameObjects[i].transform.position = realtimeRD.arrowsPositions[i];
            }
        }


        private void CreateEnvironment()
        {
            foreach (var wallPos in staticRD.wallsPositions)
            {
                walls.Add(CreateGameObject(wallPrefab, wallPos));
                walls[walls.Count - 1].transform.SetParent(wallsParent.transform);
            }

            foreach (var floorPos in staticRD.floorsPositions)
            {
                floors.Add(CreateGameObject(floorPrefab, floorPos));
                floors[floors.Count - 1].transform.SetParent(floorsParent.transform);
            }
        }
        private void CreateZombies()
        {
            foreach (var zombieRD in realtimeRD.zombiesRD)
            {
                zombiesGameObjects.Add(CreateGameObject(zombiePrefab, zombieRD.EnemyPosition));
            }
        }
        private void CreateSkeletons()
        {
            foreach (var skeletonRD in realtimeRD.skeletonsRD)
            {
                skeletonsGameObjects.Add(CreateGameObject(skeletonPrefab, skeletonRD.EnemyPosition));
            }
        }
        public GameObject CreateGameObject(GameObject gameObj, Vector3 transform)
        {
            return Instantiate(gameObj, transform, Quaternion.identity);
        }

    }
}
