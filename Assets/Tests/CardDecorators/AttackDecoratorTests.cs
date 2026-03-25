using System;
using Game.Cards;
using Game.Cards.Strategy;
using Game.GameSystem;
using Game.Lib;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.Cards
{
    // ---------------------------
    // 1. Fake Implementations
    // ---------------------------

    // Fake ICard<TEnum>
    public class FakeCard : ICard<CardTypeEnum>
    {
        public CardTypeEnum Type { get; set; }
        public CardTypeEnum IgnoreLayers { get; set; }
        public string CardName { get; set; }
        public string Description { get; set; }
        public bool isLocked { get; set; }
        public Sprite Icon { get; set; }
        public GameObject Parent { get; set; }
        
        public bool UseCalled { get; private set; }
        public GameObject LastTarget { get; private set; }

        public void Start() { }
        
        public void Use(GameObject target)
        {
            UseCalled = true;
            LastTarget = target;
        }
        
        public void SetLock(bool enabled) => isLocked = enabled;
        
        public void Dispose() { }
    }

    // Fake IDeck<CardData>
    public class FakeDeck : IDeck<CardData>
    {
        public CardData[] cardDatas { get; private set; }
        public int CardCount { get; private set; }

        public FakeDeck(int size = 5)
        {
            cardDatas = new CardData[size];
        }

        public bool AddCard(CardData newCard)
        {
            for (int i = 0; i < cardDatas.Length; i++)
            {
                if (cardDatas[i] == null)
                {
                    cardDatas[i] = newCard;
                    CardCount++;
                    return true;
                }
            }
            return false;
        }

        public void RemoveCard(CardData deletedCard, bool isClearAll)
        {
            for (int i = 0; i < cardDatas.Length; i++)
            {
                if (cardDatas[i] == deletedCard)
                {
                    cardDatas[i] = null;
                    CardCount--;
                    break;
                }
            }
            if (isClearAll)
            {
                Array.Clear(cardDatas, 0, cardDatas.Length);
                CardCount = 0;
            }
        }

        public int GetCardCount() => CardCount;
    }

    // Fake StrategyAttackBase
    public class FakeStrategy : StrategyAttackBase
    {
        public GameObject Target { get; private set; }
        public int DamageValue { get; private set; }
        public bool ExecuteCalled { get; private set; }

        public override string Name => "Fake strategy";

        public override void Execute()
        {
            ExecuteCalled = true;
        }

        public override void UpdateTarget(GameObject target)
        {
            Target = target;
        }

        public override void UpdateDamageValue(int value)
        {
            DamageValue = value;
        }
    }

    // Fake PlayerSystem
    public class FakePlayerSystem : PlayerSystem
    {
        public bool KillCalled { get; private set; }
        
        public override void Kill()
        {
            KillCalled = true;
        }
        
        public override void Respawn() { }
    }

    // ---------------------------
    // 2. Tests
    // ---------------------------

    [TestFixture]
    public class AttackDecoratorTests
    {
        private FakeCard _innerCard;
        private FakePlayerSystem _player;
        private FakeDeck _defendDeck;
        private GameObject _dummyTarget;
        private AttackDecorator _decorator;
        private FakeStrategy _fakeStrategy;

        [SetUp]
        public void SetUp()
        {
            _innerCard = new FakeCard { Type = CardTypeEnum.Attack, Parent = new GameObject() };
            _player = new FakePlayerSystem();
            _defendDeck = new FakeDeck();
            _dummyTarget = new GameObject();

            _decorator = new AttackDecorator(10, _player, _defendDeck, _innerCard);
            _decorator.Start();

            // Replace internal strategy with fake for verification
            _fakeStrategy = new FakeStrategy();
            _decorator.ChangeStrategy(_fakeStrategy);
        }

        [TearDown]
        public void TearDown()
        {
            _decorator?.Dispose();
            
            if (_innerCard.Parent != null) UnityEngine.Object.DestroyImmediate(_innerCard.Parent);
            if (_dummyTarget != null) UnityEngine.Object.DestroyImmediate(_dummyTarget);
        }

        [Test]
        public void Constructor_ShouldSetDefaultDamageValue()
        {
            // Act
            var decorator = new AttackDecorator(15, default, default, default);

            // Assert
            Assert.AreEqual(15, decorator.defaultDamageValue);
        }

        [Test]
        public void ChangeDamage_ShouldModifyCurrentDamageCorrectly()
        {
            // Act
            _decorator.ChangeDamage(5);

            // Assert
            Assert.AreEqual(15, _decorator.currentDamage.Value);    // 10 + 5 = 15
        }

        [Test]
        public void ChangeDamage_ShouldKeepValueNonNegative()
        {
            // Act
            _decorator.ChangeDamage(-20);

            // Assert
            Assert.AreEqual(0, _decorator.currentDamage.Value);
        }

        [Test]
        public void Use_ShouldUpdateStrategyParametersAndExecute()
        {
            // Act
            _decorator.Use(_dummyTarget);

            // Assert
            Assert.AreEqual(_dummyTarget, _fakeStrategy.Target);
            Assert.AreEqual(10, _fakeStrategy.DamageValue);
            Assert.IsTrue(_fakeStrategy.ExecuteCalled);
        }

        [Test]
        public void Use_ShouldCallBaseUse()
        {
            // Act
            _decorator.Use(_dummyTarget);

            // Assert
            Assert.IsTrue(_innerCard.UseCalled);
            Assert.AreEqual(_dummyTarget, _innerCard.LastTarget);
        }

        [Test]
        public void ToString_ShouldReturnFormattedMessage()
        {
            // Arrange
            _decorator.ChangeDamage(0); // ensures currentDamage = defaultDamageValue = 10

            // Act
            string result = _decorator.ToString();

            // Assert
            StringAssert.Contains("Attack: было нанесено 10 урона.", result);
        }
    }
}