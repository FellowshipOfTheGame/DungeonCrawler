using UnityEngine;

public static class AttributeCalculation
{
    public static float AttackCalculation(int strenght, float weaponDamage, float abilityDamage, float addOrSub = 0, float multiplier = 1, int lvlChar = 1, int lvlTarget = 1)
    {
        // Atq = (For + DanArma + Hab + Add/Sub) * Mult     ( Add/Sub e Mult opcionais )
        // (Opcional) Se lvlPersonagem < lvlAlvo, Atq = Atq * ( 1 - (lvlAlvo - lvlPersonagem) / 100 )

        float attack = (strenght + weaponDamage + abilityDamage + addOrSub) * multiplier;

        if (lvlChar < lvlTarget)
        {
            float levelDifference = (float)(lvlTarget - lvlChar) / 100;
            attack *= (1 - levelDifference);
        }

        return attack;
    }

    public static float DefenceCalculation(int constitution, float armorDefence, float addOrSub = 0, float multiplier = 1)
    {
        // Def = (Con + DefArmadura + Add/Sub) * Mult       ( Add/Sub e Mult opcionais )

        float defense = (constitution + armorDefence + addOrSub) * multiplier;
        return defense;
    }

    public static float MagicCalculation(int intelligence, float spellClass, float abilityDamage, float addOrSub = 0, float multiplier = 1)
    {
        // OBS: Existe int e knw, qual a diferenca dos 2??

        // Mag = (ClasseMagica * (Int + Hab) + Add/Sub) * Mult     ( Add/Sub e Mult opcionais )

        float magic = (spellClass * (intelligence + abilityDamage) + addOrSub) * multiplier;
        return magic;
    }

    public static float WillpowerCalculation(int intelligence, float addOrSub = 0, float multiplier = 1)
    {
        // Vont = (Int + Add/Sub) * Mult       ( Add/Sub e Mult opcionais )
        float willpower = (intelligence + addOrSub) * multiplier;
        return willpower;
    }

    public static float HitChanceCalculation(int dexterityChar, int dexterityTarget)
    {
        // OBS: Existe agility e dexterity, qual a diferenca dos 2??

        // Acerto = (DexChar / DexAlvo * 2) - 0.05
        // Se Acerto < 0, Acerto = 0

        float hitChance = ((float)dexterityChar / ((float)dexterityTarget * 2)) - 0.05f;

        if (hitChance < 0)
            hitChance = 0;

        return hitChance;
    }

    public static int SanityCalculation(int intelligence, int constitution)
    {
        // San = (Int + Vit) * 5

        int sanity = (intelligence + constitution) * 5;
        return sanity;
    }

    public static float DamageCalculation(float attack, float targetDefence, float deviation = 0)
    {
        // OBS: A forma que as afinidades sao usadas nao esta nesta funcao
        // A funcao que define as afinidades deve ter acesso aos personagens
        // para que seja possivel fazer repel e absorcao

        /* Modelo para funcoes de afinidade
         * if (!repel && !absorb) dano = dano * afinidade;
         * else if (repel) 
         *      if (!repelled) dano = 0 e dano de volta, else dano = 0
         * else if (absorb) dano *= -1
         */


        // Dano = (Atq - Def) * random(1 - desvio, 1 + desvio)
        // Se Dano < 0, Dano = 0

        // Como serao calculados os criticos??

        float damage = (attack - targetDefence) * Random.Range(1 - deviation, 1 + deviation);

        if (damage < 0)
            damage = 0;

        return damage;
    }
}
