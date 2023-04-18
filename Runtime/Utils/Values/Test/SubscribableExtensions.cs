using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

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

        public static Action Subscribe<T, TValue>(this T obj, Expression<Func<T, TValue>> expression, Action<T> callback)
        {
            var field = GetFieldInfo(expression);

            if (field == null)
            {
                throw new ArgumentException($"Field not found in type '{obj.GetType().Name}'");
            }
            
            var propertyName = field.Name;
            var attribute = field.GetCustomAttribute<SubscribableAttribute>();
            if (attribute == null)
            {
                throw new ArgumentException($"Field '{propertyName}' does not have the {typeof(SubscribableAttribute)} attribute", nameof(propertyName));
            }

            var metadata = (FieldMetadata<T>)fieldsData.GetOrAdd(field, _ => new FieldMetadata<T>());

            metadata.Notifier.Subscribe(callback);

            var subscriptionGUID = Guid.NewGuid().ToString();
            
            Action unsubscribeToken = () =>
            {
                metadata.Notifier.Unsubscribe(callback);
                metadata.UnsubscribeTokens.Remove(subscriptionGUID);
            };
            
            metadata.UnsubscribeTokens.Add(subscriptionGUID, unsubscribeToken);
            
            return unsubscribeToken;
        }
        
        private static FieldInfo GetFieldInfo<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                return null;
            }

            if (!(memberExpression.Member is FieldInfo fieldInfo))
            {
                return null;
            }

            if (!(fieldInfo.IsPublic || fieldInfo.IsFamily || fieldInfo.IsAssembly))
            {
                return null;
            }

            if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
            {
                return null;
            }

            return fieldInfo;
        }

        public static void Notify<T, TValue>(this T obj, Expression<Func<T, TValue>> expression)
        {
            var field = GetFieldInfo(expression);
            var metadata = GetMetadata<T>(field);

            metadata?.Notifier.Notify(obj);
        }

        private static FieldMetadata<T> GetMetadata<T>(FieldInfo field)
        {
            if (fieldsData.TryGetValue(field, out var metadata))
            {
                return (FieldMetadata<T>)metadata;
            }

            Debug.LogError($"Failed to retrieve metadata for field {field.Name}");
            return null;
        }
    }
}