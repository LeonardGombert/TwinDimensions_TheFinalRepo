using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundSettingStatic
{
    public static List<SoundSetting> GetSSOfType(this List<SoundSetting> _ss, PlayerSFX _type)
    {
        List<SoundSetting> result = new List<SoundSetting>();

        foreach (SoundSetting item in _ss)
        {
            if(item.type == _type) result.Add(item);
        }

        return result;
    }
}
