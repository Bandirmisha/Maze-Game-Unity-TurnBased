using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System;

namespace MazeGame
{
    public class ViewModel : MonoBehaviour
    {
        public static ViewModel instance { get; set; }

        [HideInInspector] public View view { get; set; }

        public Field field { get; set; }

        [HideInInspector] public PlayerModel playerModel { get; set; }
        [HideInInspector] public List<Zombie> zombies { get; set; }
        [HideInInspector] public List<Skeleton> skeletons { get; set; }

        [HideInInspector] public Action onGameEnd { get; set; }


        private void OnEnable()
        {
            Time.timeScale = 1;

            instance = this;

            field = new Field();

            playerModel = new PlayerModel();
            CreateZombies();
            CreateSkeletons();
           
            view = GetComponent<View>();
        }
        
        private void Start()
        {
            SetQuest(0);

            onGameEnd += BlockControls;
            onGameEnd += view.uiManager.ShowGameEndMenu;
        }

        private void FixedUpdate()
        {
            foreach (Zombie zombie in zombies)
                zombie.Event();
            foreach (Skeleton skeleton in skeletons)
                skeleton.Event();
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
            while (field.field[(int)vec.x, -(int)vec.z].type == CellType.Wall);

            return vec;
        }

        private void BlockControls()
        {
            Time.timeScale = 0;
            playerModel.canMove = false;
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
