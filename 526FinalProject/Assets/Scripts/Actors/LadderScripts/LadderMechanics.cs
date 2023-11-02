using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class LadderMechanics : EntityController
{
    public bool isVertical = true;
    private Vector2 initialSize;

    public override void Update()
    {
        base.Update();
    }

    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        GameManager.Instance.CalculatePosessionCount("Ladder");
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
       
    }

    void SetVertical()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float offsetX = collider.size.x * 0.5f;
        float offsetY = collider.size.y * 0.5f;
        Vector3 point = new Vector3(0, - offsetX + offsetY, 0);
        transform.position += point;
        transform.Rotate(0, 0, -90);
        isVertical = true;
    }

    void SetHorizontal()
    {
        transform.Rotate(0, 0, 90);

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float offsetX = collider.size.x * 0.5f;
        float offsetY = collider.size.y * 0.5f;
        Vector3 point = new Vector3(0, offsetX - offsetY, 0);
        transform.position += point;

        isVertical = false;
    }

    protected override void Ability()
    {
        if (isVertical)
        {
            SetHorizontal();
        }
        else
        {
            SetVertical();
        }
    }
}
