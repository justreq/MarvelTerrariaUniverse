using System;
using System.Collections.Generic;

namespace MarvelTerrariaUniverse.Core.IronMan;
public class IronManSuit
{
    public int mark;
    public float power;
    public float integrity;
    public float flightSpeed;
    public Dictionary<Type, Ability> abilities;
    public string alias;

    public IronManSuit(int mark, float power, float integrity, float flightSpeed, Dictionary<Type, Ability> abilities, string alias = null)
    {
        this.mark = mark;
        this.power = power;
        this.integrity = integrity;
        this.flightSpeed = flightSpeed;
        this.abilities = abilities;
        this.alias = alias;
    }
}