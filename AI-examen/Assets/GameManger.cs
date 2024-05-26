using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public CityController cityController;
    public MonsterController monsterController;
    public TMP_Text progressText, amountMR, amountMM, amountV,villagerCounter;
    public Slider progressSlider, amountMRs, amountMMs, amountVs;
    public GameObject loadningMenu, settingMenu,hud;
    public Color slected, nonSelected;
    public Image followPlayer, followBody;
    public Transform villagerParent;
    public string villagerCounterText = " Villagers";
    private void Start()
    {
        changeControlSystem(true);
        amountMRs.value = monsterController.amountOfMonsters[1].amount;
        amountMMs.value = monsterController.amountOfMonsters[0].amount;
        amountVs.value = cityController.Villagers[0].amount;
    }
    public void startGame() {
        settingMenu.SetActive(false);
        loadningMenu.SetActive(true);
        cityController.StartCreateCity(progressText, progressSlider,loadningMenu, hud);
    }

    public void changeControlSystem(bool followingPlayer) {
        if (followingPlayer)
        {
            followPlayer.color = slected;
            followBody.color = nonSelected;
            monsterController.targetFromPlayer = true;
        }
        else {
            followBody.color = slected;
            followPlayer.color = nonSelected;
            monsterController.targetFromPlayer = false;
        }
    }

    public void changeRangeAmount(Slider slide) {
        amountMR.text = slide.value.ToString();
        monsterController.amountOfMonsters[1].amount = (int)slide.value;
    }
    public void changeMeleeAmount(Slider slide) {
        amountMM.text = slide.value.ToString();
        monsterController.amountOfMonsters[0].amount = (int)slide.value;
    }
    public void changeVillagerAmount(Slider slide) {
        amountV.text = slide.value.ToString();
        cityController.Villagers[0].amount = (int)slide.value;

    }

    public void resetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LateUpdate()
    {
        villagerCounter.text = villagerParent.childCount + villagerCounterText;
    }
}
