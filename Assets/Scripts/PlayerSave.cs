using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    public List<Character> heroes;

    private void Start()
    {
        if(heroes.Count != 4) throw new Exception("This Game is intended to work with 4 heroes.");
    }

    public List<Character> GetHeroes()
    {
        return heroes;
    }
}
