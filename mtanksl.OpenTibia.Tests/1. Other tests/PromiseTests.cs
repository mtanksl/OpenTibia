using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Tests
{
    [TestClass]
    [TestCategory("1. Other tests")]
    public class PromiseTests
    {
        [TestMethod]
        public void Constructor()
        {
            var promise = new Promise();

            Assert.AreEqual(false, promise.IsCompleted);
        }

        [TestMethod]
        public void TrySetResult()
        {
            var promise = new Promise();

            promise.TrySetResult();

            Assert.AreEqual(true, promise.IsCompleted);
        }

        [TestMethod]
        public void TrySetResultContinuation()
        {
            var success = false;
            var fail = false;

            var promise = new Promise();

            promise.Then( () => { success = true; } ).Catch(ex => { fail = true; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(false, fail);

            promise.TrySetResult();

            Assert.AreEqual(true, success);
            Assert.AreEqual(false, fail);
        }

        [TestMethod]
        public void TrySetResultContinuationException()
        {
            var success = false;
            var fail = false;
            string message = null;

            var promise = new Promise();

            promise.Then( () => { throw new Exception("2"); } ).Then( () => { success = true; } ).Catch(ex => { fail = true; message = ex.Message; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(false, fail);

            promise.TrySetResult();

            Assert.AreEqual(false, success);
            Assert.AreEqual(true, fail);
            Assert.AreEqual("2", message);
        }

        [TestMethod]
        public void TrySetException()
        {
            var promise = new Promise();

            promise.TrySetException(new Exception("1") );

            Assert.AreEqual(true, promise.IsCompleted);
            Assert.AreEqual("1", Assert.ThrowsException<Exception>(promise.Wait).Message);
        }

        [TestMethod]
        public void TrySetExceptionContinuation()
        {
            var success = false;
            var fail = false;

            var promise = new Promise();

            promise.Then( () => { success = true; } ).Catch(ex => { fail = true; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(false, fail);

            promise.TrySetException(new Exception("1") );

            Assert.AreEqual(false, success);
            Assert.AreEqual(true, fail);
        }

        [TestMethod]
        public void TrySetExceptionContinuationException()
        {
            var success = false;
            var fail = false;
            string message = null;

            var promise = new Promise();

            promise.Catch(ex => { throw new Exception("2"); } ).Then( () => { success = true; } ).Catch(ex => { fail = true; message = ex.Message; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(false, fail);

            promise.TrySetException(new Exception("1") );

            Assert.AreEqual(false, success);
            Assert.AreEqual(true, fail);
            Assert.AreEqual("2", message);
        }

        [TestMethod]
        public void Constructor2()
        {
            var promise = new Promise( (resolve, reject) =>
            {
                
            } );

            Assert.AreEqual(false, promise.IsCompleted);
        }

        [TestMethod]
        public void Resolve()
        {
            var promise = new Promise( (resolve, reject) =>
            {
                resolve();
            } );

            Assert.AreEqual(true, promise.IsCompleted);
        }

        [TestMethod]
        public void ResolveContinuation()
        {
            var success = false;
            var fail = false;

            var promise = new Promise( (resolve, reject) =>
            {
                resolve();
            } );

            promise.Then( () => { success = true; } ).Catch(ex => { fail = true; } );

            Assert.AreEqual(true, success);
            Assert.AreEqual(false, fail);
        }

        [TestMethod]
        public void ResolveContinuationException()
        {
            var success = false;
            var fail = false;
            string message = null;

            var promise = new Promise( (resolve, reject) =>
            {
                resolve();
            } );

            promise.Then( () => { throw new Exception("2"); } ).Then( () => { success = true; } ).Catch(ex => { fail = true; message = ex.Message; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(true, fail);
            Assert.AreEqual("2", message);
        }

        [TestMethod]
        public void Reject()
        {
            var promise = new Promise( (resolve, reject) =>
            {
                reject(new Exception("1") );
            } );

            Assert.AreEqual(true, promise.IsCompleted);
            Assert.AreEqual("1", Assert.ThrowsException<Exception>(promise.Wait).Message);
        }

        [TestMethod]
        public void RejectContinuation()
        {
            var success = false;
            var fail = false;

            var promise = new Promise( (resolve, reject) =>
            {
                reject(new Exception("1") );
            } );

            promise.Then( () => { success = true; } ).Catch(ex => { fail = true; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(true, fail);
        }

        [TestMethod]
        public void RejectContinuationException()
        {
            var success = false;
            var fail = false;
            string message = null;

            var promise = new Promise( (resolve, reject) =>
            {
                reject(new Exception("1") );
            } );

            promise.Catch(ex => { throw new Exception("2"); } ).Then( () => { success = true; } ).Catch(ex => { fail = true; message = ex.Message; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(true, fail);
            Assert.AreEqual("2", message);
        }

        [TestMethod]
        public void Reject2()
        {
            var promise = new Promise( (resolve, reject) =>
            {
                throw new Exception("1");
            } );

            Assert.AreEqual(true, promise.IsCompleted);
            Assert.AreEqual("1", Assert.ThrowsException<Exception>(promise.Wait).Message);
        }

        [TestMethod]
        public void Reject2Continuation()
        {
            var success = false;
            var fail = false;

            var promise = new Promise( (resolve, reject) =>
            {
                throw new Exception("1");
            } );

            promise.Then( () => { success = true; } ).Catch(ex => { fail = true; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(true, fail);
        }

        [TestMethod]
        public void Reject2ContinuationException()
        {
            var success = false;
            var fail = false;
            string message = null;

            var promise = new Promise( (resolve, reject) =>
            {
                throw new Exception("1");
            } );

            promise.Catch(ex => { throw new Exception("2"); } ).Then( () => { success = true; } ).Catch(ex => { fail = true; message = ex.Message; } );

            Assert.AreEqual(false, success);
            Assert.AreEqual(true, fail);
            Assert.AreEqual("2", message);
        }

        [TestMethod]
        public void WaitNoTimeout()
        {
            var promise = new Promise( (resolve, reject) =>
            {
                resolve();
            } );

            var success = promise.Wait(10);

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void WaitTimeout()
        {
            var promise = new Promise( (resolve, reject) =>
            {
                
            } );

            var success = promise.Wait(10);

            Assert.AreEqual(false, success);
        }
    }
}