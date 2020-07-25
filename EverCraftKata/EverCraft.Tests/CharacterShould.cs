using EverCraft.Core;
using EverCraft.Tests.Extensions;
using FluentAssertions;
using Xunit;

namespace EverCraft.Tests
{
    public class CharacterShould
    {
        private readonly Character _character;

        public CharacterShould()
        {
            _character = new Character();
        }

        [Fact]
        public void HaveDefaultValuesAtCreation()
        {
            _character.Name.Should().Be("Name");
            _character.Alignment.Should().Be(Alignments.Neutral);
            _character.ArmorClass.Should().Be(10);
            _character.HitPoints.Should().Be(5);

            _character.ExperiencePoints.Should().Be(0);
            _character.Level.Should().Be(1);

            _character.Strength.Value.Should().Be(10);
            _character.Dexterity.Value.Should().Be(10);
            _character.Constitution.Value.Should().Be(10);
            _character.Wisdom.Value.Should().Be(10);
            _character.Intelligence.Value.Should().Be(10);
            _character.Charisma.Value.Should().Be(10);
        }

        [Theory]
        [InlineData("Artemis", Alignments.Good)]
        [InlineData("Harry", Alignments.Neutral)]
        [InlineData("Horrible", Alignments.Evil)]
        public void GetAndSetNameAndAlignment(string name, Alignments alignment)
        {
            _character.Name = name;
            _character.Alignment = alignment;

            _character.Name.Should().Be(name);
            _character.Alignment.Should().Be(alignment);
        }

        [Theory]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        [InlineData(11, true)]
        public void AttackTargetCharacters(int roll, bool expected)
        {
            var target = new Character();
            bool result = _character.Attack(target, roll);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(8, false, 0)]
        [InlineData(9, false, 0)]
        [InlineData(10, true, 1)]
        [InlineData(11, true, 1)]
        [InlineData(20, true, 2)]
        public void DamageTargetOnAttackHit(int roll, bool isHit, int expectedDamage)
        {
            var target = new Character();
            target.HitPoints.Should().Be(5);

            bool result = _character.Attack(target, roll);

            result.Should().Be(isHit);
            target.Damage.Should().Be(expectedDamage);
        }

        [Fact]
        public void BeDeadIfHitPointsAreZero()
        {
            var target = new Character();
            target.HitPoints.Should().Be(5);

            _character.Attack(target, 19);
            _character.Attack(target, 19);
            _character.Attack(target, 19);
            _character.Attack(target, 19);
            target.IsDead.Should().BeFalse();
            _character.Attack(target, 19);

            target.Damage.Should().Be(target.HitPoints);
            target.IsDead.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(8, 9)]
        [InlineData(9, 9)]
        [InlineData(10, 10)]
        [InlineData(11, 10)]
        [InlineData(12, 11)]
        [InlineData(14, 12)]
        [InlineData(17, 13)]
        public void AdjustArmorClassByDexterityModifier(int dexterity, int expected)
        {
            _character.Dexterity = dexterity;

            _character.ArmorClass.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(9, 4)]
        [InlineData(10, 5)]
        [InlineData(11, 5)]
        [InlineData(12, 6)]
        [InlineData(14, 7)]
        [InlineData(20, 10)]
        public void AdjustHitPointsByConstitutionModifier(int constitution, int expected)
        {
            var character = new Character(constitution: constitution);

            character.HitPoints.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(999, 1)]
        [InlineData(1000, 2)]
        [InlineData(1999, 2)]
        [InlineData(2000, 3)]
        [InlineData(4500, 5)]
        public void HaveLevelBasedOnExperiencePoints(int exp, int expectedLevel)
        {
            _character.ExperiencePoints = exp;

            _character.Level.Should().Be(expectedLevel);
        }

        [Theory]
        [InlineData(1, 10, 5)]
        [InlineData(2, 10, 10)]
        [InlineData(2, 14, 14)]
        [InlineData(3, 14, 21)]
        public void HaveHitPointsBasedOnLevel(int level, int constitution, int expected)
        {
            _character.SetLevel(level);

            _character.Constitution = constitution;

            _character.HitPoints.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(4, 2)]
        [InlineData(6, 3)]
        [InlineData(7, 3)]
        public void IncreaseAttackModifierEveryTwoLevels(int level, int expectedModifier)
        {
            _character.SetLevel(level);

            _character.AttackRollModifier.Should().Be(expectedModifier);
        }

        [Theory]
        [InlineData(1, 1, -5)]
        [InlineData(1, 2, -4)]
        [InlineData(1, 20, 5)]
        [InlineData(1, 12, 1)]
        [InlineData(4, 1, -3)]
        [InlineData(4, 2, -2)]
        [InlineData(4, 20, 7)]
        [InlineData(4, 12, 3)]
        public void IncreaseAttackModifierByLevelAndStrengthModifier(int level, int strength, int expected)
        {
            _character.SetLevel(level);

            _character.Strength = strength;

            _character.AttackRollModifier.Should().Be(expected);
        }
    }
}
