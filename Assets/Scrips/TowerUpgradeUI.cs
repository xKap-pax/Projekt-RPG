using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TowerUpgradeUI : MonoBehaviour
{

    public Tower tower;
    public Button upgradeButton;
    public TextMeshProUGUI priceTxt;
    private bool justOpened = true;
    private void Awake()
        {
        upgradeButton.onClick.AddListener(TryUpgrade);
    }
    private void TryUpgrade()
    {
      if(CoinManager.Instance.coins < tower.upgradeStages[tower.upgradeStage].price)
        {
            return;
        }
        CoinManager.Instance.UpdateCoins(-tower.upgradeStages[tower.upgradeStage].price);
        tower.Upgrade();
        Destroy(gameObject);
    }
   void update()
    {
        if (justOpened)
        {
            justOpened = false;
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Destroy(gameObject);
        }
        if(tower.upgradeStage ==tower.upgradeStages.Length)
        {
            Destroy(gameObject);
        }
       
    }
}
