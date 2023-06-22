using System.Text.Json.Serialization;
using Converters;

namespace Models;

class Child
{
	public Int32 Id { get; set; }
	public String FirstName { get; set; }
	public String LastName { get; set; }
	[JsonConverter(typeof(BirthDateJsonConverter))]
	public Int64 BirthDate { get; set; }
	[JsonConverter(typeof(GenderJsonConverter))]
	public Gender Gender { get; set; }
}