using System;
using Game.Cards;
using Game.Lib;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.Cards
{
    // ---------------------------
    // 1. Fake Implementations
    // ---------------------------

    // A fake ModifierDecorator that records calls to UpdateModifiers
    public class FakeModifierDecorator : CardDecorator
    {
        public bool UpdateModifiersCalled { get; private set; }
        public GameObject LastUpdateTarget { get; private set; }

        public FakeModifierDecorator(ICard<CardTypeEnum> card) : base(card) { }

        public void UpdateModifiers(GameObject target)
        {
            UpdateModifiersCalled = true;
            LastUpdateTarget = target;
        }
    }

    // ---------------------------
    // 2. Tests
    // ---------------------------

    [TestFixture]
    public class CallBackDecoratorTests
    {
        private FakeCard _innerCard;
        private GameObject _targetObject;
        private GameObject _sourceObject;
        private FakeModifierDecorator _targetModifier;
        private CallBackDecorator _decorator;

        [SetUp]
        public void SetUp()
        {
            // Create source card data
            _innerCard = new FakeCard { Type = CardTypeEnum.Attack, Parent = new GameObject() };
            _decorator = new CallBackDecorator(_innerCard);

            // Create target object with a ModifierDecorator
            _targetObject = new GameObject();
            var targetCardData = _targetObject.AddComponent<CardData>();
            _targetModifier = new FakeModifierDecorator(new FakeCard());
            targetCardData.decorateCard = _targetModifier;

            _sourceObject = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            _decorator?.Dispose();
            if (_innerCard.Parent != null) UnityEngine.Object.DestroyImmediate(_innerCard.Parent);
            if (_targetObject != null) UnityEngine.Object.DestroyImmediate(_targetObject);
            if (_sourceObject != null) UnityEngine.Object.DestroyImmediate(_sourceObject);
        }

        [Test]
        public void Call_WithValidTarget_ShouldInvokeUpdateModifiersOnTarget()
        {
            // Act
            _decorator.Call(_targetObject);

            // Assert
            Assert.IsTrue(_targetModifier.UpdateModifiersCalled);
            Assert.IsNotNull(_targetObject);
            Assert.AreEqual(_targetObject, _targetModifier.LastUpdateTarget);
        }

        [Test]
        public void Call_WhenTargetDoesNotHaveModifierDecorator_ShouldNotThrow()
        {
            // Arrange
            var noModifierTarget = new GameObject();
            var noModifierCardData = noModifierTarget.AddComponent<CardData>();
            noModifierCardData.decorateCard = new FakeCard(); // no ModifierDecorator in chain

            // Act & Assert
            Assert.DoesNotThrow(() => _decorator.Call(noModifierTarget));
        }

        [Test]
        public void Call_WithNullTarget_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _decorator.Call(null));
        }

        [Test]
        public void Call_WhenTargetHasModifierDecoratorButDifferentChain_ShouldStillWork()
        {
            // Arrange
            var target = new GameObject();
            var cardData = target.AddComponent<CardData>();
            var baseCard = new FakeCard();
            var modifier = new FakeModifierDecorator(baseCard);
            cardData.decorateCard = modifier;

            // Act
            _decorator.Call(target);

            // Assert
            Assert.IsTrue(modifier.UpdateModifiersCalled);
            Assert.AreEqual(_sourceObject, modifier.LastUpdateTarget);
        }

        [Test]
        public void Call_ShouldUseSourceCardAsTargetParameter()
        {
            // Arrange
            var sourceCard = new FakeCard();
            var sourceGameObject = new GameObject();
            sourceCard.Parent = sourceGameObject;
            var callbackDecorator = new CallBackDecorator(sourceCard);
            callbackDecorator.Parent = sourceGameObject;

            var target = new GameObject();
            var targetCardData = target.AddComponent<CardData>();
            var targetModifier = new FakeModifierDecorator(new FakeCard());
            targetCardData.decorateCard = targetModifier;

            // Act
            callbackDecorator.Call(target);

            // Assert
            Assert.IsTrue(targetModifier.UpdateModifiersCalled);
            Assert.AreEqual(sourceGameObject, targetModifier.LastUpdateTarget);
        }
    }
}