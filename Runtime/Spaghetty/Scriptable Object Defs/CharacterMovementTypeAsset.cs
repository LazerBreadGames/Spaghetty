using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Movement Type", menuName = "Dialog/Character Movement Type", order = 1)]
public class CharacterMovementTypeAsset : ScriptableObject
{
    [SerializeField] AnimationCurve xPosition = null;
    [SerializeField] AnimationCurve yPosition = null;

    public AnimationCurve XPosition => xPosition;
    public AnimationCurve YPosition => yPosition;
}
