using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// In game UI controls
public class InGameMenu : MonoBehaviour
{
    private const string KILLS_FORMAT = "KILLS : {0}";
    private const string WAVES_FORMAT = "WAVES : {0}";
    private const string GOLD_FORMAT = "GOLD : {0}";

    public GameBoard gameBoardRef;

    public List<ButtonWrapper> towerButtons;

    public TextMeshProUGUI kills;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI gold;
    
    int pageIndex = 0;

    private void Start()
    {
        for (int i = 0; i < towerButtons.Count; i++)
        {
            towerButtons[i].onWrappedClickEvent += onClickTowerButton;
        }

        initializeTowerButtons();
    }

    public void goToNextPage()
    {
        if (pageIndex < (InfoCache.allTowerInfo.Count() - 1) - towerButtons.Count())
        {
            pageIndex += towerButtons.Count();
            initializeTowerButtons();
        }
    }

    public void goToPreviousPage()
    {
        if (pageIndex > towerButtons.Count() - 1)
        {
            pageIndex -= towerButtons.Count();
            initializeTowerButtons();
        }
    }

    public void initializeTowerButtons()
    {
        for (int i = pageIndex; i < towerButtons.Count; i++)
        {
            if (i < InfoCache.allTowerInfo.Count)
            {
                towerButtons[i].gameObject.SetActive(true);
                towerButtons[i].buttonText.text = InfoCache.allTowerInfo[i].name;
                towerButtons[i].args = new Dictionary<string, object>();
                towerButtons[i].args.Add("tower", InfoCache.allTowerInfo[i]);
            }
            else
            {
                towerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void onClickTowerButton(Dictionary<string, object> args)
    {
        TowerInfo info = args["tower"] as TowerInfo;

        if (gameBoardRef.baseToDefend.money >= info.cost)
        {
            gameBoardRef.builder.buildTower(info);
            gameBoardRef.baseToDefend.money -= info.cost;
        }
    }

    private void Update()
    {
        if (gameBoardRef != null && gameBoardRef.baseToDefend != null && gameBoardRef.spawner != null)
        {
            kills.text = string.Format(KILLS_FORMAT, gameBoardRef.baseToDefend.kills);
            wave.text = string.Format(WAVES_FORMAT, gameBoardRef.spawner.wave);
            gold.text = string.Format(GOLD_FORMAT, gameBoardRef.baseToDefend.money);
        }
    }
}


