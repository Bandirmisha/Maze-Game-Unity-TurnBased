using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System;
using Unity.VisualScripting;


namespace MazeGame
{
    public class ViewModel : MonoBehaviour
    {
        public static ViewModel instance { get; private set; }

        private View view { get; set; }
        private Model model { get; set; }

        [HideInInspector] public Action<KeyCode> onInputAction { get; private set; }
        [HideInInspector] private Action onGameMenuStateChanged { get; set; }
        [HideInInspector] public Action onGameEnd { get; private set; }



        private void Awake()
        {
            Time.timeScale = 1;

            instance = this;

            model = new Model();
            view = GetComponent<View>();

            onInputAction += InputAction;
        }

        private void Start()
        {
            onGameEnd += GameOver;
            onGameEnd += view.uiManager.ShowGameEndMenu;

            onGameMenuStateChanged += view.uiManager.PauseMenuSwitchView;
        }

        private void Update()
        {
            model.Tick();
        }


        public void InputAction(KeyCode key)
        {
            if (key == KeyCode.Escape)
                onGameMenuStateChanged.Invoke();

            model.PlayerInputAction(key);
        }


        public StaticRenderData GetStaticRenderData()
        {
            return new StaticRenderData(model.mazeWidth,
                                        model.mazeHeight,
                                        model.Field,
                                        model.keyPos,
                                        model.exitPos);
        }

        public RealtimeRenderData GetRealtimeRenderData()
        {
            return new RealtimeRenderData(model.playerHP,
                                          model.quest,
                                          model.isKeyPicked,
                                          model.playerCurrentPosition,
                                          model.GetZombiesRenderData(),
                                          model.GetSkeletonsRenderData(),
                                          model.GetArrowsCurrentPositions());
        }


        private void GameOver()
        {
            Time.timeScale = 0;
        }
    }
}
