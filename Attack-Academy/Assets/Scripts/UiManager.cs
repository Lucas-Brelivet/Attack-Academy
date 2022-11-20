using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas pauseCanvas;

    [SerializeField] private TextMeshProUGUI timeToWaveText;

    [SerializeField] Slider healthBar;
    [SerializeField] Slider manaBar;

    [SerializeField] Button fireButton;
    [SerializeField] Button iceButton;
    [SerializeField] Button lightningButton;
    [SerializeField] Button windButton;
    [SerializeField] Image selectionArrow;
    
    private Controls controls;


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
        healthBar.maxValue = Player.Instance.healthMax;
        healthBar.value = Player.Instance.healthMax;
        manaBar.maxValue = Player.Instance.manaMax;
        manaBar.value = Player.Instance.manaMax;

        controls = new Controls();
        controls.UI.Enable();
        controls.UI.Pause.performed += context => Pause();
    }

    void Update()
    {
        // int value = Mathf.Clamp((int)(SpawnerManager.Instance.spawnerTimeWait - Time.time), 0, int.MaxValue);
        if(SpawnerManager.Instance != null)
        {
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

        foreach (PlayerSpell ps in Player.Instance.GetComponent<PlayerPower>().playerSpells)
        {
            ColorBlock cb = new ColorBlock();
            switch (ps.magicType)
            {
                case Utility.MagicType.Fire:
                    cb = fireButton.colors;
                    break;
                case Utility.MagicType.Ice:
                    cb = iceButton.colors;
                    break;
                case Utility.MagicType.Lightning:
                    cb = lightningButton.colors;
                    break;
                case Utility.MagicType.Wind:
                    cb = windButton.colors;
                    break;
            }
            cb.normalColor = AlphaColor(cb.normalColor, ps.power / ps.distMax);
        }
    }

    private Color AlphaColor(Color c, float alpha)
    {
        return new Color(c.r, c.g, c.b, alpha);
    }

    public void Pause()
    {
        gameCanvas.enabled = false;
        pauseCanvas.enabled = true;
    }

    public void Resume()
    {
        pauseCanvas.enabled = false;
        gameCanvas.enabled = true;
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("MenuScene");
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

    public void UpdateHealth()
    {
        healthBar.value = Player.Instance.health;
    }
    public void UpdateMana()
    {
        manaBar.value = Player.Instance.mana;
    }

    void OnDestroy()
    {
        controls.Dispose();
    }
}