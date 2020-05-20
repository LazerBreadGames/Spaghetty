using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDirector : MonoBehaviour
{
    public Color spriteFadeColor;
    [SerializeField] private List<DialogCharacter> characters = null;

    public void HideAllCharacters()
    {

    }

    public void ParseShowCharacterEvent(string[] args)
    {
        switch (args.Length)
        {
            case 2:
                ShowCharacter(args[0], args[1]);
                break;
            case 3:
                float delay;
                if(float.TryParse(args[2].Trim(), out delay))
                {
                    ShowCharacter(args[0], args[1], "", delay);
                }
                else
                {
                    ShowCharacter(args[0], args[1], args[2]);
                }
                break;
            case 4:
                ShowCharacter(args[0], args[1], args[2], float.Parse(args[3]));
                break;
            default:
                throw new System.ArgumentException($"Invalid number of args for Show Character Event.");
        }
    }

    public void ShowCharacter(string name, string sprite, string stagePosition = "", float delay = 0)
    {
        var character = characters.Find(x => x.name.ToLower() == name.ToLower());
        character?.ShowCharacter(sprite, stagePosition, delay);
    }

    public void HideCharacter(string name, string sprite, string anim)
    {
        var character = characters.Find(x => x.name.ToLower() == name.ToLower());
        character?.HideCharacter(sprite, anim);
    }

    public void MoveCharacter(string name, string toStagePosition, string anim, string duration)
    {
        var character = characters.Find(x => x.name.ToLower() == name.ToLower());
        Debug.Log(character.name);
        character?.MoveCharacter(toStagePosition, anim, float.Parse(duration.Trim()));
    }

    public void FlipCharacter(string[] args)
    {
        var character = characters.Find(x => x.name.ToLower() == args[0].ToLower());

        float delay;
        if (args.Length > 1 && float.TryParse(args[1].Trim(), out delay))
        {
            character?.FlipCharacter(delay);
        }
        else
        {
            character?.FlipCharacter(0);
        }
    }

    public void HighlightCharacter(string name)
    {
        foreach (DialogCharacter character in characters)
        {
            if (name != null && name.ToLower() == character.gameObject.name.ToLower())
            {
                character.SelectCharacter();
            }
            else
            {
                character.DeselectCharacter(spriteFadeColor);
            }
        }
    }
}
