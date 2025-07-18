using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.ValueObjects
{
    public class Address
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public string FormattedAddress { get; private set; }
        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }

        private Address() { } // For EF Core

        public Address(string street, string city, string state, string postalCode, string country)
        {
            ValidateAddress(street, city, state, postalCode, country);
            
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
            FormattedAddress = FormatAddress(street, city, state, postalCode, country);
        }
        
        public void SetCoordinates(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new DomainException("Latitude must be between -90 and 90");
                
            if (longitude < -180 || longitude > 180)
                throw new DomainException("Longitude must be between -180 and 180");
                
            Latitude = latitude;
            Longitude = longitude;
        }
        
        private static void ValidateAddress(string street, string city, string state, string postalCode, string country)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new DomainException("Street is required");
                
            if (street.Length > 200)
                throw new DomainException("Street cannot be longer than 200 characters");
                
            if (string.IsNullOrWhiteSpace(city))
                throw new DomainException("City is required");
                
            if (city.Length > 100)
                throw new DomainException("City cannot be longer than 100 characters");
                
            if (string.IsNullOrWhiteSpace(state))
                throw new DomainException("State is required");
                
            if (state.Length > 100)
                throw new DomainException("State cannot be longer than 100 characters");
                
            if (postalCode != null && postalCode.Length > 20)
                throw new DomainException("Postal code cannot be longer than 20 characters");
                
            if (string.IsNullOrWhiteSpace(country))
                throw new DomainException("Country is required");
                
            if (country.Length > 100)
                throw new DomainException("Country cannot be longer than 100 characters");
        }
        
        private static string FormatAddress(string street, string city, string state, string postalCode, string country)
        {
            return $"{street}, {city}, {state} {postalCode}, {country}";
        }

        public override string ToString()
        {
            return FormattedAddress;
        }

        // Equality overrides
        public override bool Equals(object obj)
        {
            if (obj is not Address address)
                return false;

            return Street == address.Street &&
                   City == address.City &&
                   State == address.State &&
                   PostalCode == address.PostalCode &&
                   Country == address.Country;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Street, City, State, PostalCode, Country);
        }

        public static bool operator ==(Address left, Address right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !(left == right);
        }
    }
} 