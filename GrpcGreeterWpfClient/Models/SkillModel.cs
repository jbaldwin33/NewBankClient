using System;
using System.Collections.Generic;
using System.Text;
using static GrpcGreeterWpfClient.Models.Enums;

namespace GrpcGreeterWpfClient.Models
{
  public class SkillModel
  {
		public Guid ID { get; set; }

		public string Name { get; set; }

		public Proficiency SkillProficiency { get; set; }

    public static GrpcGreeter.Protos.Proficiency ConvertFromDbType(Proficiency proficiency)
    {
      return proficiency switch
      {
        Proficiency.Beginner => GrpcGreeter.Protos.Proficiency.Beginner,
        Proficiency.Adept =>    GrpcGreeter.Protos.Proficiency.Adept,
        Proficiency.Expert =>   GrpcGreeter.Protos.Proficiency.Expert,
        Proficiency.Master =>   GrpcGreeter.Protos.Proficiency.Master,
        _ => throw new NotSupportedException(),
      };
    }

    public static Proficiency ConvertFromProtoType(GrpcGreeter.Protos.Proficiency proficiency)
    {
      return proficiency switch
      {
        GrpcGreeter.Protos.Proficiency.Beginner => Proficiency.Beginner,
        GrpcGreeter.Protos.Proficiency.Adept => Proficiency.Adept,
        GrpcGreeter.Protos.Proficiency.Expert => Proficiency.Expert,
        GrpcGreeter.Protos.Proficiency.Master => Proficiency.Master,
        _ => throw new NotSupportedException(),
      };
    }
  }
}
