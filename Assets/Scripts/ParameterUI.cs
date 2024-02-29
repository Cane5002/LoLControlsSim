using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParameterUI : MonoBehaviour {
    public PlayerController pc;
    public TMP_InputField AttackSpeed;
    public TMP_InputField AttackDelay;
    public TMP_InputField AttackRange;
    public TMP_InputField MovementSpeed;
    public TMP_InputField AttackMoveRadius;

    void Start()
    {
        AttackSpeed.text = pc.AttackSpeed.ToString("0.00");
        AttackDelay.text = pc.AttackDelay.ToString("0.00");
        AttackRange.text = pc.AttackRange.ToString("0.00");
        MovementSpeed.text = pc.MovementSpeed.ToString("0.00");
        AttackMoveRadius.text = pc.AttackMoveSearchRadius.ToString("0.00");
    }

    // Update is called once per frame
    void Update()
    {
        pc.AttackSpeed = float.Parse(AttackSpeed.text);
        pc.AttackDelay = float.Parse(AttackDelay.text);
        pc.AttackRange = float.Parse(AttackRange.text);
        pc.MovementSpeed = float.Parse(MovementSpeed.text);
        pc.AttackMoveSearchRadius = float.Parse(AttackMoveRadius.text);
    }
}
