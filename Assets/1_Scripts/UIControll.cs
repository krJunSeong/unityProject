using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControll : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    Vector2 toPosition;

    [SerializeField, Range(0.0f, 10.0f)]
    float speed = 10;

    public bool isMoveActive = true;
    [SerializeField]
    bool isLerp;

    public bool isX;
    public bool isY;
    public bool isZ;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (isMoveActive)
        {
            Vector2 temp = rectTransform.anchoredPosition;
            if (isLerp)
            {
             temp = Vector2.Lerp(rectTransform.anchoredPosition, toPosition, speed * Time.deltaTime);
            }

            rectTransform.anchoredPosition = temp;
            if (Vector2.Distance(rectTransform.anchoredPosition, toPosition) < 2.0f) isMoveActive = false;
            /*
             * Vector2 tempTopos = new Vector2(rectTransform.position.x, rectTransform.position.y);

            if (isLerp)
            {
                if (isX) tempTopos.x = Mathf.Lerp(rectTransform.position.x, toPosition.x, speed * Time.deltaTime);
                if (isY) tempTopos.y = Mathf.Lerp(rectTransform.position.y, toPosition.y, speed * Time.deltaTime);
            }
            else
                rectTransform.position = toPosition;

            rectTransform.position = tempTopos;

            float temfl = Vector2.Distance(rectTransform.anchoredPosition, toPosition);
            if ( temfl < 1.0f)
            {
                isMoveActive = false;
            }
            */
        }

    }
}
