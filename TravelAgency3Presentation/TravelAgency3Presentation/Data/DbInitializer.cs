using Microsoft.AspNetCore.Http.HttpResults;
using TravelAgency3Presentation.Context;
using TravelAgency3Presentation.Models;
using TravelAgency3Presentation.Models.Enums;

namespace TravelAgency3Presentation.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            // Check if DB has been seeded
            if (context.Locations.Any())
            {
                return; // DB has been seeded
            }
            // Seed Locations
            var locations = new Location[]
            {
                new Location
                {
                    Name = "Santorini Villa",
                    Country = "Greece",
                    Description = "Experience the magical sunset views from this traditional Cycladic villa overlooking the caldera. Perfect for couples or small families seeking a luxurious getaway.",
                    PricePerNight = 250.00M,
                    ImageUrl = "/images/locations/santorini.jpg",
                    IsPopular = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new Location
                {
                    Name = "Bali Beach Resort",
                    Country = "Indonesia",
                    Description = "This beachfront resort offers private bungalows surrounded by tropical gardens, with direct access to a pristine white sand beach and crystal clear waters.",
                    PricePerNight = 180.00M,
                    ImageUrl = "/images/locations/bali.jpg",
                    IsPopular = true,
                    CreatedAt = DateTime.Now.AddMonths(-5)
                },
                new Location
                {
                    Name = "Kyoto Traditional Inn",
                    Country = "Japan",
                    Description = "Stay in an authentic Japanese ryokan with tatami floors, sliding paper doors, and an onsen hot spring bath. Experience traditional Japanese hospitality at its finest.",
                    PricePerNight = 200.00M,
                    ImageUrl = "/images/locations/kyoto.jpg",
                    IsPopular = true,
                    CreatedAt = DateTime.Now.AddMonths(-4)
                },
                new Location
                {
                    Name = "Paris Apartment",
                    Country = "France",
                    Description = "Charming Parisian apartment in the heart of Montmartre with stunning views of the Eiffel Tower. Walking distance to cafes, bakeries, and major attractions.",
                    PricePerNight = 220.00M,
                    ImageUrl = "/images/locations/paris.jpg",
                    IsPopular = false,
                    CreatedAt = DateTime.Now.AddMonths(-3)
                },
                new Location
                {
                    Name = "New York Loft",
                    Country = "USA",
                    Description = "Modern spacious loft in Manhattan with industrial design elements. Close to subway stations, museums, theaters and Central Park.",
                    PricePerNight = 300.00M,
                    ImageUrl = "/images/locations/newyork.jpg",
                    IsPopular = true,
                    CreatedAt = DateTime.Now.AddMonths(-2)
                }
            };

            context.Locations.AddRange(locations);
            context.SaveChanges();

            // Seed Reviews
            var reviews = new Review[]
            {
                new Review
                {
                    //LocationId, CustomerName, Email, Rating, Comment, CreatedAt, Location

                    //LocationId = locations[0].Id,
                    CustomerName = "Alice Johnson",
                    Email = "alice@gmail.com",
                    Rating = 5,
                    Comment = "Absolutely stunning views and a beautiful villa! The perfect romantic getaway.",
                    CreatedAt = DateTime.Now.AddDays(-10),
                    Location = locations[0],
                },
                new Review {
                    //LocationId = locations[1].Id,
                    CustomerName = "Bob Smith",
                    Email = "bobsmith@gmail.com",
                    Rating = 4,
                    Comment = "Great location and friendly staff. The beach was amazing!",
                    CreatedAt = DateTime.Now.AddDays(-5),
                    Location = locations[1],
                    },
                new Review {
                    //LocationId = locations[2].Id,
                    CustomerName = "Charlie Brown",
                    Email = "charliebrown@gmail.com",
                    Rating = 5,
                    Comment = "A unique experience! The onsen was a highlight of our stay.",
                    CreatedAt = DateTime.Now.AddDays(-3),
                    Location = locations[2],
                    },
                new Review {
                    //LocationId = locations[0].Id,
                    CustomerName = "Diana Prince",
                    Email = "dianaprince@gmail.com",
                    Rating = 3,
                    Comment = "The apartment was nice, but a bit noisy at night. Overall a good stay.",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Location = locations[0],
                    },
            };

            context.Reviews.AddRange(reviews);
            context.SaveChanges();

            // Seed Bookings
            var bookings = new Booking[]
            {
                new Booking
                {
                    //LocationId = locations[0].Id,
                    CustomerName = "Alice Johnson",
                    Email = "alice@gmail.com",
                    CheckInDate = DateTime.Now.AddDays(5),
                    CheckOutDate = DateTime.Now.AddDays(10),
                    NumberOfGuests = 2,
                    TotalPrice = 1250.00M,
                    Status = BookingStatus.Confirmed,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    Location = locations[0]
                },
                new Booking {
                   // LocationId = locations[1].Id,
                    CustomerName = "Bob Smith",
                    Email = "bobsmith@gmail.com",
                    CheckInDate = DateTime.Now.AddDays(2),
                    CheckOutDate = DateTime.Now.AddDays(7),
                    NumberOfGuests = 4,
                    TotalPrice = 900.00M,
                    Status = BookingStatus.Pending,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Location = locations[1]
                },
                new Booking {
                   // LocationId = locations[2].Id,
                    CustomerName = "Charlie Brown",
                    Email = "charliebrown@gmail.com",
                    CheckInDate = DateTime.Now.AddDays(1),
                    CheckOutDate = DateTime.Now.AddDays(6),
                    NumberOfGuests = 2,
                    TotalPrice = 1000.00M,
                    Status = BookingStatus.Cancelled,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Location = locations[2]
                    },
                new Booking {
                    //LocationId = locations[0].Id,
                    CustomerName = "Diana Prince",
                    Email = "dianaprince@gmail.com",
                    CheckInDate = DateTime.Now.AddDays(3),
                    CheckOutDate = DateTime.Now.AddDays(8),
                    NumberOfGuests = 3,
                    TotalPrice = 1100.00M,
                    Status = BookingStatus.Confirmed,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    Location = locations[0]
                    },
            };

            context.Bookings.AddRange(bookings);
            context.SaveChanges();

            // Seed Contacts
            var contacts = new Contact[]
            {
                new Contact
                {
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Subject = "Booking Inquiry",
                    Message = "I have some questions about the Santorini Villa availability in August.",
                    IsResolved = false,
                    CreatedAt = DateTime.Now.AddDays(-20),
                },
                new Contact
                {
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Subject = "Cancellation Policy",
                    Message = "Could you please provide more details about your cancellation policy?",
                    IsResolved = true,
                    CreatedAt = DateTime.Now.AddDays(-10),
                },
                new Contact
                {
                    Name = "Robert Brown",
                    Email = "robert.brown@example.com",
                    Subject = "Special Requirements",
                    Message = "We will be traveling with a baby and would like to know if you provide cribs at the Kyoto Traditional Inn.",
                    IsResolved = false,
                    CreatedAt = DateTime.Now.AddDays(-5),
                }
            };

            context.Contacts.AddRange(contacts);
            context.SaveChanges();
        }
    }
}