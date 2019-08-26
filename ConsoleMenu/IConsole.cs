using System;
using System.IO;
using System.Text;

namespace ConsoleTools
{
  internal interface IConsole
  {
    bool IsOutputRedirected { get; }
    int BufferHeight { get; set; }
    int BufferWidth { get; set; }
    bool CapsLock { get; }
    int CursorLeft { get; set; }
    int CursorSize { get; set; }
    int CursorTop { get; set; }
    bool CursorVisible { get; set; }
    TextWriter Error { get; }
    ConsoleColor ForegroundColor { get; set; }
    Encoding InputEncoding { get; set; }
    bool IsErrorRedirected { get; }
    bool IsInputRedirected { get; }
    int WindowTop { get; set; }
    TextReader In { get; }
    bool KeyAvailable { get; }
    int LargestWindowWidth { get; }
    int LargestWindowHeight { get; }
    bool NumberLock { get; }
    TextWriter Out { get; }
    Encoding OutputEncoding { get; set; }
    string Title { get; set; }
    bool TreatControlCAsInput { get; set; }
    int WindowHeight { get; set; }
    int WindowWidth { get; set; }
    int WindowLeft { get; set; }
    ConsoleColor BackgroundColor { get; set; }
    event ConsoleCancelEventHandler CancelKeyPress;
    void Beep();
    void Beep(int frequency, int duration);
    void Clear();
    void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop);
    void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor);
    Stream OpenStandardError();
    Stream OpenStandardInput();
    Stream OpenStandardOutput();
    int Read();
    ConsoleKeyInfo ReadKey(bool intercept);
    ConsoleKeyInfo ReadKey();
    string ReadLine();
    void ResetColor();
    void SetBufferSize(int width, int height);
    void SetCursorPosition(int left, int top);
    void SetError(TextWriter newError);
    void SetIn(TextReader newIn);
    void SetOut(TextWriter newOut);
    void SetWindowPosition(int left, int top);
    void SetWindowSize(int width, int height);
    void Write(char[] buffer, int index, int count);
    void Write(char[] buffer);
    void Write(float value);
    void Write(bool value);
    void Write(decimal value);
    void Write(char value);
    void Write(double value);
    void Write(int value);
    void Write(long value);
    void Write(string value);
    void Write(string format, object arg0);
    void Write(string format, object arg0, object arg1);
    void Write(string format, object arg0, object arg1, object arg2);
    void Write(string format, params object[] arg);
    void Write(uint value);
    void Write(ulong value);
    void Write(object value);
    void WriteLine();
    void WriteLine(bool value);
    void WriteLine(char value);
    void WriteLine(char[] buffer);
    void WriteLine(ulong value);
    void WriteLine(double value);
    void WriteLine(int value);
    void WriteLine(long value);
    void WriteLine(object value);
    void WriteLine(float value);
    void WriteLine(string value);
    void WriteLine(string format, object arg0);
    void WriteLine(string format, object arg0, object arg1);
    void WriteLine(string format, object arg0, object arg1, object arg2);
    void WriteLine(string format, params object[] arg);
    void WriteLine(uint value);
    void WriteLine(decimal value);
    void WriteLine(char[] buffer, int index, int count);
  }
}