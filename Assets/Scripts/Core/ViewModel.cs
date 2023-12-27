using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System;

namespace MazeGame
{
    public class ViewModel : MonoBehaviour
    {
        public static ViewModel instance;

        public View view;

        public Field field;
        
        [HideInInspector] public PlayerModel playerModel;
        [HideInInspector] public List<Zombie> zombies;
        [HideInInspector] public List<Skeleton> skeletons;

        [HideInInspector] public Action onGameEnd;


        private void OnEnable()
        {
            instance = this;

            field = new Field();

            playerModel = new PlayerModel();
            CreateZombies();
            CreateSkeletons();
           

            onGameEnd += BlockControls;
            onGameEnd += view.uiManager.ShowGameEndMenu;
        }

        private void Start()
        {
            SetQuest(0);
        }

        private void FixedUpdate()
        {
            foreach (Zombie zombie in zombies)
                zombie.Event();
            foreach (Skeleton skeleton in skeletons)
                skeleton.Event();
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }


        private void CreateZombies()
        {
            zombies = new List<Zombie>();
            int zombieCount = UnityEngine.Random.Range(5, 10);
            for (int i = 0; i < zombieCount; i++)
            {
                zombies.Add(new Zombie(GetEnemyStartPos()));
            }
        }

        private void CreateSkeletons()
        {
            skeletons = new List<Skeleton>();
            int skeletonCount = UnityEngine.Random.Range(3, 6);
            for (int i = 0; i < skeletonCount; i++)
            {
                skeletons.Add(new Skeleton(GetEnemyStartPos()));
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
            Time.timeScale = 0;
        }


        public void SetQuest(int index)
        {
            switch (index)
            {
                case 0: playerModel.Quest = "Найдите ключ"; break;
                case 1: playerModel.Quest = "Доберитесь до выхода"; break;
            }
            view.uiManager.onQuestChanged.Invoke();
        }
    }
}
