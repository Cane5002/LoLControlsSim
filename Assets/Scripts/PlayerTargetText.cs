using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTargetText : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private TextMeshProUGUI text;

    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update() {
        if (pc.AttackTarget) text.text = pc.AttackTarget.name;
        else text.text = "";
    }
}
