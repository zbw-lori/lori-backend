﻿using lori.backend.Core.ProjectAggregate;
using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace lori.backend.Web;

public static class SeedData
{
  public static readonly Project TestProject1 = new Project("Test Project", PriorityStatus.Backlog);
  public static readonly ToDoItem ToDoItem1 = new ToDoItem
  {
    Title = "Get Sample Working",
    Description = "Try to get the sample to build."
  };
  public static readonly ToDoItem ToDoItem2 = new ToDoItem
  {
    Title = "Review Solution",
    Description = "Review the different projects in the solution and how they relate to one another."
  };
  public static readonly ToDoItem ToDoItem3 = new ToDoItem
  {
    Title = "Run and Review Tests",
    Description = "Make sure all the tests run and review what they are doing."
  };

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using (var dbContext = new LoriDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<LoriDbContext>>(), null))
    {
      // Look for any TODO items.
      if (dbContext.Robots.Any())
      {
        return;   // DB has been seeded
      }

      PopulateTestData(dbContext);


    }
  }
  public static void PopulateTestData(LoriDbContext dbContext)
  {
    RemoveExistingAddresses(dbContext);
    AddAddresses(dbContext);

    RemoveExistingCustomers(dbContext);
    AddCustomers(dbContext);
  }

  private static void RemoveExistingAddresses(LoriDbContext dbContext)
  {
    foreach (var address in dbContext.Addresses)
    {
      dbContext.Remove(address);
    }
    dbContext.SaveChanges();
  }

  private static void AddAddresses(LoriDbContext dbContext)
  {
    dbContext.Add(new Address
    {
      Street = "Musterstrasse",
      StreetNumber = 1,
      City = "St.Gallen",
      CityCode = 9000
    });
    dbContext.Add(new Address
    {
      Street = "9th Street",
      StreetNumber = 93,
      City = "Brooklyn",
      CityCode = 11211
    });
    dbContext.SaveChanges();
  }

  private static void RemoveExistingCustomers(LoriDbContext dbContext)
  {
    foreach (var customer in dbContext.Customers)
    {
      dbContext.Remove(customer);
    }
    dbContext.SaveChanges();
  }

  private static void AddCustomers(LoriDbContext dbContext)
  {
    dbContext.Add(new Customer
    {
      Forename = "Max",
      Surename = "Muster",
      Email = "max.muster@test.ch",
      Phone = "202-555-0102",
      AddressId = 1,
    });
    dbContext.Add(new Customer
    {
      Forename = "Peter",
      Surename = "Parker",
      Email = "peter.parker@spider.com",
      Phone = "478-488-8628",
      AddressId = 2,
    });
    dbContext.SaveChanges();
  }
}
