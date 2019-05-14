using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundSetting : ScriptableObject
{
    public PlayerSFX type;

    public abstract void Play(AudioSource _source);    
}

public enum PlayerSFX {Walking, Punching, Summoning}
