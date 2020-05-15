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
	}
}
