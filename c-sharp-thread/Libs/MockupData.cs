using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_thread
{
  
    class MockupData
    {
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public Guid CartId { get; set; }

        public User(int userIds, string mark)
        {

        }
    }

    public class FakeData
    {
        public List<User> Data(int nb)
        {
            var rnd = new Random(8675309);
            var userIds = rnd.Next(1, 100);

            var testUsers = new Faker<User>()
                .CustomInstantiator(f => {
                    return new User(userIds++, f.Random.Replace("###-##-####"));
                })
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.CartId, f => Guid.NewGuid())

                .FinishWith((f, u) =>
                {
                    // Console.WriteLine("User created");
                    // Console.WriteLine("User Created! Id={0}", u.Id);
                });
            return testUsers.Generate(nb);
        }

    }
}
