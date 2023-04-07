using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PokemonData
{
    public Pokemon[] pokemons;
}


[Serializable]
public class Pokemon
{
    public int id;
    public PokemonName name;
    public string[] type;
    public PokemonBaseStat statbase;

    public override string ToString() => $"{id} {name.english}";
}

[Serializable]
public class PokemonName
{
    public string english;
    public string japanese;
    public string chinese;
    public string french;
}

[Serializable]
public class PokemonBaseStat
{
    public int HP;
    public int Attack;
    public int Defense;
    public int SpAttack;
    public int SpDefense;
    public int Speed;

    public int BasePower => HP + Attack + Defense + SpAttack + SpDefense + Speed;
}