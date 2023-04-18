using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Padoru.Core
{
    public static class SubscribableExtensions
    {
        private class FieldMetadata<T>
        {
            public Notifier<T> Notifier = new();
            public Dictionary<string, Action> UnsubscribeTokens = new();
        }
        
        private static readonly ConcurrentDictionary<FieldInfo, object> fieldsData = new();

        /// <summary>
        /// Returns an unsubscribe token
        /// </summary>
        public static Action Subscribe<T, TValue>(this T _, Expression<Func<T, TValue>> expression, Action<T> callback)
        {
            var metadata = GetMetadata(expression);

            metadata.Notifier.Subscribe(callback);

            var subscriptionGuid = Guid.NewGuid().ToString();
            
            void UnsubscribeToken()
            {
                metadata.Notifier.Unsubscribe(callback);
                metadata.UnsubscribeTokens.Remove(subscriptionGuid);
            }

            metadata.UnsubscribeTokens.Add(subscriptionGuid, UnsubscribeToken);
            
            return UnsubscribeToken;
        }

        public static void Notify<T, TValue>(this T obj, Expression<Func<T, TValue>> expression)
        {
            var metadata = GetMetadata(expression);

            metadata.Notifier.Notify(obj);
        }

        private static FieldMetadata<T> GetMetadata<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var field = GetFieldInfo(expression);
            
            var metadata = (FieldMetadata<T>)fieldsData.GetOrAdd(field, _ => new FieldMetadata<T>());

            return metadata;
        }
        
        private static FieldInfo GetFieldInfo<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException($"Expression body is not of type '{typeof(MemberExpression)}'");
            }

            if (!(memberExpression.Member is FieldInfo fieldInfo))
            {
                throw new ArgumentException($"MemberExpression member is not of type '{typeof(FieldInfo)}'");
            }

            if (!fieldInfo.IsDefined(typeof(SubscribableAttribute), false))
            {
                throw new ArgumentException($"'{typeof(SubscribableAttribute)}' is not defined", fieldInfo.Name);
            }
            
            if (!fieldInfo.IsPublic)
            {
                throw new ArgumentException($"Field is not public", fieldInfo.Name);
            }

            if (fieldInfo.IsFamily || fieldInfo.IsAssembly)
            {
                throw new ArgumentException($"Field is Family or Assembly", fieldInfo.Name);
            }

            if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
            {
                throw new ArgumentException($"Field is InitOnly or Literal", fieldInfo.Name);
            }

            return fieldInfo;
        }
    }
}