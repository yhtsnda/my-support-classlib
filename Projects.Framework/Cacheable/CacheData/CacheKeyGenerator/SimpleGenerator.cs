using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace Projects.Framework
{
    internal class SimpleGenerator
    {
        public static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        internal static readonly long DatetimeMinTimeTicks;
        internal static readonly int RecursionLimit = 100;


        static SimpleGenerator()
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DatetimeMinTimeTicks = time.Ticks;
        }

        private class ReferenceComparer : IEqualityComparer
        {
            // Methods
            bool IEqualityComparer.Equals(object x, object y)
            {
                return (x == y);
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }

        public static string Serialize(object obj)
        {
            StringBuilder output = new StringBuilder();
            Serialize(obj, output);
            return output.ToString();
        }

        public static void Serialize(object obj, StringBuilder output)
        {
            sw.Start();
            SerializeValue(obj, output, 0, null);
            sw.Stop();
        }

        static void SerializeBoolean(bool o, StringBuilder sb)
        {
            if (o)
                sb.Append("true");
            else
                sb.Append("false");
        }

        static void SerializeCustomObject(object o, StringBuilder sb, int depth, Hashtable objectsInUse)
        {
            Type type = o.GetType();
            var pa = PropertyAccessorFactory.GetPropertyAccess(type);
            SerializeDictionary(pa.ToDictionary(o), sb, depth, objectsInUse);
        }

        static void SerializeDateTime(DateTime datetime, StringBuilder sb)
        {
            sb.Append("\"\\/Date(");
            sb.Append((long)((datetime.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 0x2710));
            sb.Append(")\\/\"");
        }

        static void SerializeDictionary(IDictionary<string, object> o, StringBuilder sb, int depth, Hashtable objectsInUse)
        {
            sb.Append('{');
            bool flag = true;
            foreach (var entry in o)
            {
                string key = entry.Key as string;
                if (key == null)
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' is not supported for serialization/deserialization of a dictionary, keys must be strings or objects.", new object[] { o.GetType().FullName }));

                if (!flag)
                    sb.Append(',');

                SerializeDictionaryKeyValue(key, entry.Value, sb, depth, objectsInUse);
                flag = false;
            }
            sb.Append('}');
        }

        static void SerializeDictionary(IDictionary o, StringBuilder sb, int depth, Hashtable objectsInUse)
        {
            sb.Append('{');
            bool flag = true;
            foreach (DictionaryEntry entry in o)
            {
                string key = entry.Key as string;
                if (key == null)
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' is not supported for serialization/deserialization of a dictionary, keys must be strings or objects.", new object[] { o.GetType().FullName }));

                if (!flag)
                    sb.Append(',');

                SerializeDictionaryKeyValue(key, entry.Value, sb, depth, objectsInUse);
                flag = false;
            }
            sb.Append('}');
        }

        static void SerializeDictionaryKeyValue(string key, object value, StringBuilder sb, int depth, Hashtable objectsInUse)
        {
            SerializeString(key, sb);
            sb.Append(':');
            SerializeValue(value, sb, depth, objectsInUse);
        }

        static void SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, int depth, Hashtable objectsInUse)
        {
            sb.Append('[');
            bool flag = true;
            foreach (object obj2 in enumerable)
            {
                if (!flag)
                {
                    sb.Append(',');
                }
                SerializeValue(obj2, sb, depth, objectsInUse);
                flag = false;
            }
            sb.Append(']');
        }

        static void SerializeGuid(Guid guid, StringBuilder sb)
        {
            sb.Append("\"").Append(guid.ToString()).Append("\"");
        }

        static void SerializeString(string input, StringBuilder sb)
        {
            sb.Append('"');
            sb.Append(input);
            sb.Append('"');
        }
        static void SerializeUri(Uri uri, StringBuilder sb)
        {
            sb.Append("\"").Append(uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped)).Append("\"");
        }

        static void SerializeValue(object o, StringBuilder sb, int depth, Hashtable objectsInUse)
        {
            if (++depth > RecursionLimit)
                throw new ArgumentException("RecursionLimit exceeded.");

            SerializeValueInternal(o, sb, depth, objectsInUse);
        }

        static void SerializeExpression(Expression expr, StringBuilder sb, int depth, Hashtable objectsInUse)
        {
            var e = Evaluator.PartialEval(expr, CanBeEvaluatedLocally);
            SerializeString(e.ToString(), sb);

            //IDictionary<string, object> result = new Dictionary<string, object>();
            //result.Add("Expression", expr.ToString());
            //ConstantExpressionVisitor vistor   = new ConstantExpressionVisitor(expr);
            //int num                            = 1;
            //foreach (var cs in vistor.Constants)
            //{
            //    result.Add("Ref" + num.ToString(), cs);
            //    num++;
            //}
            //SerializeDictionary(result, sb, depth, objectsInUse);
        }

        static Func<Expression, bool> CanBeEvaluatedLocally
        {
            get
            {
                return expression =>
                {
                    // don't evaluate parameters
                    if (expression.NodeType == ExpressionType.Parameter)
                        return false;

                    // can't evaluate queries
                    if (typeof(IQueryable).IsAssignableFrom(expression.Type))
                        return false;

                    return true;
                };
            }
        }

        static void SerializeValueInternal(object o, StringBuilder sb, int depth, Hashtable objectsInUse)
        {
            if ((o == null) || DBNull.Value.Equals(o))
            {
                sb.Append("null");
            }
            else
            {
                string input = o as string;
                if (input != null)
                {
                    SerializeString(input, sb);
                }
                else if (o is char)
                {
                    if (((char)o) == '\0')
                    {
                        sb.Append("null");
                    }
                    else
                    {
                        SerializeString(o.ToString(), sb);
                    }
                }
                else if (o is bool)
                {
                    SerializeBoolean((bool)o, sb);
                }
                else if (o is DateTime)
                {
                    SerializeDateTime((DateTime)o, sb);
                }
                else if (o is Guid)
                {
                    SerializeGuid((Guid)o, sb);
                }
                else if (o is Expression)
                {
                    SerializeExpression((Expression)o, sb, depth, objectsInUse);
                }
                else
                {
                    Uri uri = o as Uri;
                    if (uri != null)
                    {
                        SerializeUri(uri, sb);
                    }
                    else if (o is double)
                    {
                        sb.Append(((double)o).ToString("r", CultureInfo.InvariantCulture));
                    }
                    else if (o is float)
                    {
                        sb.Append(((float)o).ToString("r", CultureInfo.InvariantCulture));
                    }
                    else if (o.GetType().IsPrimitive || (o is decimal))
                    {
                        IConvertible convertible = o as IConvertible;
                        if (convertible != null)
                        {
                            sb.Append(convertible.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            sb.Append(o.ToString());
                        }
                    }
                    else
                    {
                        Type enumType = o.GetType();
                        if (enumType.IsEnum)
                        {
                            sb.Append(((Enum)o).ToString("D"));
                        }
                        else
                        {
                            try
                            {
                                if (objectsInUse == null)
                                    objectsInUse = new Hashtable(new ReferenceComparer());

                                if (!objectsInUse.ContainsKey(o))
                                {
                                    objectsInUse.Add(o, null);
                                    IDictionary dictionary = o as IDictionary;
                                    if (dictionary != null)
                                    {
                                        SerializeDictionary(dictionary, sb, depth, objectsInUse);
                                        return;
                                    }

                                    IEnumerable enumerable = o as IEnumerable;
                                    if (enumerable != null)
                                    {
                                        SerializeEnumerable(enumerable, sb, depth, objectsInUse);
                                        return;
                                    }

                                    ICacheKeySupport support = o as ICacheKeySupport;
                                    if (support != null)
                                    {
                                        SerializeDictionary(support.Serialize(), sb, depth, objectsInUse);
                                        return;
                                    }

                                    SerializeCustomObject(o, sb, depth, objectsInUse);
                                }
                            }
                            finally
                            {
                                if (objectsInUse != null)
                                {
                                    objectsInUse.Remove(o);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
