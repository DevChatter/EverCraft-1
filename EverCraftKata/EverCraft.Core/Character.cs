using System;

namespace EverCraft.Core
{
    public class Character
    {
        private const int ExperienceGainedPerHit = 10;
        private const int MinimumDamage = 1;
        public const int ExperiencePerLevel = 1000;
        private const int LevelsPerAttackIncrease = 2;

        public string Name { get; set; }

        public Alignments Alignment { get; set; }
        public int ArmorClass => 10 + Dexterity.Modifier;
        public int HitPoints => Level * Math.Max(1, 5 + Constitution.Modifier);
        public int Damage { get; private set; }
        public bool IsDead => Damage >= HitPoints;
        public int ExperiencePoints { get; set; }

        public int Level => 1 + ExperiencePoints / ExperiencePerLevel;

        public int AttackRollModifier => Strength.Modifier + Level / LevelsPerAttackIncrease;

        public Attribute Strength { get; set; }
        public Attribute Dexterity { get; set; }
        public Attribute Constitution { get; set; }
        public Attribute Wisdom { get; set; }
        public Attribute Intelligence { get; set; }
        public Attribute Charisma { get; set; }

        public Character(string name = "Name", Alignments alignment = Alignments.Neutral,
            int strength = 10, int dexterity = 10, int constitution = 10,
            int wisdom = 10, int intelligence = 10, int charisma = 10)
        {
            Name = name;
            Alignment = alignment;
            Strength = strength;
            Dexterity = dexterity;
            Constitution = constitution;
            Wisdom = wisdom;
            Intelligence = intelligence;
            Charisma = charisma;
        }

        public bool Attack(Character target, int roll)
        {
            bool isHit = target.IsHitBy(roll + AttackRollModifier);

            if (isHit)
            {
                const int baseDamage = 1;
                int damage = baseDamage + Strength.Modifier;
                if (RollIsCrit(roll))
                {
                    damage *= 2;
                }
                target.InflictDamage(Math.Max(MinimumDamage, damage));
                ExperiencePoints += ExperienceGainedPerHit;
            }

            return isHit;
        }

        private static bool RollIsCrit(int roll) => roll == 20;

        public bool IsHitBy(int modifiedRoll) => modifiedRoll >= ArmorClass;

        public void InflictDamage(int damageTotal) => Damage += damageTotal;
    }
}
