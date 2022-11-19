using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI timeToWaveText;

    [SerializeField] Button fireButton;
    [SerializeField] Button iceButton;
    [SerializeField] Button lightningButton;
    [SerializeField] Button windButton;
    [SerializeField] Image selectionArrow;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SelectMagicType(Player.Instance.currentMagicType);
    }

    void Update()
    {
        // int value = Mathf.Clamp((int)(SpawnerManager.Instance.spawnerTimeWait - Time.time), 0, int.MaxValue);
        int value = (int) (SpawnerManager.Instance.spawnerTimeWait - Time.time + 0.5f);
        if (value <= 0)
        {
            timeToWaveText.text = "";
        }
        else
        {
            timeToWaveText.text = value + "s";
        }
    }

    public void SelectMagicType(Utility.MagicType magicType)
    {
        switch (magicType)
        {
            case Utility.MagicType.Fire: 
                MoveSelectionArrow(fireButton.transform.position.x);
                selectionArrow.color = fireButton.colors.normalColor;
                break;
            case Utility.MagicType.Ice: 
                MoveSelectionArrow(iceButton.transform.position.x);
                selectionArrow.color = iceButton.colors.normalColor;
                break;
            case Utility.MagicType.Lightning: 
                MoveSelectionArrow(lightningButton.transform.position.x);
                selectionArrow.color = lightningButton.colors.normalColor;
                break;
            case Utility.MagicType.Wind: 
                MoveSelectionArrow(windButton.transform.position.x);
                selectionArrow.color = windButton.colors.normalColor;
                break;
        }
    }
    
    public void SelectFire()
    {
        Player.Instance.SetMagicType(Utility.MagicType.Fire);
        SelectMagicType(Utility.MagicType.Fire);
    }
    public void SelectIce()
    {
        Player.Instance.SetMagicType(Utility.MagicType.Ice);
        SelectMagicType(Utility.MagicType.Ice);
    }
    public void SelectLightning()
    {
        Player.Instance.SetMagicType(Utility.MagicType.Lightning);
        SelectMagicType(Utility.MagicType.Lightning);
    }
    public void SelectWind()
    {
        Player.Instance.SetMagicType(Utility.MagicType.Wind);
        SelectMagicType(Utility.MagicType.Wind);
    }
    private void MoveSelectionArrow(float x)
    {
        selectionArrow.transform.position = new Vector3(x, selectionArrow.transform.position.y, selectionArrow.transform.position.z);
    }
}