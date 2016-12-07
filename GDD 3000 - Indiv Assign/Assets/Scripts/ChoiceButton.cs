using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceButton : MonoBehaviour
{
    public Text descText;

    ChoiceNode thisChoice;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => { ButtonClick(); });
    }        

    public void LoadChoice(ChoiceNode choice)
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().text = choice.ButtonText;
        thisChoice = choice;
    }

    public void OnMouseEnter()
    {
        if (gameObject.GetComponent<Button>().interactable)
            descText.text = thisChoice.Description;
        else
            descText.text = "Insufficient Resources";
    }

    public void OnMouseExit()
    {
        descText.text = "";
    }

    public void ButtonClick()
    {
        gameObject.transform.parent.transform.parent.GetComponent<Story>().ExecuteChoice(thisChoice);
        descText.text = "";
    }    
}