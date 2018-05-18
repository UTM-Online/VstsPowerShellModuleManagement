// ***********************************************************************
// Assembly         : VstsModuleManagementCore
// Author           : Josh Irwin (joirwi)
// Created          : 05-17-2018
//
// Last Modified By : Josh Irwin (joirwi)
// Last Modified On : 05-17-2018
// ***********************************************************************
// <copyright file="AbstractBaseCmdlet.cs" company="Microsoft">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace VstsModuleManagementCore.Cmdlets
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;

    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Resources;

    /// <summary>
    ///     Class AbstractBaseCmdlet.
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    public abstract class AbstractBaseCmdlet : PSCmdlet
    {
        /// <summary>
        ///     When overridden in the derived class, performs initialization
        ///     of command execution.
        ///     Default implementation in the base class just returns.
        /// </summary>
        protected override sealed void BeginProcessing()
        {
            this.BeginProcessingCommand();
        }

        /// <summary>
        ///     Begins the processing command.
        /// </summary>
        protected virtual void BeginProcessingCommand()
        {
        }

        /// <summary>
        ///     Creates the parameter dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        protected Dictionary<string, object> CreateParameterDictionary(string key = null, string value = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new Dictionary<string, object>();
            }

            return new Dictionary<string, object>
                       {
                          { key, value } 
                       };
        }

        /// <summary>
        ///     When overridden in the derived class, performs clean-up
        ///     after the command execution.
        ///     Default implementation in the base class just returns.
        /// </summary>
        protected override sealed void EndProcessing()
        {
            this.EndProcessingCommand();
        }

        /// <summary>
        ///     Ends the processing command.
        /// </summary>
        protected virtual void EndProcessingCommand()
        {
        }

        /// <summary>
        ///     Gets the ps variable.
        /// </summary>
        /// <typeparam name="T">The type of the variable being retrieved</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <c>object</c> of type <typeparamref name="T" />.</returns>
        protected T GetPsVariable<T>(string name, object defaultValue = null)
        {
            return (T)this.GetVariableValue(name, defaultValue);
        }

        /// <summary>
        ///     Gets the run time module settings.
        /// </summary>
        /// <returns>The current ModuleSettings.</returns>
        protected ModuleSettings GetRunTimeModuleSettings()
        {
            return this.GetPsVariable<ModuleSettings>(ModuleVariables.ModuleSettings);
        }

        /// <summary>
        ///     Processes the command.
        /// </summary>
        protected abstract void ProcessCommand();

        /// <summary>
        ///     When overridden in the derived class, performs execution
        ///     of the command.
        /// </summary>
        protected override sealed void ProcessRecord()
        {
            this.ProcessCommand();
        }

        /// <summary>
        ///     Sets the ps variable.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="name" /> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     If the provider that the <paramref name="name" /> refers to does
        ///     not support this operation.
        /// </exception>
        /// <exception cref="SessionStateUnauthorizedAccessException">
        ///     If the variable is read-only or constant.
        /// </exception>
        /// <exception cref="SessionStateOverflowException">
        ///     If the maximum number of variables has been reached for this scope.
        /// </exception>
        /// <exception cref="ProviderNotFoundException">
        ///     If the <paramref name="name" /> refers to a provider that could not be found.
        /// </exception>
        /// <exception cref="DriveNotFoundException">
        ///     If the <paramref name="name" /> refers to a drive that could not be found.
        /// </exception>
        /// <exception cref="ProviderInvocationException">
        ///     If the provider threw an exception.
        /// </exception>
        protected void SetPsVariable(string name, object value)
        {
            this.SessionState.PSVariable.Set(name, value);
        }
    }
}