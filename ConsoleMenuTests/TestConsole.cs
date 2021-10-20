using ConsoleTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleMenuTests
{
  internal sealed class TestConsole : IConsole
  {
    private MemoryStream output;
    private StreamWriter outputWriter;
    private readonly Queue<Func<string>> GetUserInputs = new();
    public bool Details;

    public override string ToString()
    {
      var pos = this.output.Position;
      this.output.Position = 0;
      var reader = new StreamReader(this.output);
      string text = reader.ReadToEnd();
      this.output.Position = pos;
      return text;
    }

    public TestConsole()
    {
      this.output = new MemoryStream();
      this.outputWriter = new StreamWriter(output) { AutoFlush = true };
    }

    public bool IsOutputRedirected => throw new NotImplementedException();

    public int BufferHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int BufferWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool CapsLock => throw new NotImplementedException();

    public int CursorLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int CursorSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int CursorTop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool CursorVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TextWriter Error => throw new NotImplementedException();

    public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

    public Encoding InputEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool IsErrorRedirected => throw new NotImplementedException();

    public bool IsInputRedirected => throw new NotImplementedException();

    public int WindowTop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TextReader In => throw new NotImplementedException();

    public bool KeyAvailable => throw new NotImplementedException();

    public int LargestWindowWidth => throw new NotImplementedException();

    public int LargestWindowHeight => throw new NotImplementedException();

    public bool NumberLock => throw new NotImplementedException();

    public TextWriter Out => throw new NotImplementedException();

    public Encoding OutputEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Title { get; set; }
    public bool TreatControlCAsInput { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int WindowHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int WindowWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int WindowLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ConsoleColor BackgroundColor { get; set; }

    public event ConsoleCancelEventHandler CancelKeyPress;

    public void Beep()
    {
      throw new NotSupportedException("Test Console cannot do this operation.");
    }

    public void Beep(int frequency, int duration)
    {
      throw new NotSupportedException("Test Console cannot do this operation.");
    }

    public void Clear()
    {
      this.output.SetLength(0);
    }

    public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
    {
      throw new NotImplementedException();
    }

    public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
    {
      throw new NotImplementedException();
    }

    public Stream OpenStandardError()
    {
      throw new NotImplementedException();
    }

    public Stream OpenStandardInput()
    {
      throw new NotImplementedException();
    }

    public Stream OpenStandardOutput()
    {
      throw new NotImplementedException();
    }

    public int Read()
    {
      throw new NotImplementedException();
    }

    public void AddUserInput(string text)
    {
      GetUserInputs.Enqueue(() => text);
    }

    public void AddUserInputWithActionBefore(string text, Action action)
    {
      GetUserInputs.Enqueue(() =>
      {
        action();
        return text;
      });
    }

    public ConsoleKeyInfo ReadKey(bool intercept)
    {
      var line = this.GetUserInputs.Dequeue().Invoke();
      if (line.Length > 1)
      {
        throw new ArgumentException($"input should be single character but was `{line}`");
      }
      if (!intercept)
      {
        this.outputWriter.Write(line);
        this.outputWriter.WriteLine("\t//typed from keyboard");
      }
      return new ConsoleKeyInfo(line[0], (ConsoleKey)line[0], false, false, false);
    }

    public ConsoleKeyInfo ReadKey()
      => ReadKey(intercept: false);

    public string ReadLine()
    {
      var line = this.GetUserInputs.Dequeue().Invoke();
      this.outputWriter.Write(line);
      this.outputWriter.WriteLine("\t//typed from keyboard");
      return line;
    }

    public void ResetColor()
    {
      throw new NotImplementedException();
    }

    public void SetBufferSize(int width, int height)
    {
      throw new NotSupportedException("Test Console cannot do this operation.");
    }

    public void SetCursorPosition(int left, int top)
    {
      throw new NotSupportedException("Test Console cannot do this operation.");
    }

    public void SetError(TextWriter newError)
    {
      throw new NotSupportedException("Test Console cannot do this operation.");
    }

    public void SetIn(TextReader newIn)
    {
      this.output = new MemoryStream();
      this.outputWriter = new StreamWriter(this.output);
    }

    public void SetOut(TextWriter newOut)
    {
      if (!(newOut is StreamWriter streamWriter))
      {
        throw new NotSupportedException(newOut.ToString());
      }
      this.output = (MemoryStream)streamWriter.BaseStream;
      this.outputWriter = streamWriter;
    }

    public void SetWindowPosition(int left, int top)
    {
      throw new NotSupportedException("Test Console cannot do this operation.");
    }

    public void SetWindowSize(int width, int height)
    {
      throw new NotSupportedException("Test Console cannot do this operation.");
    }

    public void Write(char[] buffer, int index, int count)
      => this.Write(new string(buffer, index, count));

    public void Write(char[] buffer)
      => this.Write(new string(buffer));

    public void Write(float value)
      => this.Write(value.ToString());

    public void Write(bool value)
      => this.Write(value.ToString());

    public void Write(decimal value)
      => this.Write(value.ToString());

    public void Write(char value)
      => this.Write(value.ToString());

    public void Write(double value)
      => this.Write(value.ToString());

    public void Write(int value)
      => this.Write(value.ToString());

    public void Write(long value)
      => this.Write(value.ToString());

    public void Write(string value)
    {
      this.outputWriter.Write(value);

      if (this.Details)
      {
        this.outputWriter.Write($"<fc: {ForegroundColor}, bc: {BackgroundColor}>");
      }
    }

    public void Write(string format, object arg0)
      => this.Write(string.Format(format, arg0));

    public void Write(string format, object arg0, object arg1)
      => this.Write(string.Format(format, arg0, arg1));

    public void Write(string format, object arg0, object arg1, object arg2)
      => this.Write(string.Format(format, arg0, arg1, arg2));

    public void Write(string format, params object[] arg)
      => this.Write(string.Format(format, arg));

    public void Write(uint value)
      => this.Write(value.ToString());

    public void Write(ulong value)
      => this.Write(value.ToString());

    public void Write(object value)
      => this.Write(value.ToString());

    public void WriteLine()
      => this.outputWriter.WriteLine();

    public void WriteLine(bool value)
      => this.WriteLine(value.ToString());

    public void WriteLine(char value)
      => this.WriteLine(value.ToString());

    public void WriteLine(char[] buffer)
      => this.WriteLine(new string(buffer));

    public void WriteLine(ulong value)
      => this.WriteLine(value.ToString());

    public void WriteLine(double value)
      => this.WriteLine(value.ToString());

    public void WriteLine(int value)
      => this.WriteLine(value.ToString());

    public void WriteLine(long value)
      => this.WriteLine(value.ToString());

    public void WriteLine(object value)
      => this.WriteLine(value.ToString());

    public void WriteLine(float value)
      => this.WriteLine(value.ToString());

    public void WriteLine(string value)
    {
      if (this.Details)
      {
        this.outputWriter.Write(value);
        this.outputWriter.Write($"<fc: {ForegroundColor}, bc: {BackgroundColor}>");
        this.outputWriter.WriteLine();
      }
      else
      {
        this.outputWriter.WriteLine(value);
      }
    }

    public void WriteLine(string format, object arg0)
      => this.WriteLine(string.Format(format, arg0));

    public void WriteLine(string format, object arg0, object arg1)
      => this.WriteLine(string.Format(format, arg0, arg1));

    public void WriteLine(string format, object arg0, object arg1, object arg2)
      => this.WriteLine(string.Format(format, arg0, arg1, arg2));

    public void WriteLine(string format, params object[] arg)
      => this.WriteLine(string.Format(format, arg));

    public void WriteLine(uint value)
      => this.WriteLine(value.ToString());

    public void WriteLine(decimal value)
      => this.WriteLine(value.ToString());

    public void WriteLine(char[] buffer, int index, int count)
      => this.WriteLine(new string(buffer, index, count));
  }
}
