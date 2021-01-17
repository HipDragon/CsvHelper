﻿// Copyright 2009-2021 Josh Close
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// https://github.com/JoshClose/CsvHelper
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvHelper.Tests
{
	[TestClass]
	public class ClearRecordsCacheTests
	{
		[TestMethod]
		public void ClearReaderTest()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				writer.WriteLine("Id,Name");
				writer.WriteLine("1,one");
				writer.WriteLine("2,two");
				writer.Flush();
				stream.Position = 0;

				csv.Context.RegisterClassMap<TestMap1>();
				csv.Read();
				var record = csv.GetRecord<Test>();

				Assert.IsNotNull(record);
				Assert.AreEqual(1, record.Id);
				Assert.AreEqual(null, record.Name);

				stream.Position = 0;

				csv.Context.RegisterClassMap<TestMap2>();
				csv.Read();
				record = csv.GetRecord<Test>();

				Assert.IsNotNull(record);
				Assert.AreEqual(0, record.Id);
				Assert.AreEqual("two", record.Name);
			}
		}

		private class Test
		{
			public int Id { get; set; }

			public string Name { get; set; }
		}

		private sealed class TestMap1 : ClassMap<Test>
		{
			public TestMap1()
			{
				Map(m => m.Id);
			}
		}

		private sealed class TestMap2 : ClassMap<Test>
		{
			public TestMap2()
			{
				Map(m => m.Name);
			}
		}
	}
}
