using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatText : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;

    public void OnInit(float damage) {  
        hpText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    public void OnDespawn() {
        Destroy(gameObject);
    }
}
