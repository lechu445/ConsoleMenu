using System;
using System.IO;
using System.Text;

namespace ConsoleTools;

internal sealed class SystemConsole : IConsole
{
  public event ConsoleCancelEventHandler? CancelKeyPress;

  public bool IsOutputRedirected => Console.IsOutputRedirected;

  public int BufferHeight { get => Console.BufferHeight; set => Console.BufferHeight = value; }

  public int BufferWidth { get => Console.BufferWidth; set => Console.BufferWidth = value; }

  public bool CapsLock => Console.CapsLock;

  public int CursorLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public int CursorSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public int CursorTop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public bool CursorVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public TextWriter Error => throw new NotImplementedException();

  public ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }

  public Encoding InputEncoding { get => Console.InputEncoding; set => Console.InputEncoding = value; }

  public bool IsErrorRedirected => throw new NotImplementedException();

  public bool IsInputRedirected => throw new NotImplementedException();

  public int WindowTop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public TextReader In => throw new NotImplementedException();

  public bool KeyAvailable => throw new NotImplementedException();

  public int LargestWindowWidth => throw new NotImplementedException();

  public int LargestWindowHeight => throw new NotImplementedException();

  public bool NumberLock => throw new NotImplementedException();

  public TextWriter Out => throw new NotImplementedException();

  public Encoding OutputEncoding { get => Console.OutputEncoding; set => Console.OutputEncoding = value; }

  public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public bool TreatControlCAsInput { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public int WindowHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public int WindowWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public int WindowLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }

  public void Beep()
  {
    throw new NotImplementedException();
  }

  public void Beep(int frequency, int duration)
  {
    throw new NotImplementedException();
  }

  public void Clear() => Console.Clear();

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
    => Console.Read();

  public ConsoleKeyInfo ReadKey(bool intercept)
    => Console.ReadKey(intercept);

  public ConsoleKeyInfo ReadKey()
    => Console.ReadKey();

  public string? ReadLine()
    => Console.ReadLine();

  public void ResetColor()
    => Console.ResetColor();

  public void SetBufferSize(int width, int height)
  {
    throw new NotImplementedException();
  }

  public void SetCursorPosition(int left, int top)
  {
    throw new NotImplementedException();
  }

  public void SetError(TextWriter newError)
  {
    throw new NotImplementedException();
  }

  public void SetIn(TextReader newIn)
  {
    throw new NotImplementedException();
  }

  public void SetOut(TextWriter newOut)
  {
    throw new NotImplementedException();
  }

  public void SetWindowPosition(int left, int top)
  {
    throw new NotImplementedException();
  }

  public void SetWindowSize(int width, int height)
  {
    throw new NotImplementedException();
  }

  public void Write(char[] buffer, int index, int count)
  {
    throw new NotImplementedException();
  }

  public void Write(char[] buffer)
    => Console.Write(buffer);

  public void Write(float value)
    => Console.Write(value);

  public void Write(bool value)
    => Console.Write(value);

  public void Write(decimal value)
    => Console.Write(value);

  public void Write(char value)
    => Console.Write(value);

  public void Write(double value)
    => Console.Write(value);

  public void Write(int value)
    => Console.Write(value);

  public void Write(long value)
    => Console.Write(value);

  public void Write(string value)
    => Console.Write(value);

  public void Write(string format, object arg0)
  {
    throw new NotImplementedException();
  }

  public void Write(string format, object arg0, object arg1)
  {
    throw new NotImplementedException();
  }

  public void Write(string format, object arg0, object arg1, object arg2)
  {
    throw new NotImplementedException();
  }

  public void Write(string format, params object[] arg)
  {
    throw new NotImplementedException();
  }

  public void Write(uint value)
    => Console.Write(value);

  public void Write(ulong value)
    => Console.Write(value);

  public void Write(object value)
    => Console.Write(value);

  public void WriteLine()
    => Console.WriteLine();

  public void WriteLine(bool value)
    => Console.WriteLine(value);

  public void WriteLine(char value)
    => Console.WriteLine(value);

  public void WriteLine(char[] buffer)
    => Console.WriteLine(buffer);

  public void WriteLine(ulong value)
    => Console.WriteLine(value);

  public void WriteLine(double value)
    => Console.WriteLine(value);

  public void WriteLine(int value)
    => Console.WriteLine(value);

  public void WriteLine(long value)
    => Console.WriteLine(value);

  public void WriteLine(object value)
    => Console.WriteLine(value);

  public void WriteLine(float value)
    => Console.WriteLine(value);

  public void WriteLine(string value)
    => Console.WriteLine(value);

  public void WriteLine(string format, object arg0)
  {
    throw new NotImplementedException();
  }

  public void WriteLine(string format, object arg0, object arg1)
  {
    throw new NotImplementedException();
  }

  public void WriteLine(string format, object arg0, object arg1, object arg2)
  {
    throw new NotImplementedException();
  }

  public void WriteLine(string format, params object[] arg)
  {
    throw new NotImplementedException();
  }

  public void WriteLine(uint value)
    => Console.WriteLine(value);

  public void WriteLine(decimal value)
    => Console.WriteLine(value);

  public void WriteLine(char[] buffer, int index, int count)
  {
    throw new NotImplementedException();
  }
}
