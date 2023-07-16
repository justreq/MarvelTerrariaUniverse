using Microsoft.Xna.Framework;

namespace MarvelTerrariaUniverse.Core.IronMan;
public class Ability
{
    public Vector2 position;
    public Vector2 velocity;
    public int damage;
    public float knockback;

    public Ability(Vector2 position, Vector2 velocity, int damage, float knockback)
    {
        this.position = position;
        this.velocity = velocity;
        this.damage = damage;
        this.knockback = knockback;
    }
}