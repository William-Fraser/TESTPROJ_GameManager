using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public static ButtonControl control;
    private float maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        control = this;
        maxHealth = GameManager.manager.health;
    }

    public void LoseHealth()
    {
        if (GameManager.manager.health > 0)
        {
            GameManager.manager.health -= 10;
        }
    }
    public void GainHealth()
    {
        if (GameManager.manager.health < maxHealth)
        {
            GameManager.manager.health += 10;
        }
    }
    
    public void LoseEXP()
    {
        if (GameManager.manager.eXP > 0)
        {
            GameManager.manager.eXP -= 10;
        }
    }
    public void GainEXP()
    {
        GameManager.manager.eXP += 10;
    }
    
    public void LoseScore()
    {
        if (GameManager.manager.score > 0)
        {
            GameManager.manager.score -= 10;
        }
    }
    public void GainScore()
    {
        GameManager.manager.score += 10;
    }
    
    public void LoseShield()
    {
        if (GameManager.manager.shield > 0)
        {
            GameManager.manager.shield -= 10;
        }
    }
    public void GainShield()
    {
        GameManager.manager.shield += 10;
    }

    public void LoseMana()
    {
        if (GameManager.manager.mana > 0)
        {
            GameManager.manager.mana -= 10;
        }
    }
    public void GainMana()
    {
        GameManager.manager.mana += 10;
    }

    public void LoseLife()
    {
        if (GameManager.manager.life > 1)
        {
            GameManager.manager.life -= 1;
        }
    }
    public void GainLife()
    {
        GameManager.manager.life += 1;
    }

    public void Save()
    {
        GameManager.manager.Save();
    }
    public void Load()
    {
        GameManager.manager.Load();
    }

    public void NewStart()
    {
        GameManager.manager.NewStart();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
