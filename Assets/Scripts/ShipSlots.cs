using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSlots : MonoBehaviour
{
    public List<ModuleSlot> Modules;
    public List<WeaponSlot> Weapons;


    public bool TieneArmas()
    {
        bool result = false;
        if(Modules.Count <= 0)
            result = false;
        else
        {
            foreach(ModuleSlot mod in Modules)
            {
                if(mod.data is WeaponModuleData)
                    result = true;
            }
        }

        return result;
    }

    public float GetLongestWeaponRange()
    {        
        float result = 0;
        if(Modules.Count <= 0)
            result = 0;
        else
        {
            foreach(WeaponSlot mod in Weapons)
            {
                float rango = mod.data.range;
                if(rango > result)
                    result = rango;
            }
        }
        return result;
    }

    public void LoadAllData(ModuleData[] genericMods, WeaponModuleData[] weaponMods)
    {
        foreach(ModuleData gData in genericMods)
        {
            ModuleSlot newModule = new ModuleSlot();
            newModule.LoadData(gData);
            Modules.Add(newModule);
        }
        foreach(WeaponModuleData wData in weaponMods)
        {
            WeaponSlot newModule = new WeaponSlot();
            newModule.LoadData(wData);
            newModule.LoadWeaponData(wData);
            Weapons.Add(newModule);
        }
    }
}

[System.Serializable]
public class ModuleSlot {
    public string name;
    public ModuleData data;
    public StarSides side;
    public bool damaged;

    public void LoadData(ModuleData _data)
    {
        data = _data;
        name = _data.name;
    }
}

[System.Serializable]
public class WeaponSlot : ModuleSlot  {
    public WeaponModuleData wData;
    public float range;
    public int damage;
    public float accuracy;

    public void LoadWeaponData(WeaponModuleData _data)
    {
        data = _data;
        wData = _data;
        name = data.name;
        range = data.range;
        damage = wData.damage;
        accuracy = wData.accuracyValue;
    }
}
