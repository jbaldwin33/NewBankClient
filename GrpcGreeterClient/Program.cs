using Grpc.Net.Client;
using GrpcGreeter;
using GrpcGreeter.Protos;
using System;
using System.Threading.Tasks;

namespace GrpcGreeterClient
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var personGuid = Guid.NewGuid();

      using var channel = GrpcChannel.ForAddress("https://localhost:5001");
      var client = new PersonCRUD.PersonCRUDClient(channel);
      var reply = await client.InsertAsync(new Person
      {
        FirstName = "bob",
        LastName = "smith",
        Id = personGuid.ToString(),
        Age = 20,
        Skill = new Skill { Name = "kendama", Proficiency = Proficiency.Adept }
      });
      Console.WriteLine("Person created successfully.");

      var otherReply = await client.GetByIDAsync(new PersonFilter { PersonID = personGuid.ToString() });
      Console.WriteLine("Person retrieved.");
      Console.WriteLine($"First name: {otherReply.FirstName}\nLast name: {otherReply.LastName}");

      Console.WriteLine("Press any key to exit...");
      Console.ReadKey();
    }
  }
}
