using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.IO;

namespace GeneralTests
{

  [Table(Name="Sales.CreditCard")]
  public class CreditCardInfo
  {
    [Column]
    public string CardType;

    [Column]
    public string CardNumber;

    [Column]
    public short ExpYear;
  }

  [TestCase("Database Raw Test")]
  public class LinqRaw
  {
    [TestCaseMethod("Retreive credit cards")]
    public void GetCards(IResultSink sink)
    {
      DataContext db = new DataContext("server=.;database=adventureworks2008;integrated security=true");
      Table<CreditCardInfo> cards = db.GetTable<CreditCardInfo>();

      var r =
        from c in cards
        where c.ExpYear > 2007 && c.ExpYear < 2010
        select c;

      
        foreach (var card in r.Take(10))
        {
          sink.Send(card.CardType + " " + card.CardNumber + " " + card.ExpYear.ToString() + "\n");
        }
      }
  }

	[TestCase("Database Linq tests")]
	public class LinqDatabase
	{
		//const string connectionString = "provider=sqloledb;database=adventureworks;trusted_connection=true";
		Adventure.AdventureWorksDataContext ctx = new GeneralTests.Adventure.AdventureWorksDataContext();

		public void ResetTest()
		{
			ctx = new GeneralTests.Adventure.AdventureWorksDataContext();
		}

		[TestCaseMethod("Connectivity test")]
		public void TestConnection(IResultSink res)
		{
			var cnt = from c in ctx.Contacts
								select c;

			res.Send("Connected, there are " + cnt.Count().ToString() + " contacts in database");
		}

		[TestCaseMethod("List individual customers")]
		public void TestListIndividualCustomers(IResultSink res)
		{
			using (StringWriter sw = new StringWriter())
			{
				var cust = from c in ctx.Customers
									 from i in ctx.Individuals
									 from co in ctx.Contacts
									 where c.CustomerType == 'I' && co.ContactID == i.ContactID && i.CustomerID == c.CustomerID
									 select co;
									 //select new
									 //{
									 //  co.FirstName,
									 //  co.LastName,
									 //  co.EmailAddress
									 //};
				ctx.Log = sw;
				foreach (var customer in cust.Take(10))
				{
					res.Send
						(
						customer.FirstName + " " + 
						customer.LastName + " " + 
						customer.EmailAddress + " "
						);
				}
				//res.Send(sw.ToString());
				//ctx.Log = null;
			}
		}
	}
}
