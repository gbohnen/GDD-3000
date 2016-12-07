using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;
using System;

public class Story : MonoBehaviour
{

    // stats
    public float HitPoints = 2;
    public float Mana = 2;
    public float Stamina = 2;
    public float Food = 3;
    public float Water = 4;
    public float Technology = 5;

    // collections
    public StoryNode[,] nodes;
    public Vector2 currCoords;
    public List<GameObject> buttons;

    // text fields
    public Text titleText;
    public Text bodyText;

    // pagination number
    public int pageCount;
    public int currentPage;

    // ui fill bars
    public Image hpBar;
    public Image manaBar;
    public Image staminaBar;
    public Image foodBar;
    public Image waterBar;
    public Image techbar;

    // buttons
    public GameObject buttonParent;
    public GameObject continueButton;

    public void SetCurrentNode(int x, int y)
    {
        // load the header
        titleText.text = nodes[x, y].Title;

        // load pagination
        pageCount = nodes[x, y].Body.Count;
        currentPage = 0;

        bodyText.text = nodes[(int)currCoords.x, (int)currCoords.y].Body[currentPage];

        // load data
        currCoords = new Vector2(x, y);

        DisableButtons();

        // set buttons
        if (pageCount > 1)
        {
            continueButton.SetActive(true);
        }
        else
            SetChoices();

    }

    public void ClickContinue()
    {
        currentPage++;
        bodyText.text = nodes[(int)currCoords.x, (int)currCoords.y].Body[currentPage];

        if (currentPage >= pageCount - 1)
        {
            continueButton.SetActive(false);
            SetChoices();
        }
    }

    public void ExecuteChoice(ChoiceNode choice)
    {
        HitPoints += choice.HPChange;
        Mana += choice.ManaChange;
        Stamina += choice.StaminaChange;
        Food += choice.FoodChange;
        Water += choice.WaterChange;
        Technology += choice.TechnologyChange;

        SetCurrentNode((int)choice.Target.x, (int)choice.Target.y);

        UpdateBars();
    }

    void Start()
    {
        // load the buttons into the list
        for (int i = 0; i < buttonParent.transform.childCount; i++)
        {
            buttons.Add(buttonParent.transform.GetChild(i).gameObject);
        }

        // load all node data
        LoadXML();

        // jump to first node
        SetCurrentNode(0, 0);

        // update ui bars
        UpdateBars();
    }

    void LoadXML()
    {
        // get xml file
        TextAsset nodeFile = (TextAsset)Resources.Load("NodeData");
        XmlDocument nodeDoc = new XmlDocument();
        nodeDoc.LoadXml(nodeFile.text);

        // set size of node array. sadly, this cannot be done dynamically
        XmlNodeList size_ = nodeDoc.GetElementsByTagName("size");
        string[] size_t = size_[0].InnerText.Split(new char[] { ':' });
        nodes = new StoryNode[int.Parse(size_t[0]), int.Parse(size_t[1])];

        // get nodes
        XmlNodeList nodeList = nodeDoc.GetElementsByTagName("node");

        foreach (XmlNode node in nodeList)
        {
            // get location, then load the node
            string[] location = node.FirstChild.InnerText.Split(new char[] { ':' });
            nodes[Int32.Parse(location[0]), Int32.Parse(location[1])] = ParseNode(node);
        }
    }

    StoryNode ParseNode(XmlNode node)
    {
        StoryNode storyNode = new StoryNode();

        foreach (XmlNode child in node.ChildNodes)
        {
            switch (child.Name)
            {
                case "title":
                    storyNode.Title = child.InnerText;
                    break;
                case "pages":
                    foreach (XmlNode page in child.ChildNodes)
                    {
                        storyNode.Body.Add(page.InnerText);
                    }
                    break;
                case "choices":
                    foreach (XmlNode choice in child.ChildNodes)
                    {
                        storyNode.Choices.Add(ParseChoice(choice));
                    }
                    break;
            }
        }

        return storyNode;
    }

    ChoiceNode ParseChoice(XmlNode choiceNode)
    {
        ChoiceNode choice = new ChoiceNode();

        foreach (XmlNode child in choiceNode.ChildNodes)
        {
            switch (child.Name)
            {
                case "text":
                    choice.ButtonText = child.InnerText;
                    break;
                case "desc":
                    choice.Description = child.InnerText;
                    break;
                case "target":
                    string[] coords = child.InnerText.Split(new char[] { ':' });
                    choice.Target = new Vector2(Int32.Parse(coords[0]), Int32.Parse(coords[1]));
                    break;
                case "hpchange":
                    choice.HPChange = Int32.Parse(child.InnerText);
                    break;
                case "manachange":
                    choice.ManaChange = Int32.Parse(child.InnerText);
                    break;
                case "staminachange":
                    choice.StaminaChange = Int32.Parse(child.InnerText);
                    break;
                case "foodchange":
                    choice.FoodChange = Int32.Parse(child.InnerText);
                    break;
                case "waterchange":
                    choice.WaterChange = Int32.Parse(child.InnerText);
                    break;
                case "techchange":
                    choice.TechnologyChange = Int32.Parse(child.InnerText);
                    break;
                case "requirements":
                    choice.Requirements = child.InnerText;
                    break;
            }
        }

        return choice;
    }

    void UpdateBars()
    {
        hpBar.fillAmount = HitPoints / 20f;
        manaBar.fillAmount = Mana / 20f;
        staminaBar.fillAmount = Stamina / 20f;
        foodBar.fillAmount = Food / 20f;
        waterBar.fillAmount = Water / 20f;
        techbar.fillAmount = Technology / 20f;
    }

    void DisableButtons()
    {
        continueButton.SetActive(false);

        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
            button.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }

    void SetChoices()
    {
        for (int i = 0; i < nodes[(int)currCoords.x, (int)currCoords.y].Choices.Count; i++)
        {
            if (CheckButton(nodes[(int)currCoords.x, (int)currCoords.y].Choices[i].Requirements))
            {
                buttons[i].SetActive(true);
                buttons[i].GetComponent<Button>().interactable = true;
                buttons[i].GetComponent<ChoiceButton>().LoadChoice(nodes[(int)currCoords.x, (int)currCoords.y].Choices[i]);
            }
            else
            {
                buttons[i].SetActive(true);
                buttons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    bool CheckButton(string param)
    {
        bool check = false;

        string[] nums = param.Split(new char[] { ':' });

        if (HitPoints < int.Parse(nums[0]))
        {
            check = true;
        }
        if (Mana < int.Parse(nums[1]))
        {
            check = true;
        }
        if (Stamina < int.Parse(nums[2]))
        {
            check = true;
        }
        if (Food < int.Parse(nums[3]))
        {
            check = true;
        }
        if (Water < int.Parse(nums[4]))
        {
            check = true;
        }
        if (Technology < int.Parse(nums[5]))
        {
            check = true;
        }

        return !check;
    }    
}
