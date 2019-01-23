namespace TiKuService
{
  partial class ProjectInstaller
  {
    /// <summary>
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// 清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region 组件设计器生成的代码

    /// <summary>
    /// 设计器支持所需的方法 - 不要
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
      this.azgbPi = new System.ServiceProcess.ServiceProcessInstaller();
      this.siAzgb = new System.ServiceProcess.ServiceInstaller();
      // 
      // azgbPi
      // 
      this.azgbPi.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
      this.azgbPi.Password = null;
      this.azgbPi.Username = null;
      // 
      // siAzgb
      // 
      this.siAzgb.Description = "tikuServer";
      this.siAzgb.DisplayName = "tikuServer";
      this.siAzgb.ServiceName = "tikuServer";
      this.siAzgb.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
      // 
      // ProjectInstaller
      // 
      this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.azgbPi,
            this.siAzgb});

    }

    #endregion

    private System.ServiceProcess.ServiceProcessInstaller azgbPi;
    private System.ServiceProcess.ServiceInstaller siAzgb;
  }
}