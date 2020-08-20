using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shared.Utils.Lib.Entities.Json
{
    public class JsonFieldSerializationPredicate : DefaultContractResolver
    {
        private readonly Func<JsonProperty, bool> _propertyPredicate;
        private readonly Func<object, bool> _objectPredicate;

        public JsonFieldSerializationPredicate(
            Func<JsonProperty, bool> propertyPredicate,
            Func<object, bool> objectPredicate)
        {
            _propertyPredicate = propertyPredicate;
            _objectPredicate = objectPredicate;
        }

        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (_propertyPredicate(property))
            {
                property.ShouldSerialize = instance => _objectPredicate(instance);
            }
            return property;
        }
    }
}