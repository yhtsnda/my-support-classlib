using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Bson.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Projects.Access.MongoAccess
{
    public class NewSerializer : IBsonSerializer
    {
        public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, 
            IBsonSerializationOptions options)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            BsonDocument document = new BsonDocument();
            document.Deserialize(bsonReader, typeof(BsonDocument), null);

            ConstructorInfo ctr = nominalType.GetConstructors()[0];
            List<object> objs = new List<object>();
            foreach (ParameterInfo param in ctr.GetParameters())
            {
                string name = param.Name == "Id" ? "_id" : param.Name;
                var v = document.GetValue(name);
                Type type = param.ParameterType;
                if (type == typeof(string))
                    objs.Add(v.AsString);
                else if (type == typeof(int))
                    objs.Add(v.AsInt32);
                else if (type == typeof(long))
                    objs.Add(v.AsInt64);
                else if (type == typeof(DateTime))
                    objs.Add(v.AsDateTime);
                else if (type == typeof(bool))
                    objs.Add(v.AsBoolean);
            }
            return ctr.Invoke(objs.ToArray());
        }

        public bool GetDocumentId(object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            throw new NotImplementedException();
        }

        public void SetDocumentId(object document, object id)
        {
            throw new NotImplementedException();
        }
    }
}
