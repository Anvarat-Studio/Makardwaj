using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioCollection", menuName = "Data/AudioCollection", order = 1)] 
public class AudioCollection : ScriptableObject
{
    public List<AudioAsset> audioCollection;
}

[System.Serializable]
public class AudioAsset
{
    public string assetName = string.Empty;
    public AudioClip asset;
}
