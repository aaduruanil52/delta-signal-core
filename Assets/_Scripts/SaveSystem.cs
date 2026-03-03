using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{

    private void Start()
    {
        //ClearSave();
    }
    public void Save(int matches, int turns, int layout)
    {
        PlayerPrefs.SetInt("Matches", matches);
        PlayerPrefs.SetInt("Turns", turns);
        PlayerPrefs.SetInt("Layout", layout);
        PlayerPrefs.Save();
    }

    public bool HasSave()
    {
        return PlayerPrefs.HasKey("Matches");
    }

    public int GetMatches()
    {
        return PlayerPrefs.GetInt("Matches");
    }

    public int GetTurns()
    {
        return PlayerPrefs.GetInt("Turns");
    }

    public int GetLayout()
    {
        return PlayerPrefs.GetInt("Layout");
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
