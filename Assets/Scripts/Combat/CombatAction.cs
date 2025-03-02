using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO <Implement everything>
namespace CombatAction
{
    public abstract class CombatAction
    {
        public Character User;

        public abstract void Do(Character user);
    }

    public class Attack : CombatAction
    {
        public Character Target;

        public override void Do(Character user)
        {
            int damage = Combat.GetInstance().Attack(user, Target);
        }
    }
    
    public class UseItem : CombatAction
    {
        public Character Target;

        public override void Do(Character user) =>
        throw new System.NotImplementedException();
            //TODO Change if target Dies
    }


    public class Defend : CombatAction
    {
        public override void Do(Character user)
        {
            user.IsDefending = true;
        }
    }

    public class Escape : CombatAction
    {
        public override void Do(Character user)=>
            throw new System.NotImplementedException();
    }

    public class UseSkill : CombatAction
    {
        public List<Character> Targets;
        public override void Do(Character user) =>
            throw new System.NotImplementedException();
    }
}

