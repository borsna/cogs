﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cogs.Publishers
{
    public class JsonSimpleConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is string schema)
            {
                var GMD = new JArray() { "month", "day" };
                var GYM = new JArray() { "year", "month" };
                var obj = new JObject();
                obj.Add(new JProperty("duration", 
                    new JObject(
                        new JProperty("type", "number"), 
                        new JProperty("format", "utc-millisec"))));

                obj.Add(new JProperty("dateTime", 
                    new JObject(
                        new JProperty("type", "string"), 
                        new JProperty("format", "date-time"))));

                obj.Add(new JProperty("time", 
                    new JObject(
                        new JProperty("type", "string"), 
                    new JProperty("format", "time"))));

                obj.Add(new JProperty("date", 
                    new JObject(
                        new JProperty("type", "string"), 
                        new JProperty("format", "date"))));

                obj.Add(new JProperty("gYearMonth", 
                    new JObject(
                        new JProperty("type", "object"), 
                        new JProperty("properties", 
                        new JObject(
                            new JProperty("year", 
                            new JObject(
                                new JProperty("type", "integer"))), 
                            new JProperty("month", 
                            new JObject(
                                new JProperty("type", "integer"))), 
                            new JProperty("timezone", 
                            new JObject(
                                new JProperty("type", "string"))))), 
                        new JProperty("required", GYM))));

                obj.Add(new JProperty("gYear", 
                    new JObject(
                        new JProperty("type", "object"), 
                        new JProperty("properties", 
                        new JObject(
                            new JProperty("year", 
                            new JObject(
                                new JProperty("type", "integer"))), 
                            new JProperty("timezone", 
                            new JObject(
                                new JProperty("type", "string"))))))));

                obj.Add(new JProperty("gMonthDay", 
                    new JObject(
                        new JProperty("type", "object"), 
                        new JProperty("properties", 
                            new JObject(
                                new JProperty("month",
                                    new JObject(
                                        new JProperty("type", "integer"))), 
                                new JProperty("day", 
                                    new JObject(
                                        new JProperty("type", "integer"))), 
                                new JProperty("timezone", 
                                    new JObject(
                                        new JProperty("type", "string"))))), 
                                new JProperty("required", GMD))));

                obj.Add(new JProperty("gDay", 
                    new JObject(
                        new JProperty("type", "object"),
                    new JProperty("properties", 
                    new JObject(
                        new JProperty("day", 
                        new JObject(
                            new JProperty("type", "integer"))), 
                        new JProperty("timezone", 
                        new JObject(
                            new JProperty("type", "string"))))))));

                obj.Add(new JProperty("gMonth", 
                    new JObject(
                        new JProperty("type", "object"), 
                    new JProperty("properties", 
                    new JObject(
                        new JProperty("month", 
                        new JObject(
                            new JProperty("type", "integer"))), 
                        new JProperty("timezone", 
                        new JObject(
                            new JProperty("type", "string"))))))));

                obj.Add(new JProperty("anyURI", 
                    new JObject(
                        new JProperty("type", "string"))));

                obj.Add(new JProperty("cogsDate",
                    new JObject(
                        new JProperty("type", "object"),
                        new JProperty("properties", 
                        new JObject(
                            new JProperty("dateTime", 
                            new JObject(
                                new JProperty("$ref", "#/simpleType/dateTime"))),
                            new JProperty("date", 
                            new JObject(
                                new JProperty("$ref", "#/simpleType/date"))),
                            new JProperty("gYearMonth", 
                            new JObject(
                                new JProperty("$ref", "#/simpleType/gYearMonth"))),
                            new JProperty("gYear", 
                            new JObject(
                                new JProperty("$ref", "#/simpleType/gYear"))),
                            new JProperty("duration", 
                            new JObject(
                                new JProperty("$ref", "#/simpleType/duration"))))))));

                obj.Add(new JProperty("language", 
                    new JObject(
                        new JProperty("type","string"))));
                obj.WriteTo(writer);
            }
        }
    }
}