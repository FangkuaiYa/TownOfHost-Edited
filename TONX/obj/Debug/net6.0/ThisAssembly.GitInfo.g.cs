// <auto-generated />
#define GLOBALNAMESPACE
#define NOMETADATA
#pragma warning disable 0436

#if ADDMETADATA
[assembly: System.Reflection.AssemblyMetadata("GitInfo.IsDirty", ThisAssembly.Git.IsDirtyString)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.RepositoryUrl", ThisAssembly.Git.RepositoryUrl)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.Branch", ThisAssembly.Git.Branch)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.Commit", ThisAssembly.Git.Commit)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.Sha", ThisAssembly.Git.Sha)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.CommitDate", ThisAssembly.Git.CommitDate)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.BaseVersion.Major", ThisAssembly.Git.BaseVersion.Major)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.BaseVersion.Minor", ThisAssembly.Git.BaseVersion.Minor)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.BaseVersion.Patch", ThisAssembly.Git.BaseVersion.Patch)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.Commits", ThisAssembly.Git.Commits)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.Tag", ThisAssembly.Git.Tag)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.BaseTag", ThisAssembly.Git.BaseTag)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.SemVer.Major", ThisAssembly.Git.SemVer.Major)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.SemVer.Minor", ThisAssembly.Git.SemVer.Minor)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.SemVer.Patch", ThisAssembly.Git.SemVer.Patch)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.SemVer.Label", ThisAssembly.Git.SemVer.Label)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.SemVer.DashLabel", ThisAssembly.Git.SemVer.DashLabel)]
[assembly: System.Reflection.AssemblyMetadata("GitInfo.SemVer.Source", ThisAssembly.Git.SemVer.Source)]
#endif

#if LOCALNAMESPACE
namespace 
{
#endif
  /// <summary>Provides access to the current assembly information.</summary>
  partial class ThisAssembly
  {
    /// <summary>Provides access to the git information for the current assembly.</summary>
    public partial class Git
    {
      /// <summary>IsDirty: true</summary>
      public const bool IsDirty = true;

      /// <summary>IsDirtyString: true</summary>
      public const string IsDirtyString = @"true";

      /// <summary>Repository URL: </summary>
      public const string RepositoryUrl = @"";

      /// <summary>Branch: TOHE</summary>
      public const string Branch = @"TOHE";

      /// <summary>Commit: 3393dd3a</summary>
      public const string Commit = @"3393dd3a";

      /// <summary>Sha: 3393dd3a91c42bfd77bac3d3ac0fb2c891dbd8d4</summary>
      public const string Sha = @"3393dd3a91c42bfd77bac3d3ac0fb2c891dbd8d4";

      /// <summary>Commit date: 2023-06-27T00:15:30+08:00</summary>
      public const string CommitDate = @"2023-06-27T00:15:30+08:00";

      /// <summary>Commits on top of base version: 8511</summary>
      public const string Commits = @"8511";

      /// <summary>Tag: test_02-117-g3393dd3a</summary>
      public const string Tag = @"test_02-117-g3393dd3a";

      /// <summary>Base tag: test_02</summary>
      public const string BaseTag = @"test_02";

      /// <summary>Provides access to the base version information used to determine the <see cref="SemVer" />.</summary>      
      public partial class BaseVersion
      {
        /// <summary>Major: 0</summary>
        public const string Major = @"0";

        /// <summary>Minor: 0</summary>
        public const string Minor = @"0";

        /// <summary>Patch: 0</summary>
        public const string Patch = @"0";
      }

      /// <summary>Provides access to SemVer information for the current assembly.</summary>
      public partial class SemVer
      {
        /// <summary>Major: 0</summary>
        public const string Major = @"0";

        /// <summary>Minor: 0</summary>
        public const string Minor = @"0";

        /// <summary>Patch: 8511</summary>
        public const string Patch = @"8511";

        /// <summary>Label: </summary>
        public const string Label = @"";

        /// <summary>Label with dash prefix: </summary>
        public const string DashLabel = @"";

        /// <summary>Source: Default</summary>
        public const string Source = @"Default";
      }
    }
  }
#if LOCALNAMESPACE
}
#endif
#pragma warning restore 0436
