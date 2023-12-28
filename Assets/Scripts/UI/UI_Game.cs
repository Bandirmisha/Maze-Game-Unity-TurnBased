using MazeGame;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    public Action onPlayerHealthChanged;
    public Action onQuestChanged;
    private static PlayerModel player => ViewModel.instance.playerModel;

    [field: SerializeField] private TextMeshProUGUI healthTextBox { get; set; }
    [field: SerializeField] private TextMeshProUGUI questTextBox { get; set; }
    [field: SerializeField] private GameObject PauseMenu { get; set; }
    [field: SerializeField] private GameObject GameEndMenu { get; set; }

    private void Awake()
    {
        onPlayerHealthChanged += DrawStats;
        onQuestChanged += DrawStats;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuSwitchView();
        }
    }

    public void DrawStats()
    {
        healthTextBox.text = "Здоровье: " + player.HP;
        questTextBox.text = "Задание: " + player.Quest;
    }

    public void PauseMenuSwitchView()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }

    public void ShowGameEndMenu()
    {
        GameEndMenu.SetActive(true);
    }

    public void LoadScene(int id)
    {
        SceneLoader.LoadScene(id);
    }


}
