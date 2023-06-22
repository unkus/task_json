using System.Text.Json.Serialization;
using Converters;

namespace Models;

class Person
{
	public Int32 Id { get; set; }
	public Guid TransportId { get; set; }
	public String FirstName { get; set; }
	public String LastName { get; set; }
	public Int32 SequenceId { get; set; }
	public String[] CreditCardNumbers { get; set; }
	public Int32 Age { get; set; }
	public String[] Phones { get; set; }
	[JsonConverter(typeof(BirthDateJsonConverter))]
	public Int64 BirthDate { get; set; }
	public Double Salary { get; set; }
	public Boolean IsMarred { get; set; }
	[JsonConverter(typeof(GenderJsonConverter))]
	public Gender Gender { get; set; }
	public Child[] Children { get; set; }	
}