using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI text;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        // int value = Mathf.Clamp((int)(SpawnerManager.Instance.spawnerTimeWait - Time.time), 0, int.MaxValue);
        int value = (int) (SpawnerManager.Instance.spawnerTimeWait - Time.time);
        if (value <= 0)
        {
            text.text = "";
        }
        else
        {
            text.text = value + "s";
        }
    }
}