using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int happiness = 100;
    public int health = 100;
    public int energy = 100;
    public int money = 700;

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

    public void AddHappiness(int value)
    {
        happiness += value;
        if (happiness > 100) happiness = 100;
        if (happiness < 0) happiness = 0;
    }

    public void AddHealth(int value)
    {
        health += value;
        if (health > 100) health = 100;
        if (health < 0) health = 0;
    }

    public void AddEnergy(int value)
    {
        energy += value;
        if (energy > 100) energy = 100;
        if (energy < 0) energy = 0;
    }

    public void AddMoney(int value)
    {
        money += value;

        /*if (money < 0)
            money = 0;*/
    }
}