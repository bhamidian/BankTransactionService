using System.Collections;
using System.Reflection;

namespace LibraryMS.Framework;
public static class ConsolePainter
{
    public static void RedMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public static void YellowMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void GreenMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void CyanMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Write(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        var originalForeground = Console.ForegroundColor;
        var originalBackground = Console.BackgroundColor;

        if (foreground.HasValue)
            Console.ForegroundColor = foreground.Value;
        if (background.HasValue)
            Console.BackgroundColor = background.Value;

        Console.Write(text);

        Console.ForegroundColor = originalForeground;
        Console.BackgroundColor = originalBackground;
    }

    public static void WriteLine(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        Write(text, foreground, background);
        Console.WriteLine();
    }

    public static void WriteTable(IEnumerable items, ConsoleColor? headerColor = null, ConsoleColor? rowColor = null, int? maxColumnWidth = null)
    {
        var headerClr = headerColor ?? ConsoleColor.White;
        var rowClr = rowColor ?? ConsoleColor.White;

        var itemList = items.Cast<object?>().Where(x => x != null).ToList();
        if (!itemList.Any())
        {
            Console.WriteLine("(no data)");
            return;
        }

        var firstNonNull = itemList.First();
        var itemType = firstNonNull?.GetType();

        WriteLine($"Class: {itemType?.Name}");

        if (itemType != null && IsSimpleType(itemType))
        {
            string header = "Value";
            int maxLen = Math.Max(header.Length, itemList.Max(x => x?.ToString()?.Length ?? 0));
            string divider = "+" + new string('-', maxLen + 2) + "+";

            WriteLine(divider, headerClr);
            WriteRowWrapped(new[] { header }, new[] { maxLen }, headerClr, false);
            WriteLine(divider, headerClr);
            foreach (var item in itemList)
            {
                WriteRowWrapped(new[] { item?.ToString() ?? "" }, new[] { maxLen }, rowClr, true);
                WriteLine(divider, headerClr);
            }
            return;
        }

        if (itemType != null)
        {
            var props = GetOrderedPropertiesByInheritance(itemType);
            var headers = props.Select(p => p.Name).ToArray();

            // استخراج مقادیر سطرها
            var rows = itemList.Select(item =>
            {
                return props.Select(p =>
                {
                    try
                    {
                        var val = p.GetValue(item);

                        // Mask کردن پسورد
                        if (p.Name.Equals("Password", StringComparison.OrdinalIgnoreCase))
                            return "***";

                        if (val == null)
                            return "";

                        if (val is IEnumerable enumerable && val is not string)
                        {
                            var list = new List<string>();
                            foreach (var obj in enumerable)
                                list.Add(obj?.ToString() ?? "");
                            return string.Join(", ", list);
                        }

                        return val.ToString() ?? "";
                    }
                    catch
                    {
                        return "";
                    }
                }).ToArray();
            }).ToList();

            int colCount = headers.Length;

            // تعیین سقف عرض ستون‌ها
            int consoleWidth;
            try { consoleWidth = Console.WindowWidth; } catch { consoleWidth = 120; }
            int avgCap = Math.Max(10, (consoleWidth - (3 * colCount + 1)) / Math.Max(1, colCount));
            int perColCap = maxColumnWidth.HasValue ? Math.Min(maxColumnWidth.Value, avgCap) : avgCap;

            // محاسبه عرض هر ستون با در نظر گرفتن سقف و حداقل طول عنوان
            int[] maxWidths = new int[colCount];
            for (int i = 0; i < colCount; i++)
            {
                int headerWidth = headers[i].Length;
                int maxCell = rows.Select(r => r[i]?.Length ?? 0).DefaultIfEmpty(0).Max();
                maxWidths[i] = Math.Min(Math.Max(headerWidth, maxCell), perColCap);
            }

            string tableDivider = "+" + string.Join("+", maxWidths.Select(w => new string('-', w + 2))) + "+";

            // Header (هیچ عنوانی پیچیده نمی‌شود)
            WriteLine(tableDivider, headerClr);
            WriteRowWrapped(headers, maxWidths, headerClr, false);
            WriteLine(tableDivider, headerClr);

            // Rows (محتویات پیچیده می‌شوند)
            foreach (var row in rows)
            {
                WriteRowWrapped(row, maxWidths, rowClr, true);
                WriteLine(tableDivider, headerClr);
            }
        }

        // --- Local Functions ---
        static void WriteRowWrapped(IEnumerable<string> cols, int[] widths, ConsoleColor? fg, bool wrap)
        {
            var colList = cols.ToList();
            var wrappedCols = new List<List<string>>(colList.Count);

            for (int i = 0; i < colList.Count; i++)
                wrappedCols.Add(wrap ? WrapText(colList[i] ?? "", widths[i]) : new List<string> { colList[i] ?? "" });

            int maxLines = wrappedCols.Max(c => c.Count);

            for (int lineIndex = 0; lineIndex < maxLines; lineIndex++)
            {
                var lineCells = new List<string>(wrappedCols.Count);
                for (int c = 0; c < wrappedCols.Count; c++)
                {
                    string line = lineIndex < wrappedCols[c].Count ? wrappedCols[c][lineIndex] : "";
                    lineCells.Add(" " + line.PadRight(widths[c]) + " ");
                }
                Write("|" + string.Join("|", lineCells) + "|", fg);
                Console.WriteLine();
            }
        }

        static List<string> WrapText(string text, int width)
        {
            if (width < 1) width = 1;
            var result = new List<string>();
            if (string.IsNullOrEmpty(text))
            {
                result.Add("");
                return result;
            }

            text = text.Replace("\r", "");
            var paragraphs = text.Split('\n');

            foreach (var para in paragraphs)
            {
                var words = para.Split(' ');
                var line = "";

                foreach (var w in words)
                {
                    var word = w;

                    while (word.Length > width)
                    {
                        if (line.Length > 0)
                        {
                            result.Add(line);
                            line = "";
                        }
                        result.Add(word.Substring(0, width));
                        word = word.Substring(width);
                    }

                    if (line.Length == 0)
                    {
                        line = word;
                    }
                    else if (line.Length + 1 + word.Length <= width)
                    {
                        line += " " + word;
                    }
                    else
                    {
                        result.Add(line);
                        line = word;
                    }
                }

                result.Add(line);
            }

            if (result.Count == 0) result.Add("");
            return result;
        }
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive || type.IsEnum ||
               type == typeof(string) ||
               type == typeof(DateTime) ||
               type == typeof(decimal);
    }

    private static List<PropertyInfo> GetOrderedPropertiesByInheritance(Type type)
    {
        var props = new List<PropertyInfo>();
        var typeStack = new Stack<Type>();

        while (type != null && type != typeof(object))
        {
            typeStack.Push(type);
            type = type.BaseType!;
        }

        while (typeStack.Count > 0)
        {
            var t = typeStack.Pop();
            props.AddRange(t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
        }

        return props;
    }
}
