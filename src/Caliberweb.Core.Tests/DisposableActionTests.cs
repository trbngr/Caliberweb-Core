using System;
using System.IO;

using NUnit.Framework;

using Caliberweb.Core.Extensions;

namespace Caliberweb.Core
{
    [TestFixture]
    public class DisposableActionTests
    {
        private DisposableAction action;
        private bool actionWasRun;

        [SetUp]
        public virtual void SetUp()
        {
            actionWasRun = false;
            action = new DisposableAction(() => actionWasRun = true);
        }

        [Test]
        public void action_will_run_when_exiting_using_statement()
        {
            using (action)
            {
                Console.Out.WriteLine("doing work...");
            }

            Assert.IsTrue(actionWasRun);
        }

        [Test]
        public void action_will_run_when_calling_dispose()
        {
            action.Dispose();

            Assert.IsTrue(actionWasRun);
        }
    }

    [TestFixture]
    public class CommonUsageDisposableActionTests
    {
        [Test]
        public void TEST_NAME()
        {
            var watcher = new FileSystemWatcher();

            using(watcher.SubscribeToAllEvents(s=>Console.Out.WriteLine(s)))
            {
                Console.Out.WriteLine("");
            }
        }

        [Test]
        public void will_usage_an_alias_for_an_operation()
        {
            const string REAL_NAME = "Chris Martin";
            const string ALIAS = "factoryd";
            
            var person = new Person(REAL_NAME);

            Assert.AreEqual(REAL_NAME, person.Name);

            using(person.Alias(ALIAS))
            {
                Assert.AreEqual(ALIAS, person.Name);
            }

            Assert.AreEqual(REAL_NAME, person.Name);
        }

        public class Person
        {
            public string Name { get; private set; }

            public Person(string name)
            {
                Name = name;
            }

            public IDisposable Alias(string alias)
            {
                var realName = Name;
                Name = alias;
                return new DisposableAction(() => Name = realName);
            }
        }
    }
}