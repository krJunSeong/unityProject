using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FarmUiMouseClick : MonoBehaviour, IPointerClickHandler
{
    private Text buttonText;
    private Slot slot;

    private void Start()
    {
        // 버튼의 자식으로 있는 Text 컴포넌트를 찾습니다.
        buttonText = GetComponentInChildren<Text>();
        slot = GetComponent<Slot>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 코드.
        //if (!EventSystem.current.IsPointerOverGameObject()) { }

            // 왼쪽 클릭일 경우
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            IncreaseNumber();
        }
        // 오른쪽 클릭일 경우
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            DecreaseNumber();
        }
    }

    private void IncreaseNumber()
    {
        if (int.TryParse(buttonText.text, out int number))
        {
            number = Mathf.Min(slot.GetCount(), number + 1);
            buttonText.text = number.ToString();
            
            if(number == slot.GetCount()) buttonText.color = Color.red;
        }
    }

    private void DecreaseNumber()
    {
        if (int.TryParse(buttonText.text, out int number))
        {
            number = Mathf.Max(0, --number);
            buttonText.text = number.ToString();
            buttonText.color = Color.black;
        }
    }
}
