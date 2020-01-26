namespace AsmResolver.DotNet.Serialized
{
    /// <summary>
    /// Provides parameters for the reading process of a .NET module.
    /// </summary>
    public class ModuleReadParameters
    {
        /// <summary>
        /// Initializes the default module read parameters.
        /// </summary>
        public ModuleReadParameters()
        {
        }
        
        /// <summary>
        /// Initializes the module read parameters with a working directory.
        /// </summary>
        /// <param name="workingDirectory">The working directory of the modules to read.</param>
        public ModuleReadParameters(string workingDirectory)
        {
            ModuleResolver = new DirectoryNetModuleResolver(workingDirectory, this);
        }
        
        /// <summary>
        /// Gets or sets the object used for resolving a net module.
        /// </summary>
        public INetModuleResolver ModuleResolver
        {
            get;
            set;
        }
    }
}