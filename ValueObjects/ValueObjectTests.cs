namespace ValueObjects
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValueObjectTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void AddressEqualsIsReflexive()
        {
            Address address = new Address("Address1", "Austin", "TX", new List<Tenant>());
            address.Should().Be(address);
        }

        [TestMethod]
        public void AddressEqualsIsTransitive()
        {
            Address address = new Address("Address1", "Austin", "TX", new List<Tenant>());
            Address address2 = new Address("Address1", "Austin", "TX", new List<Tenant>());
            Address address3 = new Address("Address1", "Austin", "TX", new List<Tenant>());

            address.Should().Be(address2);
            address2.Should().Be(address3);
            address.Should().Be(address3);
        }

        [TestMethod]
        public void AddressEqualsWorksWithIdenticalAddresses()
        {
            Address address = new Address(
                "Address1",
                "Austin",
                "TX",
                new List<Tenant>
                    {
                        new Tenant("john")
                    });

            Address address2 = new Address(
                "Address1",
                "Austin",
                "TX",
                new List<Tenant>
                    {
                        new Tenant("john")
                    });

            address.Should().Be(address2);
        }

        [TestMethod]
        public void AddressEqualsWorksWithIdenticalAddressesExcludingState()
        {
            Address address = new Address("Address1", "Austin", "AZ", new List<Tenant>());
            Address address2 = new Address("Address1", "Austin", "TX", new List<Tenant>());

            address.Should().Be(address2);
        }

        [TestMethod]
        public void AddressEqualsWorksWithNonIdenticalAddresses()
        {
            Address address = new Address("Address1", "Austin", "TX", new List<Tenant>());
            Address address2 = new Address("Address2", "Austin", "TX", new List<Tenant>());

            address.Should().NotBe(address2);
        }

        [TestMethod]
        public void AddressEqualsWorksWithNullObject()
        {
            Address address = new Address("Address1", "Austin", "TX", new List<Tenant>());
            Address address2 = null;
            address.Should().NotBe(address2);
        }

        [TestMethod]
        public void AddressOperatorsWorksWithNullObject()
        {
            Address address = new Address("Address1", "Austin", "TX", new List<Tenant>());
            Address address2 = null;
            (address == address2).Should().BeFalse();
            (address != address2).Should().BeTrue();

            address = null;
            (address == address2).Should().BeTrue();
            (address != address2).Should().BeFalse();
        }

        [TestMethod]
        public void AddressEqualsWorksWithNulls()
        {
            Address address = new Address(null, "Austin", "TX", new List<Tenant>());
            Address address2 = new Address("Address2", "Austin", "TX", new List<Tenant>());

            address.Should().NotBe(address2);
        }

        [TestMethod]
        public void AddressEqualsWorksWithNullsOnOtherObject()
        {
            Address address = new Address("Address2", "Austin", "TX", new List<Tenant>());
            Address address2 = new Address("Address2", "Austin", "TX", null);

            address.Should().NotBe(address2);
        }

        [TestMethod]
        public void AddressOperatorsWork()
        {
            Address address = new Address("Address1", "Austin", "TX", new List<Tenant>());
            Address address2 = new Address("Address1", "Austin", "TX", new List<Tenant>());
            Address address3 = new Address("Address2", "Austin", "TX", new List<Tenant>());

            (address == address2).Should().BeTrue();
            (address2 != address3).Should().BeTrue();
        }

        [TestMethod]
        public void DerivedTypesBehaveCorrectly()
        {
            Address address = new Address("Address1", "Austin", "TX", new List<Tenant>());
            ExpandedAddress address2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX", new List<Tenant>());

            Action a = () => address.Equals(address2);
            a.Should().Throw<Exception>();

            a = () =>
                {
                    if (address == address2)
                    {
                    }
                };
            a.Should().Throw<Exception>();
        }

        [TestMethod]
        public void DerivedTypesHashCodesBehaveCorrectly()
        {
            ExpandedAddress address = new ExpandedAddress("Address99999", "Apt 123", "New Orleans", "LA", new List<Tenant>());
            ExpandedAddress address2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX", new List<Tenant>());

            address.GetHashCode().Should().NotBe(address2.GetHashCode());
        }

        [TestMethod]
        public void EqualValueObjectsHaveSameHashCode()
        {
            Address address = new Address(
                "Address1",
                "Austin",
                "TX",
                new List<Tenant>
                    {
                        new Tenant("john")
                    });
            Address address2 = new Address(
                "Address1",
                "Austin",
                "TX",
                new List<Tenant>
                    {
                        new Tenant("john")
                    });

            address.GetHashCode().Should().Be(address2.GetHashCode());
        }

        [TestMethod]
        public void TransposedValuesGiveDifferentHashCodes()
        {
            Address address = new Address(null, "Austin", "TX", null);
            Address address2 = new Address("TX", "Austin", null, null);

            address.GetHashCode().Should().NotBe(address2.GetHashCode());
        }

        [TestMethod]
        public void UnequalValueObjectsHaveDifferentHashCodes()
        {
            Address address = new Address(
                "Address1",
                "Austin",
                "TX",
                new List<Tenant>
                    {
                        new Tenant("john")
                    });
            Address address2 = new Address(
                "Address2",
                "Austin",
                "TX",
                new List<Tenant>
                    {
                        new Tenant("marc")
                    });

            address.GetHashCode().Should().NotBe(address2.GetHashCode());
        }

        [TestMethod]
        public void MoneyEqualsWithSameAmountWhenEqualityRulesOverrided()
        {
            Money money = new Money("FR", 5.456m);
            Money money2 = new Money("fr", 5.46m);

            money.Should().Be(money2);
        }


        #endregion

        private class Address : ValueObject
        {
            #region Constructors and Destructors

            public Address(string street, string city, string state, List<Tenant> tenants)
            {
                this.Street = street;
                this.City = city;
                this.State = state;
                this.Tenants = tenants;
            }

            #endregion

            #region Public Properties

            public string Street { get; }

            public string City { get; }

            public string State { get; }

            public List<Tenant> Tenants { get; }

            #endregion

            #region Methods

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return this.Street;
                yield return this.City;

                if (this.Tenants == null)
                {
                    yield return null;
                }
                else
                {
                    foreach (Tenant tenant in this.Tenants)
                    {
                        yield return tenant;
                    }
                }
            }

            #endregion
        }

        private class ExpandedAddress : Address
        {
            #region Constructors and Destructors

            public ExpandedAddress(string address1, string address2, string city, string state, List<Tenant> tenants)
                : base(address1, city, state, tenants)
            {
                this.Address2 = address2;
            }

            #endregion

            #region Public Properties

            public string Address2 { get; }

            #endregion

            #region Methods

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return this.Street;
                yield return this.City;
                yield return this.Address2;

                if (this.Tenants == null)
                {
                    yield return null;
                }
                else
                {
                    foreach (Tenant tenant in this.Tenants)
                    {
                        yield return tenant;
                    }
                }
            }

            #endregion
        }

        private class Money : ValueObject
        {
            #region Constructors and Destructors

            public Money(string currency, decimal amount)
            {
                this.Currency = currency;
                this.Amount = amount;
            }

            #endregion

            #region Public Properties

            public string Currency { get; }

            public decimal Amount { get; }

            #endregion

            #region Methods

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return this.Currency.ToUpper();
                yield return Math.Round(this.Amount, 2);
            }

            #endregion
        }

        private class Tenant : ValueObject
        {
            #region Constructors and Destructors

            public Tenant(string name)
            {
                this.Name = name;
            }

            #endregion

            #region Public Properties

            public string Name { get; }

            #endregion

            #region Methods

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return this.Name;
            }

            #endregion
        }
    }
}