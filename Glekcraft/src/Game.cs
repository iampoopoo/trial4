namespace Glekcraft;

using System.Drawing;

using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

/// <summary>
/// The main game state/logic controller.
/// </summary>
public class Game : IDisposable {
    #region Private Properties

    private uint vao;

    private uint vbo;

    private uint ebo;

    private uint shader;

    #endregion

    #region Public Properties

    /// <summary>
    /// The main game window.
    /// </summary>
    public IWindow? MainWindow {
        get;
        private set;
    }

    /// <summary>
    /// The main game window input context.
    /// </summary>
    public IInputContext? MainWindowInput {
        get;
        private set;
    }

    /// <summary>
    /// The main game window graphics context.
    /// </summary>
    public GL? MainWindowGraphics {
        get;
        private set;
    }

    /// <summary>
    /// Whether the instance has been disposed.
    /// </summary>
    public bool IsDisposed {
        get;
        private set;
    }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// The finalizer.
    /// </summary>
    ~Game() =>
        Dispose(false);

    #endregion

    #region Public Methods

    /// <summary>
    /// Initialize the instance.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the instance is already initialized.
    /// </exception>
    public void Initialize() {
        if (MainWindow != null) {
            throw new InvalidOperationException("Instance is already initialized");
        }
        var graphicsOptions = GraphicsAPI.Default;
        graphicsOptions.API = ContextAPI.OpenGL;
        graphicsOptions.Version = new(4, 5);
        graphicsOptions.Flags = ContextFlags.ForwardCompatible;
        graphicsOptions.Profile = ContextProfile.Core;
        var windowOptions = WindowOptions.Default;
        windowOptions.API = graphicsOptions;
        windowOptions.Title = "Glekcraft";
        windowOptions.WindowClass = "Glekcraft";
        windowOptions.IsVisible = false;
        windowOptions.WindowBorder = WindowBorder.Resizable;
        windowOptions.WindowState = WindowState.Normal;
        windowOptions.Size = new(640, 480);
        windowOptions.FramesPerSecond = 60;
        windowOptions.UpdatesPerSecond = 60;
        windowOptions.ShouldSwapAutomatically = false;
        windowOptions.VSync = false;
        windowOptions.Samples = 0;
        MainWindow = Window.Create(windowOptions);
        MainWindow.Load += OnWindowLoad;
        MainWindow.Closing += OnWindowClosing;
        MainWindow.Update += OnWindowUpdate;
        MainWindow.Render += OnWindowRender;
    }

    /// <summary>
    /// Run the instance.
    /// </summary>
    public void Run() {
        if (MainWindow == null) {
            throw new InvalidOperationException("Instance is not initialized");
        }
        MainWindow.Run();
    }

    /// <summary>
    /// Dispose of this instance.
    /// </summary>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// The callback for when the main window is loaded.
    /// </summary>
    private void OnWindowLoad() {
        if (MainWindow == null) {
            throw new InvalidOperationException("Main game window is null");
        }
        MainWindowInput = MainWindow.CreateInput();
        MainWindowGraphics = MainWindow.CreateOpenGL();
        MainWindow.Center();
        MainWindow.IsVisible = true;
        vao = MainWindowGraphics.GenVertexArray();
        MainWindowGraphics.BindVertexArray(vao);

        vbo = MainWindowGraphics.CreateBuffer();
        MainWindowGraphics.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        MainWindowGraphics.BufferData<float>(BufferTargetARB.ArrayBuffer, new float[] {
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f, 0.5f, 0.0f
        }, BufferUsageARB.StaticDraw);

        ebo = MainWindowGraphics.CreateBuffer();
        MainWindowGraphics.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
        MainWindowGraphics.BufferData<uint>(BufferTargetARB.ElementArrayBuffer, new uint[] {
            0, 1, 2
        }, BufferUsageARB.StaticDraw);

        shader = MainWindowGraphics.CreateProgram();
        var vShader = MainWindowGraphics.CreateShader(ShaderType.VertexShader);
        var fShader = MainWindowGraphics.CreateShader(ShaderType.FragmentShader);
        MainWindowGraphics.ShaderSource(vShader, @"
#version 450 core
layout (location = 0) in vec3 aPosition;
void main() {
    gl_Position = vec4(aPosition, 1.0);
}
        ");
        MainWindowGraphics.ShaderSource(fShader, @"
#version 450 core
out vec4 fragColor;
void main() {
    fragColor = vec4(1.0, 0.0, 0.0, 1.0);
}
        ");
        MainWindowGraphics.CompileShader(vShader);
        MainWindowGraphics.CompileShader(fShader);
        MainWindowGraphics.AttachShader(shader, vShader);
        MainWindowGraphics.AttachShader(shader, fShader);
        MainWindowGraphics.LinkProgram(shader);
        MainWindowGraphics.DeleteShader(vShader);
        MainWindowGraphics.DeleteShader(fShader);
    }

    /// <summary>
    /// The callback for when the main window is requesting to closing.
    /// </summary>
    private void OnWindowClosing() =>
        Dispose();

    /// <summary>
    /// The callback for when the main window is updated.
    /// </summary>
    /// <param name="delta">
    /// The time, in fractional seconds, since the last update.
    /// </param>
    private void OnWindowUpdate(double delta) {
        if (MainWindow == null) {
            throw new InvalidOperationException("Main game window is null");
        }
        if (MainWindowInput == null) {
            throw new InvalidOperationException("Main game window input is null");
        }
        foreach (var kb in MainWindowInput.Keyboards) {
            if (kb.IsKeyPressed(Key.Escape)) {
                MainWindow?.Close();
            }
        }
    }

    /// <summary>
    /// The callback for when the main window is rendered.
    /// </summary>
    /// <param name="delta">
    /// The time, in fractional seconds, since the last render.
    /// </param>
    private void OnWindowRender(double delta) {
        if (MainWindow == null) {
            throw new InvalidOperationException("Main game window is null");
        }
        if (MainWindowGraphics == null) {
            throw new InvalidOperationException("Main game window graphics is null");
        }
        MainWindowGraphics.Viewport(MainWindow?.FramebufferSize ?? new(0, 0));
        MainWindowGraphics.ClearColor(Color.CornflowerBlue);
        MainWindowGraphics.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        MainWindowGraphics.BindVertexArray(vao);
        MainWindowGraphics.UseProgram(shader);
        MainWindowGraphics.EnableVertexAttribArray(0);
        MainWindowGraphics.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        unsafe {
            MainWindowGraphics.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
        }
        MainWindowGraphics.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
        unsafe {
            MainWindowGraphics.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, null);
        }
        MainWindowGraphics.DisableVertexAttribArray(0);
        MainWindowGraphics.UseProgram(0);

        //-- I don't know why C# thinks this is null...
        MainWindow?.SwapBuffers();
    }

    /// <summary>
    /// Dispose of this instance.
    /// </summary>
    /// <param name="managed">
    /// Whether this method is being called from managed code or from unmanaged
    /// code (e.g. the garbage collector).
    /// </param>
    private void Dispose(bool managed) {
        if (IsDisposed) {
            return;
        }
        if (managed) {
            MainWindowGraphics?.Dispose();
            MainWindowGraphics = null;
            MainWindowInput?.Dispose();
            MainWindowInput = null;
            MainWindow?.Dispose();
            MainWindow = null;
        }
        IsDisposed = true;
    }

    #endregion
}
