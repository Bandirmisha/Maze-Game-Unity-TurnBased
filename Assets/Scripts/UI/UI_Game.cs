using MazeGame;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{

    [field: SerializeField] private TextMeshProUGUI healthTextBox { get; set; }
    [field: SerializeField] private TextMeshProUGUI questTextBox { get; set; }
    [field: SerializeField] private GameObject PauseMenu { get; set; }
    [field: SerializeField] private GameObject GameEndMenu { get; set; }


    public void DrawStats(string health, string quest)
    {
        healthTextBox.text = "Здоровье: " + health;
        questTextBox.text = "Задание: " + quest;
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
