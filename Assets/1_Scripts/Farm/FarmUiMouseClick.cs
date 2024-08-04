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
        // ��ư�� �ڽ����� �ִ� Text ������Ʈ�� ã���ϴ�.
        buttonText = GetComponentInChildren<Text>();
        slot = GetComponent<Slot>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �ڵ�.
        //if (!EventSystem.current.IsPointerOverGameObject()) { }

            // ���� Ŭ���� ���
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            IncreaseNumber();
        }
        // ������ Ŭ���� ���
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
