#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Common
{
    public interface IValidationConstraint<T>
    {
        Func<T, bool> Validate { get; }
        string Message { get; }
    }

    public class ValidationConstraint<T> : IValidationConstraint<T>
    {
        public ValidationConstraint(string message, Func<T, bool> validate)
        {
            Message = message;
            Validate = validate;
        }

        public Func<T, bool> Validate { get; set; }
        public string Message { get; set; }
    }

    public class RequiredConstraint<T> : IValidationConstraint<T>
    {
        public RequiredConstraint(string propertyName, Func<T, object> propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (value is Guid guid)
                {
                    return guid != Guid.Empty;
                }
                return !string.IsNullOrEmpty(value.ToString());
            }
            return false;
        }

        string IValidationConstraint<T>.Message => $"Поле '{PropertyName}' не может быть пустым.";
    }

    public class RequiredGreaterThanZeroConstraint<T> : IValidationConstraint<T>
    {
        public RequiredGreaterThanZeroConstraint(string propertyName, Func<T, object> propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d > 0;
                }
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Поле '{PropertyName}' не может быть пустым.";
    }

    public class PositiveConstraint<T> : IValidationConstraint<T>
    {
        public PositiveConstraint(string propertyName, Func<T, object> propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d >= 0;
                }
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Поле '{PropertyName}' должно быть больше нуля.";
    }

    public class NonZeroConstraint<T> : IValidationConstraint<T>
    {
        public NonZeroConstraint(string propertyName, Func<T, object> propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d != 0;
                }
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Поле '{PropertyName}' не должно быть нулем.";
    }

    public class GreaterThanConstraint<T> : IValidationConstraint<T>
    {
        public GreaterThanConstraint(string propertyName, Func<T, object> propertyValue, double value)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            Value = value;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }
        public double Value { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d > Value;
                }
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Значение поля '{PropertyName}' должно быть больше чем {Value}.";
    }

    public class NonGreaterThanConstraint<T> : IValidationConstraint<T>
    {
        public NonGreaterThanConstraint(string propertyName, Func<T, object> propertyValue, double value, string valueDesc = null)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            Value = value;
            ValueDesc = valueDesc ?? Value.ToString();
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }
        public double Value { get; set; }
        public string ValueDesc { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d <= Value;
                }
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Значение поля '{PropertyName}' не может быть больше {ValueDesc}.";
    }

    public class LessThanConstraint<T> : IValidationConstraint<T>
    {
        public LessThanConstraint(string propertyName, Func<T, object> propertyValue, double value)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            Value = value;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }
        public double Value { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d < Value;
                }
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Значение '{PropertyName}' должно быть меньше {Value}.";
    }

    /// <summary>
    /// Проверяет значение поля со значениями в списке, больше чем 0
    /// </summary>
    public class InListGreaterThanZeroConstraint<T> : IValidationConstraint<T>
    {
        public InListGreaterThanZeroConstraint(string propertyName, Func<T, object> propertyValue, IList<int> listIn)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            ListIn = listIn;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }
        public IList<int> ListIn { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (int.TryParse(value.ToString(), out int lo))
            {
                return ListIn.First(x => x == lo) > 0;
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Значение поля '{PropertyName}' не является допустимым.";
    }

}
