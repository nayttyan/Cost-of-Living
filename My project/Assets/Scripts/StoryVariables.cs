using System.Collections.Generic;
using UnityEngine;

public class StoryVariables : MonoBehaviour
{
    public static StoryVariables instance;

    Dictionary<string, bool> bools = new Dictionary<string, bool>();
    Dictionary<string, int> ints = new Dictionary<string, int>();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetBool(string key, bool value)
    {
        bools[key] = value;
    }

    public bool GetBool(string key)
    {
        if (bools.ContainsKey(key))
            return bools[key];

        return false;
    }

    public void AddInt(string key, int value)
    {
        if (!ints.ContainsKey(key))
            ints[key] = 0;

        ints[key] += value;
    }

    public int GetInt(string key)
    {
        if (ints.ContainsKey(key))
            return ints[key];

        return 0;
    }
}