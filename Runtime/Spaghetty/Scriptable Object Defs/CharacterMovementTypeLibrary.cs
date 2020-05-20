using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Movement Type Library", menuName = "Dialog/Create Character Movement Type Library", order = 1)]
public class CharacterMovementTypeLibrary : ScriptableObject
{
    [System.Serializable]
    public struct CharacterMovementTypeElement
    {
        public string name;
        public CharacterMovementTypeAsset asset;
    }

    [SerializeField] List<CharacterMovementTypeElement> _assets = null;

    public CharacterMovementTypeAsset this[string key]
    {
        get
        {
            //Debug.Log(_assets.Find((x) => x.name.ToLower().Trim() == key.ToLower().Trim()).name);
            return _assets.Find((x) => x.name.ToLower().Trim() == key.ToLower().Trim()).asset;
        }
    }

    public int Length
    {
        get
        {
            return _assets.Count;
        }
    }
}