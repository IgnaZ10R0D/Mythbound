using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpawnConditionInstance : BulletActivationConditionInstance
{
   public override void Initialize(Bullet bullet)
   {
      IsActive = true;
   }

   public override void Tick(float deltaTime)
   {
      //Nothing.
   }
}
