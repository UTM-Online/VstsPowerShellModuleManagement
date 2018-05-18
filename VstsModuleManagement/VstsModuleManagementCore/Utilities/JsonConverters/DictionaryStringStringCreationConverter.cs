namespace VstsModuleManagementCore.Utilities.JsonConverters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json.Converters;
    public class DictionaryStringStringCreationConverter<T> : CustomCreationConverter<IDictionary>
    {
        private IEqualityComparer<T> comparer;

        public DictionaryStringStringCreationConverter(IEqualityComparer<T> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public override bool CanConvert(Type objectType)
        {
            return HasCompatibleInterface(objectType) && HasCompatibleConstructor(objectType);
        }

        private static bool HasCompatibleInterface(Type objectType)
        {
            return objectType
                             .GetInterfaces()
                             .Where(i => HasGenericTypeDefinition(i, typeof(IDictionary<,>)))
                             .Any(i => typeof(T).IsAssignableFrom(i.GetGenericArguments().First()));
        }

        private static bool HasGenericTypeDefinition(Type objectType, Type typeDefinition)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeDefinition;
        }

        private static bool HasCompatibleConstructor(Type objectType)
        {
            return objectType.GetConstructor(new Type[] { typeof(IEqualityComparer<T>) }) != null;
        }

        public override IDictionary Create(Type objectType)
        {
            return Activator.CreateInstance(objectType, this.comparer) as IDictionary;
        }
    }
}