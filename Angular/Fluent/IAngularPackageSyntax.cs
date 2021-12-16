namespace KY.Generator.Angular.Fluent
{
    public interface IAngularPackageSyntax : IAngularWriteBaseSyntax<IAngularPackageSyntax>
    {
        /// <summary>
        /// Set the name of the packages (scope is optional) e.g. @scope/my-lib <code>[REQUIRED]</code>
        /// </summary>
        IAngularPackageSyntax Name(string packageName);

        /// <summary>
        /// Set the version of the package (semver is allowed) <code>[REQUIRED]</code>
        /// </summary>
        IAngularPackageSyntax Version(string version);

        /// <summary>
        /// Reads the latest version from npm
        /// </summary>
        IAngularPackageSyntax VersionFromNpm();

        /// <summary>
        /// Increments the major (<b>1</b>.0.0) version of the package when changes are found
        /// </summary>
        IAngularPackageSyntax IncrementMajorVersion();

        /// <summary>
        /// Increments the minor (0.<b>1</b>.0) version of the package when changes are found
        /// </summary>
        IAngularPackageSyntax IncrementMinorVersion();

        /// <summary>
        /// Increments the patch/bugfix/revision (0.0.<b>1</b>) version of the package when changes are found
        /// </summary>
        IAngularPackageSyntax IncrementPatchVersion();

        /// <summary>
        /// Set the version of the angular cli (semver is allowed)
        /// </summary>
        IAngularPackageSyntax CliVersion(string version);

        /// <summary>
        /// Adds a peer dependency to the package. Version is required and can look like: ^1.2.3
        /// </summary>
        IAngularPackageSyntax DependsOn(string packageName, string version);

        /// <inheritdoc cref="IAngularModelSyntax.OutputPath"/>
        IAngularPackageSyntax OutputPath(string path);

        /// <summary>
        /// Builds the package after generation is done
        /// </summary>
        IAngularPackageSyntax Build();

        /// <summary>
        /// Builds and publish the package after generation is done
        /// </summary>
        IAngularPackageSyntax Publish();

        /// <summary>
        /// Builds and publish the package to the root folder of the package after generation is done
        /// </summary>
        IAngularPackageSyntax PublishLocal();
    }
}
