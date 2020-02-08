using System;

namespace Cicada.Core.Interfaces
{
    #region For MEF
    public interface IMEFOperation : ICloneable
    {
        CheckResult Operate(string checkId, DateTime checkDateTime, params string[] param);
    }

    public interface IPluginData
    {
        string PluginName { get; }
    }
    #endregion
}
