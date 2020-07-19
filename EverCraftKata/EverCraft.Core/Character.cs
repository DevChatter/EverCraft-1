using System;

namespace EverCraft.Core
{
    public class Character
    {
        public string Name { get; set; }

        public Alignments Alignment { get; set; }
        public int ArmorClass => 10 + Dexterity.Modifier;
        public int HitPoints { get; private set; }
        public bool IsDead => HitPoints < 1;

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

            HitPoints = Math.Max(1, 5 + Constitution.Modifier);
        }

        public bool Attack(Character target, int roll)
        {
            bool isHit = (roll + Strength.Modifier) >= target.ArmorClass;

            if (isHit)
            {
                int damage = 1 + Strength.Modifier;
                if (roll == 20)
                {
                    damage *= 2;
                }
                target.HitPoints -= Math.Max(1, damage);
            }

            return isHit;
        }
    }
}
