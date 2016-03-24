using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace UploadingFiles.Web.ValueProviders
{
    public class JsonNetValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            // first make sure we have a valid context
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            //now make sure we are dealing with a json request
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                return null;

            // get a generic stream reader (get reader for the http stream)
            var streamReader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            // convert stream reader to a JSON Text Reader
            var JsonReader = new JsonTextReader(streamReader);
            // tell JSON to read
            if (!JsonReader.Read())
                return null;

            // make a new Json serializer
            var JsonSerializer = new JsonSerializer();
            // add the dyamic object converter to our serializer
            JsonSerializer.Converters.Add(new ExpandoObjectConverter());

            // use JSON.NET to deserialize object to a dynamic (expando) object
            Object JsonObject;
            // if we start with a "[", treat this as an array
            if (JsonReader.TokenType == JsonToken.StartArray)
                JsonObject = JsonSerializer.Deserialize<List<ExpandoObject>>(JsonReader);
            else
                JsonObject = JsonSerializer.Deserialize<ExpandoObject>(JsonReader);

            // create a backing store to hold all properties for this deserialization
            var backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            // add all properties to this backing store
            AddToBackingStore(backingStore, String.Empty, JsonObject);
            // return the object in a dictionary value provider so the MVC understands it
            return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
        }

        private static void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
        {
            var dictionary = value as IDictionary<string, object>;
            if (dictionary != null)
            {
                foreach (var entry in dictionary)
                {
                    AddToBackingStore(backingStore, MakePropertyKey(prefix, entry.Key), entry.Value);
                }
                return;
            }

            var list = value as IList;
            if (list != null)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    AddToBackingStore(backingStore, MakeArrayKey(prefix, i), list[i]);
                }
                return;
            }

            // primitive
            backingStore[prefix] = value;
        }

        private static string MakeArrayKey(string prefix, int index)
        {
            return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
        }

        private static string MakePropertyKey(string prefix, string propertyName)
        {
            return (String.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
        }
    }
}