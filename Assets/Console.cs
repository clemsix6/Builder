using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    [SerializeField] private RectTransform       contentTransform;
    [SerializeField] private VerticalLayoutGroup contentVertical;
    [SerializeField] private CommandExecutor     commandExecutor;
    [SerializeField] private TMP_InputField      commandInput;
    [SerializeField] private EventSystem         eventSystem;
    [SerializeField] private TMP_Text            lineText;
    [SerializeField] private float               contentSpacing = 10;
    [SerializeField] private Color               inputColor;
    [SerializeField] private Color               outputColor;

    private List<string> commandsHistory = new();
    private int          historyIndex    = 0;


    private void Update()
    {
        UpdateContentSize();
        MoveHistory();
    }


    private void UpdateContentSize()
    {
        if (contentTransform.childCount <= 0)
        {
            var w = contentTransform.sizeDelta.x;
            contentTransform.sizeDelta = new Vector2(w, 0);
        }
        else
        {
            var child  = (RectTransform)contentTransform.GetChild(0).transform;
            var childH = child.sizeDelta.y;
            var w      = contentTransform.sizeDelta.x;
            var h      = (childH + contentSpacing) * (contentTransform.childCount - 1) + contentVertical.padding.top;

            contentTransform.sizeDelta = new Vector2(w, h);
        }
    }


    private void MoveHistory()
    {
        if (eventSystem.currentSelectedGameObject != commandInput.gameObject)
            return;
        if (Input.GetKeyDown(KeyCode.UpArrow) && historyIndex > 0)
            commandInput.text = commandsHistory[--historyIndex];
        else if (Input.GetKeyDown(KeyCode.DownArrow) && historyIndex < commandsHistory.Count - 1)
            commandInput.text = commandsHistory[++historyIndex];
    }


    public void ExecuteCommand()
    {
        var command = commandInput.text;
        if (command.Replace(" ", "").Length <= 0)
            return;
        var result = commandExecutor.ExecuteCommand(command)
                                    .Replace("ERR:/", "ERROR: ")
                                    .Replace("SUCCESS:/", "");

        AddLine(result, outputColor);
        AddLine("$>\t" + command, inputColor);
        if (!commandsHistory.Any() || commandsHistory.Last() != command)
            commandsHistory.Add(command);
        historyIndex      = commandsHistory.Count;
        commandInput.text = string.Empty;
    }


    private void AddLine(string content, Color color)
    {
        lineText.SetText(content);
        lineText.transform.parent.GetComponent<Image>().color = color;
        var newLine = Instantiate(lineText.transform.parent, contentVertical.transform);
        newLine.gameObject.SetActive(true);
    } 
}
