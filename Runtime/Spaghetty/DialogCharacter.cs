using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterSpriteElement
{
    public string name;
    public Sprite sprite;
}

public class DialogCharacter : MonoBehaviour
{
    [SerializeField] CharacterMovementTypeLibrary _characterMovement = null;
    [SerializeField] CharacterStage _stage = null;
    [SerializeField] SpriteMask HighlightedEffect = null;
    [SerializeField] CharacterSpriteElement[] emotions = null;
    [SerializeField] AnimationCurve _bloopWiggleCurve = null;


    SpriteRenderer spriteRenderer;
    Dictionary<string, Sprite> emotionDict;
    bool selected;
    bool hidden = false;

    void Awake()
    {
        emotionDict = new Dictionary<string, Sprite>();
        foreach (CharacterSpriteElement c in emotions)
        {
            emotionDict.Add(c.name.ToLower(), c.sprite);
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Need to be able to pass movement type here. 
    //Movement type should probably contain all timing information as well as curve stuff
    //Best to define timing information as speed or time to complete movement??
    public void ShowCharacter(string emote, string stagePosition, float delay)
    {
        if (stagePosition != "")
        {
            var _targetTransform = _stage.GetStagePosition(stagePosition);
            transform.SetPositionAndRotation(_targetTransform.position, _targetTransform.rotation);
            transform.localScale = _targetTransform.localScale;
        }
        spriteRenderer.enabled = true;

        IEnumerator waitToSetSprite()
        {
            yield return new WaitForSeconds(delay);
            SetSprite(emote);
        }

        StartCoroutine(waitToSetSprite());
    }

    public void MoveCharacter(string toStagePosition, string animation, float duration)
    {
        StartCoroutine(AnimateCharacter(animation, duration, _stage.GetStagePosition(toStagePosition)));
    }

    public void HideCharacter(string emote, string animation)
    {
        hidden = true;
        SetSprite(emote);
        HighlightedEffect?.gameObject.SetActive(false);
        selected = false;
    }

    public void FlipCharacter(float delay)
    {
        IEnumerator waitToFlip()
        {
            yield return new WaitForSeconds(delay);
            spriteRenderer.flipX = !spriteRenderer.flipX;
            if (HighlightedEffect != null)
            {
                HighlightedEffect.transform.localScale = (spriteRenderer.flipX) ?
                    Vector3.right * -2 + Vector3.one :
                    Vector3.one;
                HighlightedEffect.transform.localPosition = 
                    Vector3.Scale(HighlightedEffect.transform.localPosition, Vector3.left);
            }
        }
        if(selected) StartCoroutine(BloopCharacter());
        StartCoroutine(waitToFlip());
    }

    IEnumerator DoHighlight(float delay, bool value)
    {
        yield return new WaitForSeconds(delay);
        if (value && !hidden && spriteRenderer.enabled && HighlightedEffect != null){
            HighlightedEffect.gameObject.SetActive(value);
        }
    }

    IEnumerator AnimateCharacter(string animation, float duration, Transform to)
    {
        CharacterMovementTypeAsset movement = _characterMovement[animation];
        float timer = 0;
        Quaternion _startRotation = transform.rotation;
        Vector3 _startPos = transform.position;
        while(timer < duration)
        {
            float x = Mathf.Lerp(_startPos.x,
                to.position.x,
                movement.XPosition.Evaluate(timer / duration));
            float y = Mathf.Lerp(_startPos.y,
                to.position.y,
                movement.YPosition.Evaluate(timer / duration));
            transform.position = new Vector3(x, y, transform.position.z);
            transform.rotation = Quaternion.Lerp(_startRotation, to.rotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void SetSprite(string emote)
    {
        if(emotionDict.ContainsKey(emote.ToLower()))
        {
            
            if(selected) StartCoroutine(BloopCharacter());
            spriteRenderer.sprite = emotionDict[emote.ToLower()];
            if (HighlightedEffect != null && spriteRenderer.enabled)
            {
                HighlightedEffect.sprite = emotionDict[emote.ToLower()];
            }
        }
        else
        {
            Debug.LogWarning(emote + " emote for character " + name + " not found.");
        }
    }

    public void SelectCharacter()
    {
        if (gameObject.activeSelf && !selected)
        {
            spriteRenderer.color = Color.white;
            StartCoroutine(DoHighlight(0.0f, true));
            selected = true;
            StartCoroutine(BloopCharacter());
        }
    }

    public void DeselectCharacter(Color fadeColor)
    {
        if (gameObject.activeSelf)
        {
            selected = false;
            spriteRenderer.color = fadeColor;
            if (HighlightedEffect != null)
            {
                HighlightedEffect.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator BloopCharacter()
    {
        float timer = 0;
        Vector3 _startScale = transform.localScale;
        float duration = 0.2f;
        while(timer <= duration)
        {
            timer += Time.deltaTime;
            yield return null;
            transform.localScale = new Vector3(
                _startScale.x * (_bloopWiggleCurve.Evaluate(timer / duration)),
                _startScale.y * (_bloopWiggleCurve.Evaluate(timer / duration)),
                _startScale.z
            );
        }
    }
}
