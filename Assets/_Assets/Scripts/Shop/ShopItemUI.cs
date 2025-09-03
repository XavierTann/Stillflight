using TMPro; // if you use TextMeshPro, else replace with UnityEngine.UI.Text
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("Refs")]
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button actionButton;

    public TMP_Text actionButtonLabel;
    private ShopSystem shopSystem;

    public CameraSO cameraSO;

    private void Awake()
    {
        // Default setup for safety
        Setup(cameraSO);
        shopSystem = ShopSystem.Instance;
    }

    public void Setup(CameraSO cameraSO)
    {
        this.cameraSO = cameraSO;

        if (icon)
            icon.sprite = cameraSO.icon;
        if (nameText)
            nameText.text = cameraSO.cameraName;
        if (priceText)
            priceText.text = cameraSO.cost.ToString() + " Cr";

        // Hook up button here
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        shopSystem.TryBuy(cameraSO);
    }

    // void ConfigureButton(string label, bool interactable)
    // {
    //     if (actionButtonLabel)
    //         actionButtonLabel.text = label;
    //     if (actionButton)
    //         actionButton.interactable = interactable;
    // }
}
