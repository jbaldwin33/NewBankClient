using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Services
{
  public class PersonCRUDService : PersonCRUD.PersonCRUDBase
  {
    private readonly AppDbContext db = null;
    public PersonCRUDService(AppDbContext db)
    {
      this.db = db;
    }

    public override Task<Person> GetByID(PersonFilter request, ServerCallContext context)
    {
      var data = db.Persons.FirstOrDefault(p => p.ID == Guid.Parse(request.Id));
      if (data == null)
        throw new ArgumentNullException();

      var person = new Person
      {
        FirstName = data.FirstName,
        LastName = data.LastName,
        Id = data.ID.ToString(),
        Age = data.Age,
        Skill = new Skill { Name = data.Skill.Name, Proficiency = SkillModel.ConvertFromDbType(data.Skill.Proficiency) }
      };
      return Task.FromResult(person);

    }

    public override Task<Persons> GetPersons(Empty request, ServerCallContext context)
    {
      var persons = new Persons();
      var query = from p in db.Persons
                  select new Person
                  {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Id = p.ID.ToString(),
                    Age = p.Age,
                    Skill = new Skill { Name = p.Skill.Name, Proficiency = SkillModel.ConvertFromDbType(p.Skill.Proficiency) }
                  };
      persons.Items.AddRange(query.ToArray());
      return Task.FromResult(persons);
    }

    public override Task<Empty> Insert(Person request, ServerCallContext context)
    {
      db.Persons.Add(new PersonModel
      {
        FirstName = request.FirstName,
        LastName = request.LastName,
        ID = Guid.Parse(request.Id),
        Age = request.Age,
        Skill = new SkillModel { ID = Guid.NewGuid(), Name = request.Skill.Name, Proficiency = SkillModel.ConvertFromProtoType(request.Skill.Proficiency) }
      });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Update(Person request, ServerCallContext context)
    {
      db.Persons.Update(new PersonModel
      {
        FirstName = request.FirstName,
        LastName = request.LastName,
        ID = Guid.Parse(request.Id),
        Age = request.Age,
        Skill = new SkillModel { ID = Guid.NewGuid(), Name = request.Skill.Name, Proficiency = SkillModel.ConvertFromProtoType(request.Skill.Proficiency) }
      });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Delete(PersonFilter request, ServerCallContext context)
    {
      var person = db.Persons.FirstOrDefault(p => p.ID == Guid.Parse(request.Id));
      if (person == null)
        throw new ArgumentNullException();

      db.Persons.Remove(person);
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }
  }
}
