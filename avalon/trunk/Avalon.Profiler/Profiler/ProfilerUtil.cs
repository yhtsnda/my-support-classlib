using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using System.Security;

namespace Avalon.Profiler
{
    public static class ProfilerUtil
    {
        static string[] filters;
        static string Indent = "    ";

        static ProfilerUtil()
        {
            string filter = ConfigurationManager.AppSettings["avalon.profiler.filters"];
            filters = filter != null ? filter.Split(',') : new string[] { "System", "BLToolkit", "Avalon", "PostSharp" };
        }

        public static string FormatStackTrace(StackTrace trace)
        {
            bool displayFilenames = true;   // we'll try, but demand may fail
            String word_At = "at";
            String inFileLineNum = "in {0}:line {1}";

            bool firstFrame = true;
            bool isInner = false;
            StringBuilder sb = new StringBuilder(255);

            StackFrame[] frames = trace.GetFrames();
            for (int index = 2; index < frames.Length; index++)
            {
                StackFrame frame = frames[index];
                MethodBase method = frame.GetMethod();
                if (method != null)
                {
                    // We want a newline at the end of every line except for the last
                    if (firstFrame)
                        firstFrame = false;

                    Type t = method.DeclaringType;

                    if (t != null && (filters.Any(o => t.FullName.StartsWith(o.Trim())) || t.FullName.Contains("__")))
                    {
                        if (!isInner)
                        {
                            sb.Append(Environment.NewLine + "   ...");
                            isInner = true;
                        }
                        continue;
                    }
                    isInner = false;
                    sb.Append(Environment.NewLine);

                    sb.AppendFormat(CultureInfo.InvariantCulture, "   {0} ", word_At);

                    // if there is a type (non global method) print it
                    if (t != null)
                    {
                        sb.Append(t.FullName.Replace('+', '.'));
                        sb.Append(".");
                    }
                    sb.Append(method.Name);

                    // deal with the generic portion of the method 
                    if (method is MethodInfo && ((MethodInfo)method).IsGenericMethod)
                    {
                        Type[] typars = ((MethodInfo)method).GetGenericArguments();
                        sb.Append("[");
                        int k = 0;
                        bool fFirstTyParam = true;
                        while (k < typars.Length)
                        {
                            if (fFirstTyParam == false)
                                sb.Append(",");
                            else
                                fFirstTyParam = false;

                            sb.Append(typars[k].Name);
                            k++;
                        }
                        sb.Append("]");
                    }

                    // arguments printing
                    sb.Append("(");
                    ParameterInfo[] pi = method.GetParameters();
                    bool fFirstParam = true;
                    for (int j = 0; j < pi.Length; j++)
                    {
                        if (fFirstParam == false)
                            sb.Append(", ");
                        else
                            fFirstParam = false;

                        String typeName = "<UnknownType>";
                        if (pi[j].ParameterType != null)
                            typeName = pi[j].ParameterType.Name;
                        sb.Append(typeName + " " + pi[j].Name);
                    }
                    sb.Append(")");

                    // source location printing
                    if (displayFilenames && (frame.GetILOffset() != -1))
                    {
                        // If we don't have a PDB or PDB-reading is disabled for the module,
                        // then the file name will be null. 
                        String fileName = null;

                        // Getting the filename from a StackFrame is a privileged operation - we won't want 
                        // to disclose full path names to arbitrarily untrusted code.  Rather than just omit
                        // this we could probably trim to just the filename so it's still mostly usefull.
                        try
                        {
                            fileName = frame.GetFileName();
                        }

                        catch (SecurityException)
                        {
                            // If the demand for displaying filenames fails, then it won't 
                            // succeed later in the loop.  Avoid repeated exceptions by not trying again.
                            displayFilenames = false;
                        }

                        if (fileName != null)
                        {
                            // tack on " in c:\tmp\MyFile.cs:line 5" 
                            sb.Append(' ');
                            sb.AppendFormat(CultureInfo.InvariantCulture, inFileLineNum, fileName, frame.GetFileLineNumber());
                        }
                    }

                }
            }
            return sb.ToString();
        }

        public static string JsonFormat(string input)
        {
            var output = new StringBuilder(input.Length * 2);
            char? quote = null;
            int depth = 0;

            for (int i = 0; i < input.Length; ++i)
            {
                char ch = input[i];

                switch (ch)
                {
                    case '{':
                    case '[':
                        output.Append(ch);
                        if (!quote.HasValue)
                        {
                            output.AppendLine();
                            output.Append(Repeat(Indent, ++depth));
                        }
                        break;
                    case '}':
                    case ']':
                        if (quote.HasValue)
                            output.Append(ch);
                        else
                        {
                            output.AppendLine();
                            output.Append(Repeat(Indent, --depth));
                            output.Append(ch);
                        }
                        break;
                    case '"':
                    case '\'':
                        output.Append(ch);
                        if (quote.HasValue)
                        {
                            if (!IsEscaped(output, i))
                                quote = null;
                        }
                        else quote = ch;
                        break;
                    case ',':
                        output.Append(ch);
                        if (!quote.HasValue)
                        {
                            output.AppendLine();
                            output.Append(Repeat(Indent, depth));
                        }
                        break;
                    case ':':
                        if (quote.HasValue) output.Append(ch);
                        else output.Append(" : ");
                        break;
                    default:
                        if (quote.HasValue || !char.IsWhiteSpace(ch))
                            output.Append(ch);
                        break;
                }
            }

            return output.ToString();
        }

        static string Repeat(string str, int count)
        {
            return new StringBuilder().Insert(0, str, count).ToString();
        }

        static bool IsEscaped(string str, int index)
        {
            bool escaped = false;
            while (index > 0 && str[--index] == '\\') escaped = !escaped;
            return escaped;
        }

        static bool IsEscaped(StringBuilder str, int index)
        {
            return IsEscaped(str.ToString(), index);
        }
    }
}
