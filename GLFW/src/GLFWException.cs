namespace Glekcraft.GLFW;

using System.Runtime.Serialization;

[Serializable]
public class GLFWException : Exception {
    #region Public Properties

    public int? NativeErrorCode {
        get;
        private set;
    }

    public string? NativeErrorDescription {
        get;
        private set;
    }

    #endregion

    #region Constructors/Finalizer

    public GLFWException() : base() {
        NativeErrorCode = LibGLFW.LastErrorCode;
        NativeErrorDescription = LibGLFW.LastErrorDescription;
    }

    public GLFWException(string message) : base(message) {
        NativeErrorCode = LibGLFW.LastErrorCode;
        NativeErrorDescription = LibGLFW.LastErrorDescription;
    }

    public GLFWException(string message, Exception inner) : base(message, inner) {
        NativeErrorCode = LibGLFW.LastErrorCode;
        NativeErrorDescription = LibGLFW.LastErrorDescription;
    }

    protected GLFWException(SerializationInfo info, StreamingContext context) : base(info, context) {
        if (info.GetBoolean("HasNativeErrorCode")) {
            NativeErrorCode = info.GetInt32("NativeErrorCode");
        }
        if (info.GetBoolean("HasNativeErrorDescription")) {
            NativeErrorDescription = info.GetString("NativeErrorDescription");
        }
    }

    #endregion

    #region Public Methods

    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
        base.GetObjectData(info, context);
        info.AddValue("HasNativeErrorCode", NativeErrorCode != null);
        if (NativeErrorCode != null) {
            info.AddValue("NativeErrorCode", NativeErrorCode.Value);
        }
        info.AddValue("HasNativeErrorDescription", NativeErrorDescription != null);
        if (NativeErrorDescription != null) {
            info.AddValue("NativeErrorDescription", NativeErrorDescription);
        }
    }

    #endregion
}
