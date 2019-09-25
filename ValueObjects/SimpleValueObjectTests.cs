namespace ValueObjects
{
    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Source http://grabbagoft.blogspot.fr/2007/06/generic-value-object-equality.html
    /// </summary>
    [TestClass]
    public class SimpleValueObjectTests
    {
        [TestMethod]
        public void AddressEqualsIsReflexive()
        {
            var address = new Address("Address1", "Austin", "TX");
            address.Should().Be(address);
        }

        [TestMethod]
        public void AddressEqualsIsSymmetric()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            address.Should().NotBe(address2);
            address2.Should().NotBe(address);
        }

        [TestMethod]
        public void AddressEqualsIsTransitive()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");
            var address3 = new Address("Address1", "Austin", "TX");

            address.Should().Be(address2);
            address2.Should().Be(address3);
            address.Should().Be(address3);
        }

        [TestMethod]
        public void AddressEqualsWorksWithIdenticalAddresses()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");

            address.Should().Be(address2);
        }

        [TestMethod]
        public void AddressEqualsWorksWithNonIdenticalAddresses()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            address.Should().NotBe(address2);
        }

        [TestMethod]
        public void AddressEqualsWorksWithNullObject()
        {
            var address = new Address("Address1", "Austin", "TX");
            Address address2 = null;

            address.Equals(address2).Should().BeFalse();
        }

        [TestMethod]
        public void AddressEqualsWorksWithNulls()
        {
            var address = new Address(null, "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            address.Equals(address2).Should().BeFalse();
        }

        [TestMethod]
        public void AddressEqualsWorksWithNullsOnOtherObject()
        {
            var address = new Address("Address2", "Austin", "TX");
            var address2 = new Address("Address2", null, "TX");

            address.Should().NotBe(address2);
        }

        [TestMethod]
        public void AddressOperatorsWork()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");
            var address3 = new Address("Address2", "Austin", "TX");

            (address == address2).Should().BeTrue();
            (address2 != address3).Should().BeTrue();
        }

        [TestMethod]
        public void DerivedTypesBehaveCorrectly()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX");

            address.Should().NotBe(address2);
            (address == address2).Should().BeFalse();
        }

        [TestMethod]
        public void DerivedTypesHashCodesBehaveCorrectly()
        {
            var address = new ExpandedAddress("Address99999", "Apt 123", "New Orleans", "LA");
            var address2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX");

            address.GetHashCode().Should().NotBe(address2.GetHashCode());
        }

        [TestMethod]
        public void EqualValueObjectsHaveSameHashCode()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");

            address.GetHashCode().Should().Be(address2.GetHashCode());
        }

        [TestMethod]
        public void TransposedValuesGiveDifferentHashCodes()
        {
            var address = new Address(null, "Austin", "TX");
            var address2 = new Address("TX", "Austin", null);

            address.GetHashCode().Should().NotBe(address2.GetHashCode());
        }

        [TestMethod]
        public void TransposedValuesOfFieldNamesGivesDifferentHashCodes()
        {
            var address = new Address("_city", null, null);
            var address2 = new Address(null, "_address1", null);

            address.GetHashCode().Should().NotBe(address2.GetHashCode());
        }

        [TestMethod]
        public void UnequalValueObjectsHaveDifferentHashCodes()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            address.GetHashCode().Should().NotBe(address2.GetHashCode());
        }

        private class Address : SimpleValueObject<Address>
        {
            public Address(string address1, string city, string state)
            {
                this.Address1 = address1;
                this.City = city;
                this.State = state;
            }

            public string Address1 { get; }

            public string City { get; }

            public string State { get; }
        }

        private class ExpandedAddress : Address
        {
            public ExpandedAddress(string address1, string address2, string city, string state)
                : base(address1, city, state)
            {
                this.Address2 = address2;
            }

            public string Address2 { get; }
        }
    }
}