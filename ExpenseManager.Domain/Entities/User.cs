using ExpenseManager.Domain.Exceptions;
using ExpenseManager.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Domain.Entities
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime RegistrationDateTime { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public DateTime? LastUpdatedDate { get; private set; }
        public Guid SecurityStamp { get; private set; }
        public ICollection<Event> CreatedEvents { get; private set; }
        public ICollection<Event> UpdatedEvents { get; private set; }
        public ICollection<Expense> CreatedExpenses { get; private set; }
        public ICollection<Expense> PayededExpenses { get; private set; }
        public virtual ICollection<EventParticipant> EventParticipants { get; private set; }
        public virtual ICollection<ExpensePartTaker> ExpensePartTakerIds { get; private set; }

        private User(int id, string username, string passwordHash, DateTime registrationDateTime, Guid securityStamp, bool isActive)
        {
            if (id > 0)
                Id = id;

            Username = !string.IsNullOrWhiteSpace(username) ? username.Trim() :
              throw new ExpenseManagerDomainException("Username is not valid");

            PasswordHash = !string.IsNullOrWhiteSpace(passwordHash) ? passwordHash.Trim() :
             throw new ExpenseManagerDomainException("PasswordHash is not valid");

            SecurityStamp = !string.IsNullOrWhiteSpace(securityStamp.ToString()) ? securityStamp :
               throw new ExpenseManagerDomainException("SecurityStamp is not valid");

            RegistrationDateTime = registrationDateTime;
            IsActive = isActive;
        }

        public static User Create(string username, string passwordHash, bool isActive = true)
        {
            return Create(0, username, passwordHash, isActive);
        }

        //Need id parameter for seeding db
        public static User Create(int id, string username, string passwordHash, bool isActive = true)
        {
            Guid securityStamp = Guid.NewGuid();
            DateTime now = DateTime.Now;
            return new User(id, username, passwordHash, now, securityStamp, isActive);
        }

        public static User ChangePassword(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            user.SecurityStamp = Guid.NewGuid();
            user.LastUpdatedDate = DateTime.Now;
            return user;
        }

        public static User UpdateLastLoginDate(User user)
        {
            user.LastLoginDate = DateTime.Now;
            return user;
        }
    }
}
