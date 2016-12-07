using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryNode {

    public string Title
    { get; set; }

    public List<string> Body
    { get; set; }

    public List<ChoiceNode> Choices
    { get; set; }

    public StoryNode()
    {
        Choices = new List<ChoiceNode>();
        Body = new List<string>();
    }
}

public class ChoiceNode
{
    public string ButtonText
    { get; set; }

    public string Description
    { get; set; }

    public Vector2 Target
    { get; set; }

    public int HPChange
    { get; set; }

    public int ManaChange
    { get; set; }

    public int StaminaChange
    { get; set; }

    public int FoodChange
    { get; set; }

    public int WaterChange
    { get; set; }

    public int TechnologyChange
    { get; set; }

    public string Requirements
    { get; set; }
}
