using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PossessionMarker : MonoBehaviour
{
    [SerializeField] private float startScale;
    [SerializeField] private float endScale;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private SpriteRenderer sprite;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LerpScale(startScale, endScale));
    }

    IEnumerator LerpScale(float start, float end)
    {
        float timer = 0;
        while(timer < 1)
        {
            float scale = Mathf.Lerp(start, end, timer);
            transform.localScale = new Vector3(scale, scale, 1.0f);
            

            timer = timer + Time.deltaTime / scaleSpeed;
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = new Vector3(end, end, 1.0f);
        
        RestartScale(start, end);
    }

    void RestartScale(float start, float end)
    {
        if (start == startScale)
        {
            StartCoroutine(LerpScale(endScale, startScale));
        }
        else
        {
            StartCoroutine(LerpScale(startScale, endScale));
        }
        
    }

    public void Activate(Vector3 pos, float scale)
    {
        AdjustScale(scale);
        sprite.enabled = true;
        transform.position = pos;
    }

    public void Deactivate()
    {
        sprite.enabled = false;
    }
    public void AdjustScale(float newScale)
    {
        float diff = endScale - startScale;
        startScale = newScale;
        endScale = newScale + diff;
    }
}
