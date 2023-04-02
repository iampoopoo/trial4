namespace Glekcraft.GLFW.Tests.Mocks;

/// <summary>
/// A class that can be used to mock the native APIs.
/// </summary>
public class NativeAPIs : INativeAPIs {
    #region Public Properties

    public Version GetVersionResult {
        get;
        set;
    } = new Version(3, 3, 8);

    public string GetVersionStringResult {
        get;
        set;
    } = "3.3.8";

    public int? InitHintInput {
        get;
        set;
    }

    public int? InitHintValueInput {
        get;
        set;
    }

    public bool InitResult {
        get;
        set;
    } = true;

    public int GetErrorResult {
        get;
        set;
    }

    public string? GetErrorDescription {
        get;
        set;
    }

    public INativeAPIs.ErrorCallback? ErrorCallbackInput {
        get;
        set;
    }

    public INativeAPIs.ErrorCallback? ErrorCallbackResult {
        get;
        set;
    }

    #endregion

    #region Public Methods

    public void Reset() {
        GetVersionResult = new Version(3, 3, 8);
        GetVersionStringResult = "3.3.8";
        InitHintInput = null;
        InitHintValueInput = null;
        InitResult = true;
        GetErrorResult = 0;
        GetErrorDescription = null;
        ErrorCallbackInput = null;
        ErrorCallbackResult = null;
    }

    public Version GetVersion() =>
        GetVersionResult;

    public string GetVersionString() =>
        GetVersionStringResult;

    public void InitHint(int hint, int value) {
        InitHintInput = hint;
        InitHintValueInput = value;
    }

    public void InitHint(int hint, bool value) {
        InitHintInput = hint;
        InitHintValueInput = value ? 1 : 0;
    }

    public bool Init() =>
        InitResult;

    public void Terminate() {
        //-- Does nothing
    }

    public int GetError(out string? description) {
        description = GetErrorDescription;
        return GetErrorResult;
    }

    public INativeAPIs.ErrorCallback? SetErrorCallback(INativeAPIs.ErrorCallback? callback) {
        ErrorCallbackInput = callback;
        return ErrorCallbackResult;
    }

    #endregion
}
