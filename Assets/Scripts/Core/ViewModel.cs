using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System;

namespace MazeGame
{
    public class ViewModel : MonoBehaviour
    {
        public static ViewModel instance { get; private set; }

        [HideInInspector] public View view { get; private set; }

        public Field field { get; set; }

        [HideInInspector] public PlayerModel playerModel { get; private set; }
        [HideInInspector] public List<Enemy> zombies { get; private set; }
        [HideInInspector] public List<Enemy> skeletons { get; private set; }

        [HideInInspector] public Action onGameEnd { get; private set; }


        private void Awake()
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
            zombies = new List<Enemy>();
            int zombieCount = UnityEngine.Random.Range(5, 10);
            for (int i = 0; i < zombieCount; i++)
            {
                zombies.Add(new Zombie());
            }
        }

        private void CreateSkeletons()
        {
            skeletons = new List<Enemy>();
            int skeletonCount = UnityEngine.Random.Range(3, 6);
            for (int i = 0; i < skeletonCount; i++)
            {
                skeletons.Add(new Skeleton());
            }
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
