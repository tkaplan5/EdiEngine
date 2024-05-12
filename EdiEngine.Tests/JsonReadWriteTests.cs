﻿using EdiEngine.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using EdiEngine.Standards.X12_004010.Maps;
using M_940 = EdiEngine.Standards.X12_004010.Maps.M_940;
using EdiEngine.Tests.Maps;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using System;

namespace EdiEngine.Tests
{
    [TestClass]
    public class JsonReadWriteTests
    {
        [TestMethod]
        public void JsonReadWrite_JsonSerializationTest()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.940.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                //Write Json using newtonsoft
                //check no exception
                JsonConvert.SerializeObject(b);
                JsonConvert.SerializeObject(b.Interchanges[0].Groups[0].Transactions[0]);

                //or use writer to write to string or stream
                JsonDataWriter w  = new JsonDataWriter();
                string str = w.WriteToString(b);
                Stream stream = w.WriteToStream(b);

                Assert.IsNotNull(str);

                Assert.IsNotNull(stream);
                Assert.AreEqual(0, stream.Position);
                Assert.IsTrue(stream.CanRead);

                Assert.AreEqual(str.Length, stream.Length);
            }
        }

        [TestMethod]
        public void JsonReadWrite_DeserializeJsonOK()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.940.OK.json");

            M_940 map = new M_940();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);
        }


        [TestMethod]
        public void JsonReadWrite_DeserializeJsonWithValidationErrors()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.940.ERR.json");

            M_940 map = new M_940();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(2, t.ValidationErrors.Count);
        }

        [TestMethod]
        public void JsonReadWrite_JsonSerializationHlLoopTest()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.856.Crossdock.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                JsonDataWriter jsonWriter = new JsonDataWriter();
                jsonWriter.WriteToString(b);
            }
        }

        [TestMethod]
        public void JsonReadWrite_DeserializeJsonHlLoopOk()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.856.Crossdock.OK.json");

            M_856 map = new M_856();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);

            //string edi = TestUtils.WriteEdiEnvelope(t, "SH");
        }

        [TestMethod]
        public void JsonReadWrite_SerializeComposite()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.850.Composite.SLN.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                JsonDataWriter jsonWriter = new JsonDataWriter();
                jsonWriter.WriteToString(b);
            }
        }

        [TestMethod]
        public void JsonReadWrite_Serialize_210()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.210.UPS.5010.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                JsonDataWriter jsonWriter = new JsonDataWriter();
                var result = jsonWriter.WriteToString(b);

                Console.WriteLine(result);
            }
        }



        [TestMethod]
        public void JsonReadWrite_DeserializeComposite()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.001.Fake.Composite.json");

            M_001 map = new M_001();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);

            var sln = (EdiSegment)t.Content.First();
            Assert.IsTrue(sln.Content[4] is EdiCompositeDataElement);
            Assert.AreEqual(6, ((EdiCompositeDataElement)sln.Content[4]).Content.Count);

        }
    }
}
