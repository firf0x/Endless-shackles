using System;
using System.Collections.Generic;
using Game.Cards;
using Game.Cards.Modifier;
using Game.GameSystem;
using Game.Lib;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.Cards
{
    public class FakeModifier : ModifierBase
    {
        public bool isUseble { get; private set; } = false; 

        public override void OnCallBack(ModifierContext context)
        {
            isUseble = true;
        }
    }

    [TestFixture]
    public class CallBackDecoratorTests
    {
        private FakeCard _innerCard;
        private GameObject _targetObject;
        private GameObject _sourceObject;
        private ModifierDecorator _targetModifier;
        private CallBackDecorator _decorator;

        [SetUp]
        public void SetUp()
        {
            // Create source card data
            _sourceObject = new GameObject("Source Card");
            _sourceObject.AddComponent<CardData>();
            _innerCard = new FakeCard { Type = CardTypeEnum.Attack, Parent = _sourceObject };
            _sourceObject.GetComponent<CardData>().decorateCard = _innerCard;
            _decorator = new CallBackDecorator(_sourceObject.GetComponent<CardData>().decorateCard);

            // Create target object with a ModifierDecorator
            _targetObject = new GameObject("General Target Object");
            _targetObject.AddComponent<CardData>();
            
            var modifierList = new List<ModifierBase>();
            modifierList.Add(new FakeModifier());

            _targetModifier = new ModifierDecorator(modifierList, default, default, default, default, new FakeCard());
            _targetObject.GetComponent<CardData>().decorateCard = _targetModifier;
            _targetObject.GetComponent<CardData>().decorateCard.Parent = _targetObject;
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

            var a = _targetModifier.Modifiers[0].Modifier as FakeModifier;

            // Assert
            Assert.IsTrue(a.isUseble);
        }

        // [Test]
        // public void Call_WhenTargetDoesNotHaveModifierDecorator_ShouldNotThrow()
        // {
        //     // Arrange
        //     var noModifierTarget = new GameObject();
        //     var noModifierCardData = noModifierTarget.AddComponent<CardData>();
        //     noModifierCardData.decorateCard = new FakeCard(); // no ModifierDecorator in chain

        //     // Act & Assert
        //     Assert.DoesNotThrow(() => _decorator.Call(noModifierTarget));
        // }

        // [Test]
        // public void Call_WithNullTarget_ShouldNotThrow()
        // {
        //     // Act & Assert
        //     Assert.DoesNotThrow(() => _decorator.Call(null));
        // }

        // [Test]
        // public void Call_WhenTargetHasModifierDecoratorButDifferentChain_ShouldStillWork()
        // {
        //     // Arrange
        //     var target = new GameObject();
        //     var cardData = target.AddComponent<CardData>();
        //     var baseCard = new FakeCard();
        //     var modifier = new FakeModifierDecorator(baseCard);
        //     cardData.decorateCard = modifier;

        //     // Act
        //     _decorator.Call(target);

        //     // Assert
        //     Assert.IsTrue(modifier.UpdateModifiersCalled);
        //     Assert.AreEqual(_sourceObject, modifier.LastUpdateTarget);
        // }

        // [Test]
        // public void Call_ShouldUseSourceCardAsTargetParameter()
        // {
        //     // Arrange
        //     var sourceCard = new FakeCard();
        //     var sourceGameObject = new GameObject();
        //     sourceCard.Parent = sourceGameObject;
        //     var callbackDecorator = new CallBackDecorator(sourceCard);
        //     callbackDecorator.Parent = sourceGameObject;

        //     var target = new GameObject();
        //     var targetCardData = target.AddComponent<CardData>();
        //     var targetModifier = new FakeModifierDecorator(new FakeCard());
        //     targetCardData.decorateCard = targetModifier;

        //     // Act
        //     callbackDecorator.Call(target);

        //     // Assert
        //     Assert.IsTrue(targetModifier.UpdateModifiersCalled);
        //     Assert.AreEqual(sourceGameObject, targetModifier.LastUpdateTarget);
        // }
    }
}