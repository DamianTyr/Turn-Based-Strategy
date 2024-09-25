using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    
    
    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnOnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExplosion += GrenadeProjectile_OnOnAnyGrenadeExplosion;
        SwordAction.OnAnySwordHit += SwordActionOnOnAnySwordHit;
        PlacedExplosive.onAnyPlacedExplosiveDetonation += PlacedExplosiveOnonAnyPlacedExplosiveDetonation;
    }

    private void PlacedExplosiveOnonAnyPlacedExplosiveDetonation(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(6f);
    }

    private void SwordActionOnOnAnySwordHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(2f);
    }

    private void GrenadeProjectile_OnOnAnyGrenadeExplosion(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }

    private void ShootAction_OnOnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
