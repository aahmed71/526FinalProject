using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PossessionText;
    [SerializeField] private TextMeshProUGUI AbilityText;
    [SerializeField] private TextMeshProUGUI AltMovementText;
    [SerializeField] private GameObject PossessionControl;
    [SerializeField] private GameObject AbilityControl;
    [SerializeField] private GameObject AltMovementControl;

    public enum ControlType
    {
        Possession,
        Ability,
        AltMovement
    };
    void Start()
    {

    }

    public void SetText(ControlType type, string text)
    {
        switch (type)
        {
            case ControlType.Possession:
                {
                    PossessionText.text = text;
                    break;
                }
            case ControlType.Ability:
                {
                    AbilityText.text = text;
                    break;
                }
            case ControlType.AltMovement:
                {
                    AltMovementText.text = text;
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    public void SetVisibility(ControlType type, bool visible)
    {
        switch (type)
        {
            case ControlType.Possession:
                {
                    PossessionControl.SetActive(visible);
                    break;
                }
            case ControlType.Ability:
                {
                    AbilityControl.SetActive(visible);
                    break;
                }
            case ControlType.AltMovement:
                {
                    AltMovementControl.SetActive(visible);
                    break;
                }
            default:
                {
                    return;
                }
        }
    }
}
