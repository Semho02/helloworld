using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamag
{
    public int Health { set; get; }
    public bool Targetable { set; get; }
    public void OnHit(int damage, Vector2 knockback, bool isPlayer);
    public void OnHit(int damage);
    public void OnObjectDestroyed();
}