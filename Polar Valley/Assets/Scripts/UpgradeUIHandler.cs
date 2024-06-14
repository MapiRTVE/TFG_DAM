using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static UpgradeUIHandler main;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI upgradeUI;

    public bool mouse_over = false;

    private void Awake()
    {
        main = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }

    public void UpdateUpgradeCost(Turret turret)
    {
        if (turret.currentUpgradeCost != 0)
        {
            upgradeUI.text = "Mejorar - " + turret.currentUpgradeCost;
        }
        else
        {
            upgradeUI.text = "Mejorar - " + turret.baseUpgradeCost;
        }
    }
}
