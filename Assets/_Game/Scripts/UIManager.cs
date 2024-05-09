using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // public static UIManager Instance
    // {
    //     get {
    //         if(instance == null) {
    //             instance = FindObjectOfType<UIManager>();
    //         }

    //         return instance;
    //     }
    // }

    private void Awake() {
        instance = this;
    }

    [SerializeField] TMP_Text coinText;

    public void SetCoin(int coin) {
        coinText.text = coin.ToString();
    }
}
