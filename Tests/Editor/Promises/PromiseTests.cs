﻿using NUnit.Framework;
using System;

namespace Padoru.Core.Tests
{
    [TestFixture]
    public class PromiseTests
    {
        private bool completed = false;
        private bool failed = false;

        [Test]
        public void Complete_WhenNotCompletedAndNotFailed_ShouldNotThrowAndShouldRunCallback()
        {
            completed = false;
            var promise = new Promise();
            promise.OnComplete(OnCompleteCallback);

            Assert.DoesNotThrow(promise.Complete);
            Assert.IsTrue(completed);
        }

        [Test]
        public void Complete_WhenCompleted_ShouldThrow()
        {
            var promise = new Promise();
            promise.Complete();

            Assert.Throws<Exception>(promise.Complete);
        }

        [Test]
        public void Complete_WhenFailed_ShouldThrow()
        {
            var promise = new Promise();
            promise.Fail(new Exception());

            Assert.Throws<Exception>(promise.Complete);
        }

        [Test]
        public void Complete_WhenReseted_ShouldNotThrowAndShouldRunCallback()
        {
            completed = false;
            var promise = new Promise();
            promise.Complete();
            promise.Reset();
            promise.OnComplete(OnCompleteCallback);

            Assert.DoesNotThrow(promise.Complete);
            Assert.IsTrue(completed);
        }

        [Test]
        public void Fail_WhenNotCompletedAndNotFailed_ShouldNotThrowAndShouldRunCallback()
        {
            failed = false;
            var promise = new Promise();
            promise.OnFail(OnFailCallback);

            Assert.DoesNotThrow(() => promise.Fail(new Exception()));
            Assert.IsTrue(failed);
        }

        [Test]
        public void Fail_WhenCompleted_ShouldThrow()
        {
            var promise = new Promise();
            promise.Complete();

            Assert.Throws<Exception>(() => promise.Fail(new Exception()));
        }

        [Test]
        public void Fail_WhenFailed_ShouldThrow()
        {
            var promise = new Promise();
            promise.Fail(new Exception());

            Assert.Throws<Exception>(() => promise.Fail(new Exception()));
        }

        [Test]
        public void Fail_WhenReseted_ShouldNotThrowAndShouldRunCallback()
        {
            failed = false;
            var promise = new Promise();
            promise.Fail(new Exception());
            promise.Reset();
            promise.OnFail(OnFailCallback);

            Assert.DoesNotThrow(() => promise.Fail(new Exception()));
            Assert.IsTrue(failed);
        }

        [Test]
        public void Fail_WhenExceptionNull_ShouldThrow()
        {
            var promise = new Promise();

            Assert.Throws<Exception>(() => promise.Fail(null));
        }

        private void OnCompleteCallback()
        {
            completed = true;
        }

        private void OnFailCallback(Exception e)
        {
            failed = true;
        }
    }
}