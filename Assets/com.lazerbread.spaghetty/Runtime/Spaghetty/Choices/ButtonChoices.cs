using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogChoiceStartedListener))]
public class ButtonChoices : MonoBehaviour
{
    public GameObject dialogChoicePrefab;
    private List<DialogChoiceButton> dialogChoices = new List<DialogChoiceButton>();
    private DialogChoiceStartedListener staredListener = null;

    private void Awake()
    {
        staredListener = GetComponent<DialogChoiceStartedListener>();
    }

    public void SpawnButtons(ChoiceData[] choices)
    {
        ClearButtons();
        foreach (ChoiceData choice in choices)
        {
            GameObject choiceButton = Instantiate(dialogChoicePrefab, transform);
            DialogChoiceButton dChoice = choiceButton.GetComponent<DialogChoiceButton>();
            dChoice.ShowText(choice.args[0]);
            dChoice.AddListenerOnClick(() => ChooseChoiceIndex(choice.index));
            dialogChoices.Add(dChoice);
        }
    }

    void ChooseChoiceIndex(int index)
    {
        ClearButtons();
        staredListener.Event.FinishChoice(index);
    }

    void ClearButtons()
    {
        dialogChoices.ForEach(g => Destroy(g.gameObject));
        dialogChoices.Clear();
    }
}
